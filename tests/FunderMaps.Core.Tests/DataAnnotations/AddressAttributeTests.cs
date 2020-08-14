using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace FunderMaps.Core.Tests.DataAnnotations
{
    public class AddressAttributeTests : IClassFixture<AddressAttribute>
    {
        private readonly AddressAttribute _addressAttribute;

        public AddressAttributeTests(AddressAttribute addressAttribute)
        {
            _addressAttribute = addressAttribute;
        }

        [Fact]
        public void IsValidOnInput()
        {
            // Assert
            Assert.True(_addressAttribute.IsValid(null));
            Assert.True(_addressAttribute.IsValid("gfm-12345"));
        }

        [Fact]
        public void IsInvalidOnInput()
        {
            // Assert
            Assert.False(_addressAttribute.IsValid(""));
            Assert.False(_addressAttribute.IsValid("ggfm-12345"));
            Assert.False(_addressAttribute.IsValid("gfm12345"));
            Assert.False(_addressAttribute.IsValid("GFM-12345"));
        }
    }
}
