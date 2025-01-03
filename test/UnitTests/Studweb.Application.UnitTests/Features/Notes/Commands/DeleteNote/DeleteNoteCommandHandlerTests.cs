using FluentAssertions;
using Moq;
using Studweb.Application.Features.Notes.Commands.DeleteNote;
using Studweb.Application.Persistance;
using Studweb.Application.UnitTests.TestUtils.Repository;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.UnitTests.Features.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandlerTests
{
    private readonly DeleteNoteCommandHandler _deleteNoteCommandHandler;
    private readonly Mock<IUserContext> _mockUserContext;
    private readonly Mock<INoteRepository> _mockNoteRepository;

    public DeleteNoteCommandHandlerTests()
    {
        _mockUserContext = new Mock<IUserContext>();
        _mockNoteRepository = new Mock<INoteRepository>();
        _deleteNoteCommandHandler = new DeleteNoteCommandHandler(
            _mockUserContext.Object,
            _mockNoteRepository.Object);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange

        var deleteNoteCommand = new DeleteNoteCommand(1);
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(value: null);
        
        // Act

        var result = await _deleteNoteCommandHandler
            .Handle(deleteNoteCommand, default);
        
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
                .DeleteAsync(It.IsAny<int>(), default), Times.Never);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorNotFound_WhenNoteWithThatIdDoesNotExist()
    {
        // Arrange
        
        var deleteNoteCommand = new DeleteNoteCommand(1);
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(value: null);
        
        // Act

        var result = await _deleteNoteCommandHandler
            .Handle(deleteNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.NotFound);
        
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .DeleteAsync(It.IsAny<int>(), default), Times.Never);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorAccessDenied_WhenUserDoesNotHaveAccessToTheNote()
    {
        // Arrange

        var deleteNoteCommand = new DeleteNoteCommand(1);
        var exampleNote = NoteRepositoryUtils.GetExampleNote();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(2);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(exampleNote);
        
        // Act

        var result = await _deleteNoteCommandHandler
            .Handle(deleteNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.AccessDenied);
        
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .DeleteAsync(It.IsAny<int>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorCannotDeleteNote_WhenRepositoryReturnsZeroAffectedRows()
    {
        // Arrange

        var deleteNoteCommand = new DeleteNoteCommand(1);
        var exampleNote = NoteRepositoryUtils.GetExampleNote();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(exampleNote);
        _mockNoteRepository
            .Setup(x => x
                .DeleteAsync(It.IsAny<int>(), default))
            .ReturnsAsync(0);

        // Act
        
        var result = await _deleteNoteCommandHandler
            .Handle(deleteNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.CannotDeleteNote);
        
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .DeleteAsync(It.IsAny<int>(), default), Times.Once);
    }
    
    [Fact]
    public async Task Handle_Should_DeleteNote_WhenNoteIsValid()
    {
        // Arrange
        
        var deleteNoteCommand = new DeleteNoteCommand(1);
        var exampleNote = NoteRepositoryUtils.GetExampleNote();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(exampleNote);
        _mockNoteRepository
            .Setup(x => x
                .DeleteAsync(It.IsAny<int>(), default))
            .ReturnsAsync(1);
        
        // Act
        
        var result = await _deleteNoteCommandHandler
            .Handle(deleteNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
        
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
        _mockNoteRepository
            .Verify(noteRepository => noteRepository
                .DeleteAsync(It.IsAny<int>(), default), Times.Once);
    }
    
}