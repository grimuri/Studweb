using FluentAssertions;
using Moq;
using Studweb.Application.Features.Notes.Commands.AddNote;
using Studweb.Application.Persistance;
using Studweb.Application.UnitTests.Features.Notes.Commands.AddNote.TestUtils;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.Note;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.UnitTests.Features.Notes.Commands.AddNote;

public class AddNoteCommandHandlerTests
{
    private readonly AddNoteCommandHandler _addNoteCommandHandler;
    private readonly Mock<IUserContext> _mockUserContext;
    private readonly Mock<INoteRepository> _mockNoteRepository;

    public AddNoteCommandHandlerTests()
    {
        _mockUserContext = new Mock<IUserContext>();
        _mockNoteRepository = new Mock<INoteRepository>();
        _addNoteCommandHandler = new AddNoteCommandHandler(
            _mockNoteRepository.Object,
            _mockUserContext.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange
        
        var addNoteCommand = AddNoteCommandUtils.AddNoteCommand();
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(value: null);
        
        // Act

        var result = await _addNoteCommandHandler.Handle(addNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
        
        _mockNoteRepository.Verify(noteRepository =>
                noteRepository.CreateAsync(
                    It.IsAny<Note>(),
                    default),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_CreateNoteAndReturnId_WhenNoteIsValid()
    {
        // Arrange

        var addNoteCommand = AddNoteCommandUtils.AddNoteCommand();
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x.CreateAsync(It.IsAny<Note>(), default))
            .ReturnsAsync(1);
        
        // Act

        var result = await _addNoteCommandHandler.Handle(addNoteCommand, default);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
        
        _mockNoteRepository.Verify(noteRepository =>
            noteRepository.CreateAsync(
                It.IsAny<Note>(),
                default),
            Times.Once);
    }
}