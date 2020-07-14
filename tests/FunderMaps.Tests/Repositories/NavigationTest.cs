namespace FunderMaps.Tests.Repositories
{
#if DISABLED
    public class NavigationTest
    {
        [Fact]
        public void NavigationCreationPassingTest()
        {
            // Arrange
            int testOffset = 0;
            int testLimit = 25;
            int expectedValueOffset = 0;
            int expectedValueLimit = 25;

            // Act
            var nav = new Navigation(testOffset, testLimit);

            // Assert
            Assert.Equal(expectedValueLimit, nav.Limit);
            Assert.Equal(expectedValueOffset, nav.Offset);
        }

        [Fact]
        public void NavigationIsOfTypeNavigation()
        {
            // Arrange
            int testOffset = 0;
            int testLimit = 25;

            // Act
            var nav = new Navigation(testOffset, testLimit);

            // Assert
            Assert.IsAssignableFrom<NavigationImpl<int, int>>(nav);
        }

        [Theory]
        [InlineData(0, 25)]
        [InlineData(4, 5)]
        [InlineData(5, 9)]
        public void NavigationParamsEqualOrGreaterThenZero(uint offset, uint limit)
        {
            // Act
            var nav = new NavigationImpl<uint, uint>(offset, limit);

            // Assert
            Assert.True(nav.Limit >= 0);
            Assert.True(nav.Offset >= 0);
        }
    }
#endif
}
