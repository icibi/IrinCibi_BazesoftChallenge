using System;
using System.Collections.Generic;
using System.Linq;

namespace IrinCibiBlazesoftChallenge.Services
{
    public static class SlotMachineEngine
    {
        // Generates a random slot machine matrix using digits 0-9.
        public static int[][] GenerateMatrix(int width, int height)
        {
            int[][] matrix = new int[height][];

            for (int row = 0; row < height; row++)
            {
                matrix[row] = new int[width];
                for (int col = 0; col < width; col++)
                {
                    matrix[row][col] = Random.Shared.Next(0, 10); 
                }
            }

            return matrix;
        }

        // Generates all horizontal and zig-zag win line paths based on the configured slot machine dimensions.
        public static List<List<(int row, int col)>> GetWinLinePaths(int width, int height)
        {
            var paths = new List<List<(int row, int col)>>();

            // Horizontal rows
            for (int row = 0; row < height; row++)
            {
                var path = new List<(int row, int col)>();

                for (int col = 0; col < width; col++)
                {
                    path.Add((row, col));
                }

                paths.Add(path);
            }

            // Zig-zag diagonals
            for (int startRow = 0; startRow < height; startRow++)
            {
                var path = new List<(int row, int col)>();
                int row = startRow;
                bool goingDown = true;

                for (int col = 0; col < width; col++)
                {
                    path.Add((row, col));

                    if (goingDown)
                    {
                        if (row == height - 1)
                        {
                            goingDown = false;
                            row--;
                        }
                        else
                        {
                            row++;
                        }
                    }
                    else
                    {
                        if (row == 0)
                        {
                            goingDown = true;
                            row++;
                        }
                        else
                        {
                            row--;
                        }
                    }
                }

                paths.Add(path);
            }

            return paths;
        }

        // Calculates the win multiplier for a single win line based on consecutive matching symbols from the first column.
        public static int CalculatePathWin(int[] values)
        {
            if (values.Length < 3)
                return 0;

            int symbol = values[0];
            int count = 1;

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] == symbol)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            if (count < 3)
                return 0;

            return symbol * count;
        }
    }
}