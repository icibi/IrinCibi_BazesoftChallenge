/* Author: Irin Cibi
 * Date: 19/07/2016
 */

using System.Linq;
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
        public void CalculatePathWin_ShouldReturnNine_ForThreeMatchingThrees()
        {
            int[] values = { 3, 3, 3, 4, 5 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(9, result);
        }

        [Fact]
        public void CalculatePathWin_ShouldReturnTwentyOne_ForExampleFromChallenge()
        {
            int[] values = { 7, 7, 7, 3, 7, 7, 3 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(21, result);
        }

        [Fact]
        public void CalculatePathWin_ShouldReturnZero_WhenLessThanThreeMatches()
        {
            int[] values = { 2, 3, 2 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculatePathWin_ShouldReturnZero_WhenArrayHasLessThanThreeElements()
        {
            int[] values = { 5, 5 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculatePathWin_ShouldStopCounting_WhenSequenceBreaks()
        {
            int[] values = { 4, 4, 4, 5, 4, 4 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(12, result);
        }

        [Fact]
        public void CalculatePathWin_ShouldReturnZero_WhenFirstSymbolsDoNotMatch()
        {
            int[] values = { 5, 4, 5, 5, 5 };

            int result = SlotMachineEngine.CalculatePathWin(values);

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetWinLinePaths_ShouldReturnCorrectNumberOfPaths()
        {
            int width = 5;
            int height = 3;

            var paths = SlotMachineEngine.GetWinLinePaths(width, height);

            Assert.Equal(height * 2, paths.Count);
        }

        [Fact]
        public void EveryWinLine_ShouldContainOneCellPerColumn()
        {
            int width = 5;
            int height = 3;

            var paths = SlotMachineEngine.GetWinLinePaths(width, height);

            foreach (var path in paths)
            {
                Assert.Equal(width, path.Count);

                for (int i = 0; i < width; i++)
                {
                    Assert.Equal(i, path[i].col);
                }
            }
        }
    }
}