using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Studweb.Application.Utils;
using Studweb.Infrastructure.Utils;
using Studweb.Infrastructure.Utils.Extensions;

namespace Studweb.Infrastructure.UnitTests.Utils;

public class UserContextTests
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly IUserContext _userContext;

    public UserContextTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _userContext = new UserContext(_mockHttpContextAccessor.Object);
    }

    [Fact]
    public void IsAuthenticated_ShouldReturnNull_WhenHttpContextDoesNotExist()
    {
        // Arrange

        _mockHttpContextAccessor
            .SetupGet(x => x.HttpContext)
            .Returns(value: null);
        
        // Act

        var result = _userContext.IsAuthenticated;
        
        // Assert

        result.Should().BeNull();
    }

    [Fact]
    public void IsAuthenticated_ShouldReturnNull_WhenIdentityDoesNotExist()
    {
        // Arrange

        _mockHttpContextAccessor
            .SetupGet(x => x
                .HttpContext
                .User
                .Identity)
            .Returns(value: null);
        
        // Act

        var result = _userContext.IsAuthenticated;
        
        // Assert

        result.Should().BeNull();
    }

    [Fact]
    public void IsAuthenticated_ShouldReturnFalse_WhenUserIsNotAuthenticated()
    {
        // Arrange

        _mockHttpContextAccessor
            .SetupGet(x => x
                .HttpContext
                .User
                .Identity
                .IsAuthenticated)
            .Returns(false);
        
        // Act

        var result = _userContext.IsAuthenticated;
        
        // Assert

        result.Should().Be(false);
    }
    
    [Fact]
    public void IsAuthenticated_ShouldReturnTrue_WhenUserIsAuthenticated()
    {
        // Arrange

        _mockHttpContextAccessor
            .SetupGet(x => x
                .HttpContext
                .User
                .Identity
                .IsAuthenticated)
            .Returns(true);
        
        // Act

        var result = _userContext.IsAuthenticated;
        
        // Assert

        result.Should().Be(true);
    }
    
    [Fact]
    public void UserId_ShouldReturnNull_WhenHttpContextDoesNotExist()
    {
        // Arrange

        _mockHttpContextAccessor
            .SetupGet(x => x.HttpContext)
            .Returns(value: null);
        
        // Act

        var result = _userContext.UserId;
        
        // Assert

        result.Should().BeNull();
    }

    [Fact]
    public void UserId_ShouldReturnCorrectValue_WhenHttpContextIsValid()
    {
        // Arrange

        var mockHttpContext = new DefaultHttpContext();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }));
        mockHttpContext.User = claimsPrincipal;

        _mockHttpContextAccessor
            .SetupGet(x => x.HttpContext)
            .Returns(mockHttpContext);

        // Act
        var userId = _userContext.UserId;

        // Assert
        userId.Should().NotBe(null);
    }

}