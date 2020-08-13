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

            // Assert
            Assert.False(attr.IsValid("FI-12345"));
            Assert.False(attr.IsValid("FIR12345"));
            Assert.False(attr.IsValid("fir-12345"));
        }
    }
}
