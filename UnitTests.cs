using System;
using System.Collections.Generic;
using System.Linq;

using Life;
using System.IO;

namespace TestFramework
{
    public class AssertionException : ApplicationException
    {
        public AssertionException(string message = "") : base(message) { }
    }
    public static class CollectionAssert
    {
        public static void AreEqual<T>(T[] expected, T[] actual, string message = "")
        {
            if (expected.Length != actual.Length) { throw new AssertionException(message); }
            for (int i = 0; i < expected.Length; i++)
            {
                if (!expected[i].Equals(actual[i])) { throw new AssertionException(message); }
            }
        }

        public static void AreEqual<T>(T[,] expected, T[,] actual, string message = "")
        {
            if (expected.GetLength(0) != actual.GetLength(0)) { throw new AssertionException(message); }
            if (expected.GetLength(1) != actual.GetLength(1)) { throw new AssertionException(message); }
            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    if (!expected[i, j].Equals(actual[i, j])) { throw new AssertionException(message); }
                }
            }
        }

        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message = "")
        {
            if (!expected.SequenceEqual(actual))
            {
                throw new AssertionException(message);
            }
        }
    }
    public static class Assert
    {
        public static void AreEqual(object expected, object actual, string message = "")
        {
            if (!expected.Equals(actual))
            {
                throw new AssertionException(message);
            }
        }

        public static void IsTrue(bool expression, string message = "")
        {
            if (!expression) { throw new AssertionException(message); }
        }

        public static void IsFalse(bool expression, string message = "")
        {
            if (expression) { throw new AssertionException(message); }
        }

        public static void Fail(string message = "")
        {
            throw new AssertionException(message);
        }
    }
}
namespace Utility
{
    static class Utility
    {
        public static int NumAlive(bool[,] grid)
        {
            int numAlive = 0;
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col])
                    {
                        numAlive++;
                    }
                }
            }
            return numAlive;
        }
    }
}
namespace Test
{
    public static class TestRunner
    {
        public static void RunTests()
        {
            RunTestCategory("A: MakeGrid", new Action[] {
                UnitTests.A_Test_MakeGrid.MakeGrid_RunsWithoutErrors,
                UnitTests.A_Test_MakeGrid.MakeGrid_DimensionsAreCorrect,
                UnitTests.A_Test_MakeGrid.MakeGrid_GridIsAlwaysDifferent,
                UnitTests.A_Test_MakeGrid.MakeGrid_GridIsAboutHalfLiving
            });

            RunTestCategory("B: DrawGrid", new Action[] {
                UnitTests.B_Test_DrawGrid.DrawGrid_RunsWithoutErrors,
                UnitTests.B_Test_DrawGrid.DrawGrid_Test1,
                UnitTests.B_Test_DrawGrid.DrawGrid_Test2,
            });

            RunTestCategory("C: CountNeighbours", new Action[] {
                UnitTests.C_Test_CountNeighbours.CountNeighbours_InternalSpace_RunsWithoutErrors,
                UnitTests.C_Test_CountNeighbours.CountNeighbours_InternalSpace_ZeroNeighbours_CellIsDead,
                UnitTests.C_Test_CountNeighbours.CountNeighbours_InternalSpace_ZeroNeighbours_CellIsAlive,
                UnitTests.C_Test_CountNeighbours.CountNeighbours_InternalSpace_AdjacentNeighbours,
                UnitTests.C_Test_CountNeighbours.CountNeighbours_InternalSpace_DiagonalNeighbours,
                UnitTests.C_Test_CountNeighbours.CountNeighbours_InternalSpace_AllNeighbours,
                UnitTests.C_Test_CountNeighbours.CountNeighbours_BoundaryAndCornerSpaces_RunsWithoutErrors,
                UnitTests.C_Test_CountNeighbours.CountNeighbours_CornerSpaces_Tests,
                UnitTests.C_Test_CountNeighbours.CountNeighbours_EdgeSpaces_Tests
            });

            RunTestCategory("D: UpdateGrid", new Action[] {
                UnitTests.D_Test_UpdateGrid.UpdateGrid_RunsWithoutErrors,
                UnitTests.D_Test_UpdateGrid.UpdateGrid_Example1_Blinker1,
                UnitTests.D_Test_UpdateGrid.UpdateGrid_Example2_Blinker2,
                UnitTests.D_Test_UpdateGrid.UpdateGrid_Example3_Blinker3,
                UnitTests.D_Test_UpdateGrid.UpdateGrid_Example4_StillLife1,
                UnitTests.D_Test_UpdateGrid.UpdateGrid_Example5_StillLife2,
                UnitTests.D_Test_UpdateGrid.UpdateGrid_Example6_StillLife3,
                UnitTests.D_Test_UpdateGrid.UpdateGrid_Example7_Glider
            });

            Console.Write("Press ENTER to continue.");
            Console.ReadLine();
        }
        private static void RunTestCategory(string name, Action[] tests)
        {
            Console.WriteLine($"=== Test category: {name} ===");
            int passCount = 0;

            for (int i = 0; i < tests.Length; i++)
            {
                Console.WriteLine($"\tTest {i + 1}/{tests.Length}: {tests[i].Method.Name}...");
                try
                {
                    tests[i]();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\t\tTest failed: {e.Message}");
                    continue;
                }

                Console.WriteLine("\t\tTest passed");
                passCount++;
            }

            Console.WriteLine($"\t==> {passCount}/{tests.Length} <==\n");
        }
    }
}
namespace UnitTests
{
    static class A_Test_MakeGrid
    {
        public static void MakeGrid_RunsWithoutErrors()
        {
            bool[,] grid = Conway.MakeGrid(10, 10);
            grid = Conway.MakeGrid(10, 20);
            grid = Conway.MakeGrid(20, 20);
        }
        public static void MakeGrid_DimensionsAreCorrect()
        {
            int[] rowAmounts = { 5, 10, 15, 30, 50, 100 };
            int[] colAmounts = { 5, 10, 15, 30, 50, 100 };
            bool[,] grid;

            foreach (int rowAmount in rowAmounts)
            {
                foreach (int colAmount in colAmounts)
                {
                    grid = Conway.MakeGrid(rowAmount, colAmount);
                    TestFramework.Assert.AreEqual(
                        expected: rowAmount,
                        actual: grid.GetLength(0),
                        message: string.Format(
                            format: "The generated grid has {0} rows instead of {1}!",
                            arg0: grid.GetLength(0),
                            arg1: rowAmount
                        )
                    );

                    TestFramework.Assert.AreEqual(
                        expected: colAmount,
                        actual: grid.GetLength(1),
                        message: string.Format(
                            format: "The generated grid has {0} cols instead of {1}!",
                            arg0: grid.GetLength(1),
                            arg1: colAmount
                        )
                    );
                }
            }
        }
        public static void MakeGrid_GridIsAboutHalfLiving()
        {
            const double ERROR_MARGIN = 0.1;
            const double FILL_PERCENT = 0.5;
            int rows = 100;
            int cols = 100;
            int numCells = rows * cols;

            bool[,] grid = Conway.MakeGrid(rows, cols);

            int numAlive = Utility.Utility.NumAlive(grid);

            TestFramework.Assert.IsTrue(Math.Abs(numCells * FILL_PERCENT - numAlive) < numCells * ERROR_MARGIN, "Your MakeGrid method generally does not produce 50% alive.");
        }
        public static void MakeGrid_GridIsAlwaysDifferent()
        {
            int rows = 100;
            int cols = 100;

            bool[,] grid1 = Conway.MakeGrid(rows, cols);
            bool[,] grid2 = Conway.MakeGrid(rows, cols);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (grid1[row, col] != grid2[row, col])
                    {
                        return;
                    }
                }
            }

            TestFramework.Assert.Fail(
                "Two different 100x100 grids were generated, " +
                "but they were both exactly the same! Is your algorithm truly random?"
            );
        }
    }
    static class B_Test_DrawGrid
    {
        public static void DrawGrid_RunsWithoutErrors()
        {
            // test grid
            bool[,] grid = new bool[,]
            {
                { true, true, false, false },
                { false, false, true, false },
                { false, true, false, true },
                { true, false, false, false }
            };

            TextWriter oldOut = Console.Out;
            StringWriter newOut = new StringWriter();
            Console.SetOut(newOut);

            try
            {
                Conway.DrawGrid(grid);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Console.SetOut(oldOut);
            }
        }
        public static void DrawGrid_Test1()
        {
            // test grid
            bool[,] grid = new bool[,]
            {
                { true, true, false, false },
                { false, false, true, false },
                { false, true, false, true },
                { true, false, false, false }
            };

            // get the standard output stream
            //StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());
            TextWriter oldOut = Console.Out;

            // create a new output stream and redirect the console to it
            StringWriter newOut = new StringWriter();
            Console.SetOut(newOut);

            // draw the grid
            try
            {
                Conway.DrawGrid(grid);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // return to normal standard output
                Console.SetOut(oldOut);
            }


            // NOTE: the \r in the \r\n is automatically added during console output
            //       you do NOT need to add in your DrawGrid method
            TestFramework.Assert.AreEqual(
                expected: "##..\r\n..#.\r\n.#.#\r\n#...\r\n",
                actual: newOut.ToString()
            );
        }
        public static void DrawGrid_Test2()
        {
            // test grid
            bool[,] grid = new bool[,]
            {
                { false, false, true,  false },
                { false, true,  false, false },
                { true,  false, true,  true },
                { false, false, false, true }
            };

            // get the current output stream
            TextWriter oldOut = Console.Out;

            // create a new output stream and redirect the console to it
            StringWriter newOut = new StringWriter();
            Console.SetOut(newOut);

            // draw the grid
            try
            {
                Conway.DrawGrid(grid);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // return to normal standard output
                Console.SetOut(oldOut);
            }

            // NOTE: the \r in the \r\n is automatically added during console output
            //       you do NOT need to add in your DrawGrid method
            TestFramework.Assert.AreEqual(
                expected: "..#.\r\n.#..\r\n#.##\r\n...#\r\n",
                actual: newOut.ToString()
            );
        }
    }
    static class C_Test_CountNeighbours
    {
        public static void CountNeighbours_InternalSpace_RunsWithoutErrors()
        {
            bool[,] grid = {
                { false, false, true, false },
                { true, true, false, false },
                { true, false, false, true },
                { false, false, true, true },
            };

            Conway.CountNeighbours(grid, 2, 1);
            Conway.CountNeighbours(grid, 1, 1);
        }
        public static void CountNeighbours_InternalSpace_ZeroNeighbours_CellIsDead()
        {
            bool[,] grid = {
                { true, true,  true,  true,  true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, false, false, false, true },
                { true, true,  true,  true,  true },
            };

            TestFramework.Assert.AreEqual(
                expected: 0,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }
        public static void CountNeighbours_InternalSpace_ZeroNeighbours_CellIsAlive()
        {
            bool[,] grid = {
                { true, true,  true,  true,  true },
                { true, false, false, false, true },
                { true, false, true,  false, true },
                { true, false, false, false, true },
                { true, true,  true,  true,  true },
            };

            TestFramework.Assert.AreEqual(
                expected: 0,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }
        public static void CountNeighbours_InternalSpace_AdjacentNeighbours()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 1,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, true,  false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 2,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, true,  false, true,  false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, true,  false, true,  false },
                { false, false, true,  false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 4,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }
        public static void CountNeighbours_InternalSpace_DiagonalNeighbours()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 1,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, true,  false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 2,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, true,  false },
                { false, false, false, false, false },
                { false, true,  false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, true, false },
                { false, false, false, false, false },
                { false, true,  false, true, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 4,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }
        public static void CountNeighbours_InternalSpace_AllNeighbours()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  true,  true,  false },
                { false, true,  true,  true,  false },
                { false, true,  true,  true,  false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 8,
                actual: Conway.CountNeighbours(grid, 2, 2),
                message: "Incorrect number of neighbours calculated!"
            );
        }
        public static void CountNeighbours_BoundaryAndCornerSpaces_RunsWithoutErrors()
        {
            bool[,] grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                // first column
                Conway.CountNeighbours(grid, row, 0);

                // middle columns
                if (row != 0 && row != grid.GetLength(0) - 1)
                {
                    for (int col = 1; col < grid.GetLength(1) - 1; col++)
                    {
                        Conway.CountNeighbours(grid, row, col);
                    }
                }

                // last column
                Conway.CountNeighbours(grid, row, grid.GetLength(1) - 1);
            }
        }
        public static void CountNeighbours_CornerSpaces_Tests()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 0,
                actual: Conway.CountNeighbours(grid, 0, 0)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, true  },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 1,
                actual: Conway.CountNeighbours(grid, 0, 4)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false,  true },
                { false, false, false, true,  false }
            };

            TestFramework.Assert.AreEqual(
                expected: 2,
                actual: Conway.CountNeighbours(grid, 4, 4)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { true,  true,  false, false, false },
                { false, true,  false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 4, 0)
            );

            grid = new bool[,] {
                { true, true, false, false, false },
                { true, true, false, false, false },
                { false, false, false, false, false },
                { false,  false,  false, false, false },
                { false,  false,  false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 0, 0)
            );
        }
        public static void CountNeighbours_EdgeSpaces_Tests()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 0,
                actual: Conway.CountNeighbours(grid, 0, 1)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, true  },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 1,
                actual: Conway.CountNeighbours(grid, 2, 4)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, true,  true, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 2,
                actual: Conway.CountNeighbours(grid, 4, 2)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, true,  false, false, false },
                { true,  true,  false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 3,
                actual: Conway.CountNeighbours(grid, 2, 0)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, true,  false, false, false },
                { false, true,  false, false, false },
                { true,  true,  false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 4,
                actual: Conway.CountNeighbours(grid, 2, 0)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { true,  true,  false, false, false },
                { false, true,  false, false, false },
                { true,  true,  false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 5,
                actual: Conway.CountNeighbours(grid, 2, 0)
            );

            grid = new bool[,] {
                { false, false, false, false, false },
                { true,  true,  false, false, false },
                { true,  true,  false, false, false },
                { true,  true,  false, false, false },
                { false, false, false, false, false }
            };

            TestFramework.Assert.AreEqual(
                expected: 5,
                actual: Conway.CountNeighbours(grid, 2, 0)
            );
        }
    }
    static class D_Test_UpdateGrid
    {
        public static void UpdateGrid_RunsWithoutErrors()
        {
            bool[,] grid;

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            Conway.UpdateGrid(grid);

            grid = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, false, true,  false, false },
                { false, false, true,  false, false },
                { false, false, false, false, false }
            };

            Conway.UpdateGrid(grid);
        }
        public static void UpdateGrid_Example1_Blinker1()
        {
            bool[,] original = new bool[,] {
                { false, false, false, false, false },
                { false, false, true,  false, false },
                { false, false, true,  false, false },
                { false, false, true,  false, false },
                { false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, false, false },
                { false, false, false, false, false },
                { false, true,  true,  true,  false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            TestFramework.CollectionAssert.AreEqual(expected, actual);
        }
        public static void UpdateGrid_Example2_Blinker2()
        {
            bool[,] original = new bool[,] {
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, true,  true,  true,  false },
                { false, true,  true,  true,  false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, false, false, false },
                { false, false, false, true,  false, false },
                { false, true,  false, false, true,  false },
                { false, true,  false, false, true,  false },
                { false, false, true,  false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            TestFramework.CollectionAssert.AreEqual(expected, actual);
        }
        public static void UpdateGrid_Example3_Blinker3()
        {
            bool[,] original = new bool[,] {
                { false, false, false, false, false, false },
                { false, true,  true,  false, false, false },
                { false, true,  true,  false, false, false },
                { false, false, false, true,  true,  false },
                { false, false, false, true,  true,  false },
                { false, false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, false, false, false },
                { false, true,  true,  false, false, false },
                { false, true,  false, false, false, false },
                { false, false, false, false, true,  false },
                { false, false, false, true,  true,  false },
                { false, false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            TestFramework.CollectionAssert.AreEqual(expected, actual);
        }
        public static void UpdateGrid_Example4_StillLife1()
        {
            bool[,] original = new bool[,] {
                { true,  true, false, false, false, false },
                { true,  true, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { true,  true,  false, false, false, false },
                { true,  true,  false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            TestFramework.CollectionAssert.AreEqual(expected, actual);
        }
        public static void UpdateGrid_Example5_StillLife2()
        {
            bool[,] original = new bool[,] {
                { false, false, false, true,  true,  false },
                { false, false, true,  false, false, true },
                { false, false, false, true,  true,  false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, true,  true,  false },
                { false, false, true,  false, false, true },
                { false, false, false, true,  true,  false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            TestFramework.CollectionAssert.AreEqual(expected, actual);
        }
        public static void UpdateGrid_Example6_StillLife3()
        {
            bool[,] original = new bool[,] {
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, true,  true,  false, false, false },
                { true,  false, false, true, false, false },
                { false, true,  false, true, false, false },
                { false, false, true,  false, false, false }
            };

            bool[,] expected = new bool[,] {
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, true,  true,  false, false, false },
                { true,  false, false, true, false, false },
                { false, true,  false, true, false, false },
                { false, false, true,  false, false, false }
            };

            bool[,] actual = Conway.UpdateGrid(original);

            TestFramework.CollectionAssert.AreEqual(expected, actual);
        }
        public static void UpdateGrid_Example7_Glider()
        {
            //..#
            //#.#
            //.##
            bool[,] original = new bool[,] {
                { false, false, true,  false, false, false },
                { true,  false, true,  false, false, false },
                { false, true,  true,  false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            //#..
            //.##
            //##.
            bool[,] expected1 = new bool[,] {
                { false, true,  false, false, false, false },
                { false, false, true,  true, false, false },
                { false, true,  true,  false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            //.#.
            //..#
            //###
            bool[,] expected2 = new bool[,] {
                { false, false, true, false, false, false },
                { false, false, false, true, false, false },
                { false, true,  true,  true, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false },
                { false, false, false, false, false, false }
            };

            TestFramework.CollectionAssert.AreEqual(expected1, Conway.UpdateGrid(original));
            TestFramework.CollectionAssert.AreEqual(expected2, Conway.UpdateGrid(expected1));
        }
    }
}
