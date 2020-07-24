using FunderMaps.Core.Services;
using Xunit;

namespace FunderMaps.Core.Tests.Services
{
    public class PasswordHasherTests
    {
        [Theory]
        [InlineData("ABC")]
        [InlineData("dknrkvb417lqr77jd0vm4yvz9lyul00f")]
        [InlineData("4s73ixbv4fw4285i7ypekvtat2anbxj5")]
        [InlineData("6c864e7abe78b7b17cb481ece8710c98399182ccb23f08a343f408d2aae721c1")]
        public void GetAsyncReturnsAddress(string password)
        {
            // Arrange
            var passwordHasher = new PasswordHasher();

            // Act
            var hash = passwordHasher.HashPassword(password);
            var result = passwordHasher.IsPasswordValid(hash, password);

            // Assert
            Assert.NotNull(hash);
            Assert.True(result);
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("dknrkvb417lqr77jd0vm4yvz9lyul00f")]
        [InlineData("4s73ixbv4fw4285i7ypekvtat2anbxj5")]
        [InlineData("6c864e7abe78b7b17cb481ece8710c98399182ccb23f08a343f408d2aae721c1")]
        public void HashPasswordAndValidateSuccess(string password)
        {
            // Arrange
            var passwordHasher = new PasswordHasher();

            // Act
            var hash = passwordHasher.HashPassword(password);
            var result = passwordHasher.IsPasswordValid(hash, password);

            // Assert
            Assert.NotNull(hash);
            Assert.True(result);
        }

        [Theory]
        [InlineData("FOO")]
        [InlineData("kSSZjvdoXfA9JlqWBMeX")]
        public void HashPasswordAndValidateInvalid(string password)
        {
            // Arrange
            var passwordHasher = new PasswordHasher();

            // Act
            var hash = passwordHasher.HashPassword(password + "x");
            var result = passwordHasher.IsPasswordValid(hash, password);

            // Assert
            Assert.NotNull(hash);
            Assert.False(result);
        }
    }
}
