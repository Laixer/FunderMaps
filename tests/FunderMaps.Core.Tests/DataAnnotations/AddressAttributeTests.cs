using FunderMaps.Core.DataAnnotations;
using Xunit;

namespace FunderMaps.Core.Tests.DataAnnotations
{
    public class AddressAttributeTests
    {
        [Fact]
        public void IsValidOnInput()
        {
            // Arrange
            var attr = new AddressAttribute();

            // Act
            bool valid = attr.IsValid("gfm-12345");

            // Assert
            Assert.True(valid);
        }

        [Fact]
        public void IsInvalidOnInput()
        {
            // Arrange
            var attr = new AddressAttribute();

            // Act
            bool valid = attr.IsValid("ggfm-12345");
            bool valid2 = attr.IsValid("gfm12345");
            bool valid3 = attr.IsValid("GFM-12345");

            // Assert
            Assert.False(valid);
            Assert.False(valid2);
            Assert.False(valid3);
        }
    }
}
