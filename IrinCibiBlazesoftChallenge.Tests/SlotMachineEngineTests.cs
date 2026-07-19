using Xunit;
using IrinCibiBlazesoftChallenge.Services;

namespace IrinCibiBlazesoftChallenge.Tests
{
    public class SlotMachineEngineTests
    {
        [Fact]
        public void GenerateMatrix_ShouldCreateCorrectSize()
        {
            int width = 5;
            int height = 3;

            var matrix = SlotMachineEngine.GenerateMatrix(width, height);

            Assert.Equal(height, matrix.Length);

            foreach (var row in matrix)
            {
                Assert.Equal(width, row.Length);
            }
        }

        [Fact]
        public void GenerateMatrix_ShouldContainOnlyDigitsZeroToNine()
        {
            var matrix = SlotMachineEngine.GenerateMatrix(5, 3);

            foreach (var row in matrix)
            {
                foreach (var value in row)
                {
                    Assert.InRange(value, 0, 9);
                }
            }
        }

        [Fact]
        public void CalculatePathWin_ShouldReturnNine()
        {
            int[] values = { 3, 3, 3, 4, 5 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(9, result);
        }

        [Fact]
        public void CalculatePathWin_ShouldReturnTwentyOne()
        {
            int[] values = { 7, 7, 7, 3, 7, 7, 3 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(21, result);
        }

        [Fact]
        public void CalculatePathWin_ShouldReturnZero()
        {
            int[] values = { 2, 3, 2 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetWinLinePaths_ShouldReturnCorrectNumberOfPaths()
        {
            var paths = SlotMachineEngine.GetWinLinePaths(5, 3);

            Assert.Equal(6, paths.Count);
        }
    }
}