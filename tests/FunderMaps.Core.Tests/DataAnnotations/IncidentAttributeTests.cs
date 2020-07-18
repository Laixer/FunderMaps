using FunderMaps.Core.DataAnnotations;
using Xunit;

namespace FunderMaps.Core.Tests.DataAnnotations
{
    public class IncidentAttributeTests
    {
        [Fact]
        public void IsValidOnInput()
        {
            // Arrange
            var attr = new IncidentAttribute();

            // Act
            bool valid = attr.IsValid("FIR-12345");

            // Assert
            Assert.True(valid);
        }

        [Fact]
        public void IsInvalidOnInput()
        {
            // Arrange
            var attr = new AddressAttribute();

            // Act
            bool valid = attr.IsValid("FI-12345");
            bool valid2 = attr.IsValid("FIR12345");
            bool valid3 = attr.IsValid("fir-12345");

            // Assert
            Assert.False(valid);
            Assert.False(valid2);
            Assert.False(valid3);
        }
    }
}
