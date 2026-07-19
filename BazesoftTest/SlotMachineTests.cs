using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazesoftTest
{

    using IrinCibiBlazesoftChallenge.Services;
    using Xunit;

    namespace IrinCibiBlazesoftChallenge.Tests
    {
        public class SlotMachineTests
        {
            [Theory]
            [InlineData(new int[] { 3, 3, 3, 4, 5 }, 9)]  // 3+3+3 = 9
            [InlineData(new int[] { 2, 3, 2 }, 0)]         // No consecutive streak > 2 starting at 0
            [InlineData(new int[] { 7, 7, 7, 3, 7, 7, 3 }, 21)] // 7+7+7 = 21 (streak starting at 0)
            [InlineData(new int[] { 1, 1, 1, 1, 1 }, 5)]   // Full streak = 5
            public void Test_CalculatePathWin_ShouldReturnExpectedScore(int[] pathValues, int expectedMultiplier)
            {
                // Act
                int result = SlotMachineEngine.CalculatePathWin(pathValues);

                // Assert
                Assert.Equal(expectedMultiplier, result);
            }

            [Fact]
            public void Test_DiagonalPathGeneration_YieldsCorrectZigZagCoordinates()
            {
                // Arrange (5 x 3 Matrix)
                int width = 5;
                int height = 3;

                // Act
                var paths = SlotMachineEngine.GetWinLinePaths(width, height);

                // The top diagonal (starting at row 0) should be: [0,0] -> [1,1] -> [2,2] -> [3,1] -> [4,0]
                var topDiagonal = paths[3]; // First 3 are rows, 4th is the first diagonal

                // Assert
                Assert.Equal((0, 0), topDiagonal[0]);
                Assert.Equal((1, 1), topDiagonal[1]);
                Assert.Equal((2, 2), topDiagonal[2]);
                Assert.Equal((3, 1), topDiagonal[3]);
                Assert.Equal((4, 0), topDiagonal[4]);
            }
        }
    }
}
