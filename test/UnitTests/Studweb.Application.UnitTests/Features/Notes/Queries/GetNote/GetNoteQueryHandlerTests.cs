using FluentAssertions;
using Moq;
using Studweb.Application.Features.Notes.Queries.GetNote;
using Studweb.Application.Persistance;
using Studweb.Application.UnitTests.Features.Notes.Queries.GetNote.TestUtils;
using Studweb.Application.UnitTests.TestUtils.Repository;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.UnitTests.Features.Notes.Queries.GetNote;

public class GetNoteQueryHandlerTests
{
    private readonly GetNoteQueryHandler _getNoteQueryHandler;
    private readonly Mock<IUserContext> _mockUserContext;
    private readonly Mock<INoteRepository> _mockNoteRepository;

    public GetNoteQueryHandlerTests()
    {
        _mockUserContext = new Mock<IUserContext>();
        _mockNoteRepository = new Mock<INoteRepository>();
        _getNoteQueryHandler = new GetNoteQueryHandler(
            _mockUserContext.Object,
            _mockNoteRepository.Object
            );
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange

        var getNoteQuery = GetNoteQueryUtils.GetNoteQuery();

        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(value: null);
        
        // Act

        var result = await _getNoteQueryHandler
            .Handle(getNoteQuery, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorNotFound_WhenNoteWithThatIdDoesNotExist()
    {
        // Arrange
        
        var getNoteQuery = GetNoteQueryUtils.GetNoteQuery();
        var exampleNote = NoteRepositoryUtils.GetExampleNote();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(value: null);
        
        // Act

        var result = await _getNoteQueryHandler
            .Handle(getNoteQuery, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.NotFound);
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(x => x
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorAccessDenied_WhenUserDoesNotHaveAccessToTheNote()
    {
        // Arrange
        
        var getNoteQuery = GetNoteQueryUtils.GetNoteQuery();
        var exampleNote = NoteRepositoryUtils.GetExampleNote();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(2);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(exampleNote);
        
        // Act

        var result = await _getNoteQueryHandler
            .Handle(getNoteQuery, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.AccessDenied);
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(x => x
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnTheNote_WhenTheDataIsCorrect()
    {
        // Arrange
        
        var getNoteQuery = GetNoteQueryUtils.GetNoteQuery();
        var exampleNote = NoteRepositoryUtils.GetExampleNote();
        
        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(exampleNote);
        
        // Act

        var result = await _getNoteQueryHandler
            .Handle(getNoteQuery, default);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(x => x
                .GetByIdAsync(It.IsAny<int>(), default), Times.Once);
    }
}