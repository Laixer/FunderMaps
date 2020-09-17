using FunderMaps.Core.DataAnnotations;
using Xunit;

namespace FunderMaps.Core.Tests.DataAnnotations
{
    public class IncidentAttributeTests : IClassFixture<IncidentAttribute>
    {
        private readonly IncidentAttribute _incidentAttribute;

        public IncidentAttributeTests(IncidentAttribute incidentAttribute)
        {
            _incidentAttribute = incidentAttribute;
        }

        [Fact]
        public void IsValidOnInput()
        {
            // Assert
            Assert.True(_incidentAttribute.IsValid(null));
            Assert.True(_incidentAttribute.IsValid("FIR-12345"));
        }

        [Fact]
        public void IsInvalidOnInput()
        {
            // Assert
            Assert.False(_incidentAttribute.IsValid(""));
            Assert.False(_incidentAttribute.IsValid("FI-12345"));
            Assert.False(_incidentAttribute.IsValid("fir12345"));
            Assert.False(_incidentAttribute.IsValid("fir-12345"));
        }
    }
}
