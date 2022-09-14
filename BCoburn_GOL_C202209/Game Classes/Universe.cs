using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    // Class for each Universe (Main game board and scratchpad)
    public class Universe
    {
        /*TODO: (Personal) - Likely after final submittal and before input into portfolio.    *Make it work then make it better*
        Restructure as a Dictionary (I believe this could lead  to quicker access in the CountNeighbors and overall simulation performance
        Algorithm is in blue notebook in the binder labeled "My Algorithm Design". The nature of Key,Value leads to near instant access, vs iteration over an entire array.
        Working Theory: When getting into larger cell widths and heights, performance can become an issue, faster processing allows larger universes that run smoothly.
        The algorithm would use an arithmetic based formula to access neighbors, alive states and such. The Height and Width would be used to determine a cells neighbor.
         */

        private int _width;

        private int _height;

        private int _aliveNeighbors;

        // 2D Array holding Cells, Backbone of what is displayed on the Graphics Panel.
        private Cell[,] _universeGrid;

        public Cell[,] UniverseGrid
        {
            get { return _universeGrid; }
            set { _universeGrid = value; }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int AliveNeighbors
        {
            get { return _aliveNeighbors; }
            set { _aliveNeighbors = value; }
        }
        
        // Constructor, Fills the 2D Array with Empty Cells.
        public Universe()
        {
            UniverseGrid = new Cell[30, 30];
            _width = 30;
            _height = 30;
            FillGridArray(UniverseGrid);
        }

        public Universe(int width, int height)
        {
            UniverseGrid = new Cell[width, height];
            FillGridArray(UniverseGrid);
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Counts all living Cells around a specific cell based on cell coordinates passed to 2D Array (x = First Dimension, y = Second Dimension)
        /// </summary>
        public int CountNeighborsFinite(int x, int y)
        {
            // Alive count to be incremented then returned
            int count = 0;

            // Calculates the size of each dimension in the game boards array
            int xLen = _universeGrid.GetLength(0);
            int yLen = _universeGrid.GetLength(1);

            //TODO: Possible refactor opportunity (Optimize the neighbor search to be less checks)
            // Loops through a cells neighbor, counts how many are alive (increments count variable initialized above)
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
                    if (xCheck < 0)
                        continue;
                    if (yCheck < 0)
                        continue;
                    if (xCheck >= xLen)
                        continue;
                    if (yCheck >= yLen)
                        continue;

                    // Increments alive count if neighbors LifeState is alive. Only gets here if found to be inside the universe borders.
                    if (_universeGrid[xCheck, yCheck].Alive)
                        count++;
                }
            }

            // Return the count of alive neighbors to the caller.
            _universeGrid[x, y].AliveNeighbors = count;
            return _universeGrid[x, y].AliveNeighbors;
        }

        public void SetUniverse(Cell[,] toSet)
        {
            UniverseGrid = toSet;
        }

        public void RandomFillUniverse(Cell[,] universe, int seed)
        {
            //TODO: Update Commenting
            Random rnd = new Random(seed);

            bool isAlive = true;

            int randomNum;

            for (int x = 0; x < universe.GetLength(0); x++)
            {
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    randomNum = rnd.Next(0, 10001);

                    if (randomNum >= 6000)
                    {
                        universe[x, y] = new Cell(isAlive);
                    }
                    else
                    {
                        universe[x, y] = new Cell(!isAlive);
                    }
                }
            }
        }

        // Method to fill the 2D Array with Cells.
        public void FillGridArray(Cell[,] universe)
        {
            // Iterates the 1st Dimension: x = Left to Right
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                // Iterates the 2nd Dimension: y = Top to Bottom
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Fills the selected 2D index(x, y) with a Cell object
                    universe[x, y] = new Cell(false);
                }
            }
        }
    }
}
