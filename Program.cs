using System;
using System.Threading;
using Test;

namespace Life
{
    /// <summary>
    /// Contains methods used to run Conway's Game of Life
    /// </summary>
    /// <author>
    /// SUMIN KIM : n10710841
    /// </author>
    public static class Conway
    {
        /// <summary>
        /// Runs the game of life according to its rules and continuously refreshes the result
        /// </summary>
        public static void Main()
        {

            // Comment out the line below to disable tests
            // TestRunner.RunTests();

            // List of constants
            const int Update_Time = 150;
            const int Number_Of_Rows = 20;
            const int Number_Of_Columns = 50;

            // Write down the welcome message
            Console.WriteLine("Welcome Conway's Game of Life!");
            // Write down the enter message
            Console.Write("Press ENTER to start the simulation.");
            // Determine the number of rows and columns 
            bool[,] grid = MakeGrid(Number_Of_Rows, Number_Of_Columns);
            Console.ReadLine();

            // Iterate code
            while (true)
            {
                // Clear up all the grid
                Console.Clear();
                DrawGrid(grid);
                grid = UpdateGrid(grid);
                // Wait a specified time
                System.Threading.Thread.Sleep(Update_Time);

            }

        }

        /// <summary>
        /// Returns a new grid for Conway's Game of life using the given dimensions.
        /// Each cell has a 50% chance of initially being alive.
        /// </summary>
        /// <param name="rows">The desired number of rows</param>
        /// <param name="cols">The desired number of columns</param>
        /// <returns></returns>
        public static bool[,] MakeGrid(int rows, int cols)

        {
            // Create an array of boolean values
            bool[,] grid = new bool[rows, cols];
            // Make the grid
            // Make the row of the grid
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                // Make the column of the grid
                for (int col = 0; col < grid.GetLength(1); col++)
                {

                    // Using random number generation
                    int chance = Utility.RandInt(0, 1);
                    // Making the conditional expression to 50% of the time
                    if (chance == 1)
                    {
                        grid[row, col] = true;
                    }
                    else
                    {
                        grid[row, col] = false;
                    }
                }

            }
            return grid;
        }

        /// <summary>
        /// Writes the given game grid to standard output
        /// </summary>
        /// <param name="grid">The grid to draw to standard output</param>
        public static void DrawGrid(bool[,] grid)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {

                    // If the condition is true, write down #
                    if (grid[row, col] == true)
                    {
                        Console.Write("#");
                    }
                    // If the condition is false, write down .
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Returns the number of living neighbours adjacent to a given cell position
        /// </summary>
        /// <param name="grid">The game grid</param>
        /// <param name="row">The cell's row</param>
        /// <param name="col">The cell's column</param>
        /// <returns>The number of adjacent living neighbours</returns>
        public static int CountNeighbours(bool[,] grid, int row, int col)
        {

            {
                // Default the number of sharp to 0
                int numberOfSharp = 0;
                if (grid[row, col] == true)
                {
                    // Subtract the own count
                    numberOfSharp -= 1;
                }

                for (int rows = row - 1; rows <= row + 1; rows++)
                {
                    // If no rows exist, it should calculate as zero
                    if (rows < 0)
                    {
                        rows = 0;
                    }
                    // If the row is exceeded, the for loop must be broken
                    else if (rows >= grid.GetLength(0))
                    {
                        break;
                    }
                    for (int cols = col - 1; cols <= col + 1; cols++)
                    {
                        // If no columns exist, it should be calculated as zero
                        if (cols < 0)
                        {
                            cols = 0;
                        }
                        // If the columns exceeds, the for loop must break
                        else if (cols >= grid.GetLength(1))
                        {
                            break;
                        }
                        // If grid boolean is true, it should be added by 1
                        if (grid[rows, cols] == true)
                        {
                            numberOfSharp += 1;
                        }

                    }
                }
                return numberOfSharp;
            }

        }

        /// <summary>
        /// Returns an updated grid after progressing the rules of the Game of Life.
        /// </summary>
        /// <param name="grid">The original grid from which the new grid is derived</param>
        /// <returns>A new grid which has been updated by one time-step</returns>
        public static bool[,] UpdateGrid(bool[,] grid)
        {
            // Make the new grid base on rule of the Game of Life 
            bool[,] newgrid = new bool[grid.GetLength(0), grid.GetLength(1)];
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {

                    // Rule1: Any live cell with fewer than two live neighbors dies, as if by underpopulation.
                    if (grid[row, col] == true && CountNeighbours(grid, row, col) < 2)
                    {
                        newgrid[row, col] = false;
                    }
                    // Rule2: Any live cell with two or three live neighbors lives on to the next generation.
                    else if (grid[row, col] == true && CountNeighbours(grid, row, col) == 2 || CountNeighbours(grid, row, col) == 3)
                    {
                        newgrid[row, col] = true;
                    }
                    // Rule3: Any live cell with more than three live neighbors dies, as if by overpopulation.
                    else if (grid[row, col] == true && CountNeighbours(grid, row, col) > 3)
                    {
                        newgrid[row, col] = false;
                    }
                    // Rule4: Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
                    else if (grid[row, col] == false && CountNeighbours(grid, row, col) == 3)
                    {
                        newgrid[row, col] = true;
                    }

                }

            }
            return newgrid;
        }
    }

    /// <summary>
    /// Utility class for random number generation
    /// </summary>
    static class Utility
    {
        private static Random _rng = new Random();

        /// <summary>
        /// Generate a random integer in a range
        /// </summary>
        /// <param name="min">Minimum possible random number</param>
        /// <param name="max">Maximum possible random number</param>
        /// <returns>A random integer between min and max</returns>
        public static int RandInt(int min, int max)
        {
            if (min > max) throw new ArgumentException("Minimum cannot be greater than maximum");
            return _rng.Next(min, max + 1);
        }

        /// <summary>
        /// Generate a random double in a range
        /// </summary>
        /// <param name="min">Minimum possible random number</param>
        /// <param name="max">Maximum possible random number</param>
        /// <returns>A random double between min and max</returns>
        public static double RandDouble(double min = 0, double max = 1)
        {
            if (min > max) throw new ArgumentException("Minimum cannot be greater than maximum");
            return _rng.NextDouble() * Math.Abs(max - min) + min;
        }
    }
}
