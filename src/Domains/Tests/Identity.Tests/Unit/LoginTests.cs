using Identity.Application.Commands;
using Identity.Application.Handlers;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Moq;


namespace Identity.Tests.Unit
{
    public class LoginTests
    {
        [Fact]
        public async Task Should_ReturnToken_When_CredentialsAreValid()
        {
            // Arrange
            var user = new User("test@email.com", "hashedPassword", "Customer");

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            var hasherMock = new Mock<IPasswordHasher>();
            hasherMock.Setup(h => h.Verify("123456", user.PasswordHash)).Returns(true);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns("fake-jwt-token");

            var handler = new LoginUserHandler(repoMock.Object, hasherMock.Object, tokenServiceMock.Object);

            var command = new LoginUserCommand { Email = user.Email, Password = "123456" };

            // Act
            var token = await handler.Handle(command);

            // Assert
            Assert.Equal("fake-jwt-token", token);


        }
    }
}
