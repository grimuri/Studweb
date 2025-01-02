using FluentAssertions;
using Moq;
using Studweb.Application.Features.Notes.Queries.GetAllNotes;
using Studweb.Application.Persistance;
using Studweb.Application.UnitTests.TestUtils.Repository;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.UnitTests.Features.Notes.Queries.GetAllNotes;

public class GetAllNotesQueryHandlerTests
{
    private readonly GetAllNotesQueryHandler _getAllNotesQueryHandler;
    private readonly Mock<IUserContext> _mockUserContext;
    private readonly Mock<INoteRepository> _mockNoteRepository;

    public GetAllNotesQueryHandlerTests()
    {
        _mockUserContext = new Mock<IUserContext>();
        _mockNoteRepository = new Mock<INoteRepository>();
        _getAllNotesQueryHandler = new GetAllNotesQueryHandler(
            _mockUserContext.Object,
            _mockNoteRepository.Object
            );
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange

        var getAllNotesQuery = new GetAllNotesQuery();

        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(value: null);
        
        // Act

        var result = await _getAllNotesQueryHandler
            .Handle(getAllNotesQuery, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllNotesBelongingToUser_WhenUserIsAuthenticated()
    {
        // Arrange

        var getAllNotesQuery = new GetAllNotesQuery();
        var exampleNotes = NoteRepositoryUtils.GetExampleNotes();

        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetAllNotesAsync(It.IsAny<int>(), default))
            .ReturnsAsync(exampleNotes);
        
        // Act

        var result = await _getAllNotesQueryHandler
            .Handle(getAllNotesQuery, default);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Notes.Should().HaveCount(exampleNotes.Count());
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(x => x
                .GetAllNotesAsync(It.IsAny<int>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyCollection_WhenUserHasNoNotes()
    {
        // Arrange

        var getAllNotesQuery = new GetAllNotesQuery();

        _mockUserContext
            .SetupGet(x => x.UserId)
            .Returns(1);
        _mockNoteRepository
            .Setup(x => x
                .GetAllNotesAsync(It.IsAny<int>(), default))
            .ReturnsAsync(Enumerable.Empty<Note>());
        
        // Act

        var result = await _getAllNotesQueryHandler
            .Handle(getAllNotesQuery, default);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Notes.Should().BeEmpty();
        _mockUserContext
            .Verify(x => x.UserId, Times.Once);
        _mockNoteRepository
            .Verify(x => x
                .GetAllNotesAsync(It.IsAny<int>(), default), Times.Once);
    }
    
    
}