using FunderMaps.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FunderMaps.Tests.Repositories
{
    public class NavigationTest
    {
        /*  Xunit tests
         *  #1 Make class public ( Test classes must be public)
         *      Arrange Phase
         *  #2 Create a Method and apply the Attribute [Fact]
         *  #3 Create test data
         *  #4 Create expected data
         *      Act Phase
         *  #5 Call the function with the test data
         *      Assert Phase
         *  #6 check the Expected values with the returned values from the Act phase
         */

        [Fact]
        public void NavigationCreationPassingTest()
        {
            // Arrange
            uint testOffset = 0;
            uint testLimit = 25;
            uint expectedValueOffset = 0;
            uint expectedValueLimit = 25;

            // Act
            // create new navigation
            var nav = new Navigation(testOffset, testLimit);

            // Assert

            // Check if the nav values matched the expected values
            Assert.Equal(expectedValueLimit, nav.Limit);
            Assert.Equal(expectedValueOffset, testOffset);
        }

        [Fact]
        public void NavigationIsOfTypeNavigation()
        {
            // Arrange
            uint testOffset = 0;
            uint testLimit = 25;

            // Act
            // create new navigation
            var nav = new Navigation(testOffset, testLimit);

            // Assert
            // Check if it is the same type
            Assert.IsType<Navigation>(nav);
        }

        [Theory]
        [InlineData(0,25)]
        [InlineData(4, 5)]
        [InlineData(5, 9)]
        public void NavigationParamsEqualOrGreaterThenZero(uint offset, uint limit)
        {
            // Arrange
            // Nothing to arrange

            // Act
            // create new navigation
            var nav = new Navigation(offset, limit);

            // Assert
            // Check if the nav values are greater then 0
            Assert.True(IsEqualOrGreaterThenZero(nav.Limit));
            Assert.True(IsEqualOrGreaterThenZero(nav.Offset));
        }

        static bool IsEqualOrGreaterThenZero(uint value) => value >= 0 ? true : false;
    }
}
