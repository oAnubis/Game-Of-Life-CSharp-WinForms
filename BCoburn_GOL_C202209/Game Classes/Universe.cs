using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCoburn_GOL_C202209
{
    // Class for each Universe (Main game board and scratchpad)
    public class Universe
    {
        // 2D Array holding Cells, Backbone of what is displayed on the Graphics Panel.
        public Cell[,]UniverseGrid = new Cell[50, 50];

        public int RandomSeed { get; private set; }

        // public int PercentFill { get; private set; }

        // Constructor, Fills the 2D Array with Cells.
        public Universe()
        {
            FillGridArray(UniverseGrid);
        }

        public Universe(int seed)
        {
            RandomFillUniverse(UniverseGrid, seed);
        }

        // A setter for the RandomSeed property
        public void SetRandomSeed(int seed)
        {
            RandomSeed = seed;
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
                    universe[x, y] = new Cell();
                }
            }
        }

        
    }
}
