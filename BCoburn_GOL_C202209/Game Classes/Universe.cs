using System;

namespace BCoburn_GOL_C202209
{
    // Class for each Universe (Main game board and scratchpad)
    public class Universe
    {
        // 2D Array holding Cell objects, Backbone of what is displayed on the Graphics Panel.
        public Cell[,] UniverseGrid { get; private set; }

        // Constructor, Instantiates UniverseGrid, then proceeds to fill the array with cell objects.
        public Universe(int width, int height)
        {
            UniverseGrid = new Cell[width, height];
            FillGridArray(UniverseGrid);
        }

        /// <summary>
        /// Counts the Neighbors of a specific Cell, Border is finite, any Cell "outside" the board is assumed dead.
        /// </summary>
        /// <param name="x"> The x index of the Cell. </param>
        /// <param name="y"> The y index of the Cell. </param>
        /// <returns> Returns an int holding the number of alive neighbors of the specified Cell. </returns>
        public int CountNeighborsFinite(int x, int y)
        {
            // Instantiates an int variable to hold the count of alive neighbors.
            int count = 0;

            // Calculates the size of each dimension in the game boards array (0 = x; 1 = y)
            int xLen = UniverseGrid.GetLength(0);
            int yLen = UniverseGrid.GetLength(1);

            // Loops through a cells neighbor, counts how many are alive (increments count variable initialized above)
            #region How This Works

            // Top Cell:==========yOffset = -1, xOffset = 0
            // Top Left Cell:=====yOffset = -1, xOffset = -1
            // Top Right Cell:====yOffset = -1, xOffset = 1
            // Left Cell:=========yOffset = 0, xOffset = -1
            // Right Cell:========yOffset = 0, xOffset = 1
            // Bottom Left Cell:==yOffset = 1, xOffset = -1
            // Bottom Cell:=======yOffset = 1, xOffset = 0
            // Bottom Right Cell:=yOffset = 1, xOffset = 1

            #endregion How This Works
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    // Offset for each dimension, helpers to find the right neighbors or determine if out of bounds
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    // Current Cell (Not a Neighbor)
                    if (xOffset == 0 && yOffset == 0)
                        continue;

                    // These represent coordinates outside of the universe borders (Assumed dead)
                    if (xCheck < 0 || yCheck < 0 || xCheck >= xLen || yCheck >= yLen)
                        continue;

                    // Increments alive count if neighbors LifeState is alive.
                    if (UniverseGrid[xCheck, yCheck].Alive)
                        count++;
                }
            }

            // Sets the Cells AliveNeighbor property to the number of alive neighbors (held by the count variable)
            UniverseGrid[x, y].AliveNeighbors = count;

            // Returns the count variable to the caller.
            return count;
        }

        /// <summary>
        /// Counts the alive neighbors around a specified cell in the UniverseGrid. The borders wrap around.
        /// </summary>
        /// <param name="x"> The x index of the Cell. </param>
        /// <param name="y"> The y index of the Cell. </param>
        /// <returns> Returns the amount of alive neighbors around the specified Cell. </returns>
        public int CountNeighborsToroidal(int x, int y)
        {
            // Alive count to be incremented then returned
            int count = 0;

            // Calculates the size of each dimension in the game boards array
            int xLen = UniverseGrid.GetLength(0);
            int yLen = UniverseGrid.GetLength(1);

            // This nested for loop searches the cells around the perimeter of a cell.
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    // Offset for each dimension, helpers to find the right neighbors or determine if out of bounds
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    // Current Cell (Not a Neighbor)
                    if (xOffset == 0 && yOffset == 0)
                        continue;

                    // This covers the left most edge of the board. Sets the xCheck to the last Cell in the row
                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }

                    // This covers the top most edge of the board. Sets yCheck to the bottom Cell in the column
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }

                    // This covers the right most edge of the board. Sets xCheck to the first Cell in the row.
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }

                    // This covers the bottom most edge of the board. Sets yCheck to the first Cell in the column.
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    // Checks the Cell found at the specified coordinates for life state.
                    if (UniverseGrid[xCheck, yCheck].Alive)
                        // Increments alive count if neighbors LifeState is alive.
                        count++;
                }
            }

            // Sets the specified Cells AliveNeighbor Property to the amount of Alive Neighbors found, held by the count variable.
            UniverseGrid[x, y].AliveNeighbors = count;

            // Return the count of alive neighbors to the caller.
            return count;
        }

        /// <summary>
        /// Small Helper function to allow the UniverseGrid to be set in the SwapBoard Function in the Game Class (UniverseGrid setter is private)
        /// </summary>
        /// <param name="toSet"> The UniverseGrid to copy. </param>
        public void SetUniverse(Cell[,] toSet)
        {
            UniverseGrid = toSet;
        }

        /// <summary>
        /// Fills the UniverseGrid with Cells whose life state is randomly determined by an RNG.
        /// </summary>
        /// <param name="universe"> The universe to work with. </param>
        /// <param name="seed"> The seed to pass into the RNG. </param>
        public void RandomFillUniverse(Cell[,] universe, int seed)
        {
            // Instantiates a Random object to act as the Random Number Generator (RNG) passes a seed to determine a starting value.
            Random rnd = new Random(seed);

            // Instantiates a bool variable defaulted to true
            bool isAlive = true;

            // Declares an int variable to hold the generated random number
            int randomNum;

            // Iterates 1st Dimension of the universe. left to right
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                // Iterates 2nd Dimension of the universe. Top to Bottom
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Generates a random number between 0 and 10,000
                    randomNum = rnd.Next(0, 10001);

                    // Check if the random number is 5,000 or greater.
                    if (randomNum >= 5000)
                    {
                        // Inserts a new Cell with a state of alive. Places in the array at the (x, y) index.
                        universe[x, y] = new Cell(isAlive);
                    }
                    else
                    {
                        // Inserts a new Cell with a state of dead. Places in the array at the (x, y) index.
                        universe[x, y] = new Cell(!isAlive);
                    }
                }
            }
        }

        /// <summary>
        /// Fills the Universe Grid with Cells, the life states are defaulted to dead. (A blank Grid).
        /// </summary>
        /// <param name="universe"> The UniverseGrid to work with. </param>
        public void FillGridArray(Cell[,] universe)
        {
            // Iterates the 1st Dimension: x = Left to Right
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                // Iterates the 2nd Dimension: y = Top to Bottom
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Inserts a new Cell with a state of dead. Places in the array at the (x, y) index.
                    universe[x, y] = new Cell(false);
                }
            }
        }

        /// <summary>
        /// Retrieves the AliveNeighbors Property of the Specified Cell in the UniverseGrid Array.
        /// </summary>
        /// <param name="x"> The x Index to search. </param>
        /// <param name="y"> The y Index to search. </param>
        /// <returns> Returns the number of alive neighbors of the specified Cell. </returns>
        public int GetAliveNeighbors(int x, int y)
        {
            return UniverseGrid[x, y].AliveNeighbors;
        }

        /// <summary>
        /// Retrieves the life state of the Specified Cell in the UniverseGrid.
        /// </summary>
        /// <param name="x"> The x index to search </param>
        /// <param name="y"> The y index to search </param>
        /// <returns> Returns the life state of the specified Cell (true = alive, false = dead) </returns>
        public bool LifeState(int x, int y)
        {
            return UniverseGrid[x, y].Alive;
        }

        /// <summary>
        /// Sets the life state of the specified Cell in the UniverseGrid.
        /// </summary>
        /// <param name="x"> The x index to search. </param>
        /// <param name="y"> The y index to search. </param>
        /// <param name="state"> The life state to set in the specified Cell. </param>
        public void SetCellState(int x, int y, bool state)
        {
        }
    }
}