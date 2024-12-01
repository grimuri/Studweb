using FluentAssertions;
using Moq;
using Studweb.Application.Features.Users.Commands.RegisterUser;
using Studweb.Application.Persistance;
using Studweb.Application.UnitTests.Features.Users.Commands.RegisterUser.TestUtils;
using Studweb.Application.UnitTests.TestUtils.Repository;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.User;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.UnitTests.Features.Users.Commands.RegisterUser;

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
            .ReturnsAsync(UserRepositoryUtils.GetByEmailExampleUser());
        
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
            .ReturnsAsync(value: null) // Simulate email does not exist
            .ReturnsAsync(UserRepositoryUtils.GetByEmailExampleUser());
        
        // Act
        
        var result = await _registerUserCommandHandler.Handle(registerCommand, default);
        
        // Assert
        
        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
        
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