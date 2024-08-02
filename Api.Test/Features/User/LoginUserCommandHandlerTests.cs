using API.Auth;
using API.Common.Exceptions;
using API.Common.Utilities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using static API.Features.User.LoginUser.LoginUser;

namespace Api.Test.Features.User.LoginUser
{
    [TestFixture]
    public class LoginUserCommandHandlerTests
    {
        private ITokenService _tokenService;
        private DataContext _context;
        private LoginUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _tokenService = Substitute.For<ITokenService>();
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataContext(options);

            _handler = new LoginUserCommandHandler(_tokenService, _context);
        }

        [TearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Handle_ShouldReturnAccessToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new Infrastructure.Data.Entities.User
            {
                FirstName = "john",
                LastName = "doe",
                LivingCountry = "USA",
                UserName = "testuser",
                Password = PasswordHasher.HashPassword("password")
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var command = new LoginUserCommand("testuser", "password");

            _tokenService.GenerateBearerToken(user).ReturnsForAnyArgs("fake-jwt-token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.AccessToken, Is.EqualTo("fake-jwt-token"));
        }

        [Test]
        public void Handle_ShouldThrowBusinessException_WhenUsernameIsInvalid()
        {
            // Arrange
            var command = new LoginUserCommand("invaliduser", "password");

            // Act & Assert
            Assert.ThrowsAsync<BusinessException>(async () => await _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_ShouldThrowBusinessException_WhenPasswordIsInvalid()
        {
            // Arrange
            var user = new Infrastructure.Data.Entities.User
            {
                FirstName = "john",
                LastName = "doe",
                LivingCountry = "USA",
                UserName = "testuser",
                Password = PasswordHasher.HashPassword("password")
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var command = new LoginUserCommand("testuser", "wrongpassword");

            // Act & Assert
            Assert.ThrowsAsync<BusinessException>(async () => await _handler.Handle(command, CancellationToken.None));
        }
    }
}
