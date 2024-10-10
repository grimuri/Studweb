using FluentAssertions;
using Moq;
using Studweb.Application.Features.Users.Commands;
using Studweb.Application.Persistance;
using Studweb.Application.UnitTests.Features.Users.Commands.TestUtils;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;
using Studweb.Domain.Entities;

namespace Studweb.Application.UnitTests.Features.Users.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly RegisterUserCommandHandler _registerUserCommandHandler;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;

    public RegisterUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _registerUserCommandHandler = new RegisterUserCommandHandler(
            _mockUserRepository.Object, 
            _mockUnitOfWork.Object,
            _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorDuplicateEmail_WhenEmailIsNotUnique()
    {
        // Arrange

        var registerCommand = RegisterUserCommandUtils.RegisterUserCommand();

        _mockUserRepository.Setup(repository => 
                repository.GetByEmailAsync(It.IsAny<string>(), default))
            .ReturnsAsync(1);
        
        // Act

        var result = await _registerUserCommandHandler.Handle(registerCommand, default);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.DuplicateEmail);
    }

    [Fact]
    public async Task Handle_Should_CreateUserAndReturnId_WhenUserIsValid()
    {
        // Arrange 
        
        var registerCommand = RegisterUserCommandUtils.RegisterUserCommand();


        _mockUserRepository.SetupSequence(repository =>
                repository.GetByEmailAsync(It.IsAny<string>(), default))
            .ReturnsAsync(0) // Simulate email does not exist
            .ReturnsAsync(1);
        
        // Act
        
        var result = await _registerUserCommandHandler.Handle(registerCommand, default);
        
        // Assert
        
        result.IsError.Should().BeFalse();
        result.Value.Id.Should().Be(1);
        
        _mockUserRepository.Verify(userRepository => 
            userRepository.RegisterAsync(
                It.IsAny<User>(),
                default), 
            Times.Once);
        
        _mockUnitOfWork.Verify(unitOfWork => 
            unitOfWork.BeginTransaction(), Times.Once);
        _mockUnitOfWork.Verify(unitOfWork => 
            unitOfWork.CommitAndCloseConnection(), Times.Once);
        
        _mockPasswordHasher.Verify(passwordHasher =>
            passwordHasher.Hash(It.IsAny<string>()), Times.Once);
    }
}