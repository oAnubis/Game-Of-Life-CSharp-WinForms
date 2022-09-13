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

        private int _width;

        private int _height;

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
                    universe[x, y] = new Cell();
                }
            }
        }
    }
}
