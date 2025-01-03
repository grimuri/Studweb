using FluentAssertions;
using Moq;
using Studweb.Application.Features.Notes.Commands.UpdateNote;
using Studweb.Application.Persistance;
using Studweb.Application.UnitTests.Features.Notes.Commands.UpdateNote.TestUtils;
using Studweb.Application.UnitTests.TestUtils.Repository;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.UnitTests.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandlerTests
{
    private readonly UpdateNoteCommandHandler _updateNoteCommandHandler;
    private readonly Mock<IUserContext> _mockUserContext;
    private readonly Mock<INoteRepository> _mockNoteRepository;

    public UpdateNoteCommandHandlerTests()
    {
        _mockUserContext = new Mock<IUserContext>();
        _mockNoteRepository = new Mock<INoteRepository>();
        _updateNoteCommandHandler = new UpdateNoteCommandHandler(
            _mockUserContext.Object,
            _mockNoteRepository.Object);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange

        var updateNoteCommand = UpdateNoteCommandUtils.UpdateNoteCommand();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(value: null);
        
        // Act

        var result = await _updateNoteCommandHandler
            .Handle(updateNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
        
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .GetByIdAsync(It.IsAny<int>(), default), Times.Never);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .UpdateAsync(It.IsAny<Note>(), default), Times.Never);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorNotFound_WhenNoteWithThatIdDoesNotExist()
    {
        // Arrange
        
        var updateNoteCommand = UpdateNoteCommandUtils.UpdateNoteCommand();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(value: null);
        
        // Act

        var result = await _updateNoteCommandHandler
            .Handle(updateNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.NotFound);
        
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(x => x
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .UpdateAsync(It.IsAny<Note>(), default), Times.Never);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorAccessDenied_WhenUserDoesNotHaveAccessToTheNote()
    {
        // Arrange

        var updateNoteCommand = UpdateNoteCommandUtils.UpdateNoteCommand();
        var exampleNote = NoteRepositoryUtils.GetExampleNote();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(2);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(exampleNote);
        
        // Act

        var result = await _updateNoteCommandHandler
            .Handle(updateNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.AccessDenied);
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(x => x
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .UpdateAsync(It.IsAny<Note>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_UpdateAndReturnNote_WhenNoteIsValid()
    {
        // Arrange
        
        var updateNoteCommand = UpdateNoteCommandUtils.UpdateNoteCommand();
        var exampleNote = NoteRepositoryUtils.GetExampleNote();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(exampleNote);
        
        // Act
        
        var result = await _updateNoteCommandHandler
            .Handle(updateNoteCommand, default);
        
        // Assert
        
        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(x => x
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .UpdateAsync(It.IsAny<Note>(), default), Times.Once);
    }
}