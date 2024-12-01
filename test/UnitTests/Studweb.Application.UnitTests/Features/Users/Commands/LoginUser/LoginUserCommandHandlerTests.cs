using FluentAssertions;
using Moq;
using Studweb.Application.Features.Users.Commands.LoginUser;
using Studweb.Application.Persistance;
using Studweb.Application.UnitTests.Features.Users.Commands.LoginUser.TestUtils;
using Studweb.Application.UnitTests.TestUtils.Repository;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.User;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.UnitTests.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandlerTests
{
    private readonly LoginUserCommandHandler _loginUserCommandHandler;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtProvider> _mockJwtProvider;

    public LoginUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtProvider = new Mock<IJwtProvider>();
        _loginUserCommandHandler = new LoginUserCommandHandler(
            _mockUserRepository.Object,
            _mockPasswordHasher.Object,
            _mockJwtProvider.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorIncorrectData_WhenUserWithThatEmailIsNotFound()
    {
        // Arrange

        var loginUserCommand = LoginUserCommandUtils.LoginUserCommand();

        _mockUserRepository.Setup(r =>
                r.GetByEmailAsync(It.IsAny<string>(), default))
            .ReturnsAsync(value: null);
        
        // Act 

        var result = await _loginUserCommandHandler.Handle(loginUserCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.IncorrectData);
        _mockUserRepository.Verify(x => 
            x.GetByEmailAsync(It.IsAny<string>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotVerified_WhenUserIsNotVerified()
    {
        // Arrange

        var loginUserCommand = LoginUserCommandUtils.LoginUserCommand();
        var exampleUser = UserRepositoryUtils.GetByEmailExampleUser();
        UserRepositoryUtils.SetFieldVerifiedOnUtcOnNull(exampleUser);

        _mockUserRepository.Setup(r =>
                r.GetByEmailAsync(It.IsAny<string>(), default))
            .ReturnsAsync(exampleUser);
        
        // Act

        var result = await _loginUserCommandHandler.Handle(loginUserCommand, default);
        
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotVerified);
        _mockUserRepository.Verify(x => 
            x.GetByEmailAsync(It.IsAny<string>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorIncorrectData_WhenUserProvidedWrongPassword()
    {
        // Arrange

        var loginUserCommand = LoginUserCommandUtils.LoginUserCommand();
        var exampleUser = UserRepositoryUtils.GetByEmailExampleUser();

        _mockUserRepository.Setup(r =>
                r.GetByEmailAsync(It.IsAny<string>(), default))
            .ReturnsAsync(exampleUser);
        _mockPasswordHasher.Setup(x =>
                x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        // Act

        var result = await _loginUserCommandHandler.Handle(loginUserCommand, default);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.IncorrectData);
        _mockUserRepository.Verify(x => 
            x.GetByEmailAsync(It.IsAny<string>(), default), Times.Once);
        _mockPasswordHasher.Verify(x =>
            x.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnLoginResponse_WhenUserIsValid()
    {
        // Arrange

        var loginUserCommand = LoginUserCommandUtils.LoginUserCommand();
        var exampleUser = UserRepositoryUtils.GetByEmailExampleUser();
        UserRepositoryUtils.SetFieldVerifiedOnUtcOnNotNull(exampleUser);

        _mockUserRepository.Setup(r =>
                r.GetByEmailAsync(It.IsAny<string>(), default))
            .ReturnsAsync(exampleUser);
        _mockPasswordHasher.Setup(x =>
                x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        // Act

        var result = await _loginUserCommandHandler.Handle(loginUserCommand, default);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.FirstName.Should().NotBe(null);
        _mockUserRepository.Verify(x => 
            x.GetByEmailAsync(It.IsAny<string>(), default), Times.Once);
        _mockPasswordHasher.Verify(x =>
            x.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockJwtProvider.Verify(x => 
            x.Generate(It.IsAny<User>()), Times.Once);
    }
}