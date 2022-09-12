﻿using System.Drawing;

namespace BCoburn_GOL_C202209
{
    internal class Game
    {
        // PROPERTIES FOR THE GAME

        // Main universe, holds an array of cells. This is what is shown (Current generation)
        public Universe gameBoard { get; private set; }

        // Scratchpad, holds an array of cells. This holds the next generation to be shown
        public Universe scratchPad { get; private set; }

        public int Seed { get; set; }

        // Determines the color of the gridlines
        //TODO: Allow to be customizable
        private Color gridColor = Color.Black;

        private Color cellColor = Color.DarkGreen;

        // Game constructor - Creates the gameboard and the scratchpad. Called on program launch
        public Game()
        {
            gameBoard = new Universe();
            scratchPad = new Universe();
        }

        ///// <summary>
        ///// This method is called from Form1 Paint event. Fills in the board based on game rules (Alive or Dead)
        ///// </summary>
        //public void DrawBoard(GraphicsPanel panel, Graphics graphics)
        //{
        //    //TODO: Function Extension (Make possible to choose different images, cell and background colors) Enum or other properties at the top of file?.
        //    // Spider and Spiderweb images, loading from Resources
        //    Image spiderWeb = global::BCoburn_GOL_C202209.Properties.Resources.SpiderWeb;
        //    Image spider = global::BCoburn_GOL_C202209.Properties.Resources.Spider;

        //    // Calculate the width and height of each cell in pixels
        //    // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
        //    int cellWidth = panel.ClientSize.Width / gameBoard.UniverseGrid.GetLength(0);
        //    // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
        //    int cellHeight = panel.ClientSize.Height / gameBoard.UniverseGrid.GetLength(1);

        //    // A Pen for drawing the grid lines (color, width)
        //    Pen gridPen = new Pen(gridColor, 1);

        //    // A Brush for filling living cells interiors (color)
        //    Brush cellBrush = new SolidBrush(cellColor);

        //    // Iterate through the universe in the y, top to bottom
        //    for (int y = 0; y < gameBoard.UniverseGrid.GetLength(1); y++)
        //    {
        //        // Iterate through the universe in the x, left to right
        //        for (int x = 0; x < gameBoard.UniverseGrid.GetLength(0); x++)
        //        {
        //            // A rectangle to represent each cell in pixels
        //            Rectangle cellRect = Rectangle.Empty;
        //            cellRect.X = x * cellWidth;
        //            cellRect.Y = y * cellHeight;
        //            cellRect.Width = cellWidth;
        //            cellRect.Height = cellHeight;

        //            if (gameBoard.UniverseGrid[x, y].Alive)
        //            {
        //                graphics.FillRectangle(cellBrush, cellRect);
        //            }
        //            else
        //            {
        //            }

        //            // Fill the cell with a Spider if alive, or a CobWeb if dead
        //            //TODO: Reimplement Images
        //            //if (gameBoard.UniverseGrid[x, y].Alive)
        //            //{
        //            //    graphics.DrawImage(spider, cellRect);
        //            //}
        //            //else
        //            //{
        //            //    graphics.DrawImage(spiderWeb, cellRect);
        //            //}

        //            // Outline the cell with a pen
        //            graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
        //        }
        //    }

        //    // Releases the resources for the gridPen
        //    gridPen.Dispose();
        //    cellBrush.Dispose();
        //}

        /// <summary>
        /// Method to swap the universe and scratchpad arrays (Make Next generation the current generation)
        /// </summary>
        public void SwapBoards()
        {
            Cell[,] temp = gameBoard.UniverseGrid;
            gameBoard.UniverseGrid = scratchPad.UniverseGrid;
            scratchPad.UniverseGrid = temp;
        }

        /// <summary>
        /// Counts all living Cells around a specific cell based on cell coordinates passed to 2D Array (x = First Dimension, y = Second Dimension)
        /// </summary>
        private int CountNeighborsFinite(int x, int y)
        {
            // Creates local variables for the Universe and ScratchPad's Cell Arrays
            Cell[,] universe = gameBoard.UniverseGrid;
            Cell[,] scratch = scratchPad.UniverseGrid;

            // Alive count to be incremented then returned
            int count = 0;

            // Calculates the size of each dimension in the game boards array
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

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
                    if (universe[xCheck, yCheck].Alive)
                        count++;
                }
            }

            // Return the count of alive neighbors to the caller.
            return count;
        }

        /// <summary>
        /// Method that holds the rules of the game - Determines if the cell lives or dies in the next generation.
        /// </summary>
        public void GameRules()
        {
            // Variables for Universe and Scratchpad 2D Arrays.
            Cell[,] universe = gameBoard.UniverseGrid;
            Cell[,] scratch = scratchPad.UniverseGrid;

            // Iterate through 1st Dimension of the Universe:  x = Left to Right.
            for (int x = 0; x < universe.GetLength(1); x++)
            {
                // Iterate through 2nd Dimension of the Universe: y = Top to Bottom.
                for (int y = 0; y < universe.GetLength(0); y++)
                {
                    // Holds value of how many living neighbors, Calculated with the CountNeighborsFinite Method.
                    int count = CountNeighborsFinite(x, y);

                    // Holds the LifeState of the currently selected cell
                    bool lifeState = universe[x, y].GetLifeState();

                    //TODO: Possible refactor opportunity (?Check life state, then run the rules?)
                    // Rule: Any living cell with less than 2 living neighbors dies, as if by under-population.
                    if (count < 2 && lifeState == true)
                    {
                        scratch[x, y].SetLifeState(false);
                    }
                    // Rule: Any living cell with more than 3 living neighbors dies, as if by over-population.
                    else if (count > 3 && lifeState == true)
                    {
                        scratch[x, y].SetLifeState(false);
                    }
                    // Rule: Any dead cell with exactly 3 living neighbors are born again, as if by reproduction.
                    else if (count == 3 && lifeState == false)
                    {
                        scratch[x, y].SetLifeState(true);
                    }
                    // Rule: Any living cell with 2 or 3 living neighbors will continue to live (Not over or under populated).
                    else
                    {
                        scratch[x, y].SetLifeState(universe[x, y].GetLifeState());
                    }
                }
            }

            // Method to swap the scratchpad and the universe, to display the next generation.
            SwapBoards();
        }

        /// <summary>
        /// Called when Clear Button on the form is pressed, makes all cells LifeState "dead".
        /// </summary>
        public void ClearUniverse()
        {
            Cell[,] universe = gameBoard.UniverseGrid;
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Checks if the selected cells LifeState, if its alive switches to dead
                    if (universe[x, y].Alive)
                    {
                        universe[x, y].SetLifeState(!universe[x, y].Alive);
                    }
                }
            }
        }

        /// <summary>
        /// Called when a cell is clicked in the form, Toggles LifeState.
        /// </summary>
        public void ToggleCell(int x, int y)
        {
            Cell[,] universe = gameBoard.UniverseGrid;
            // Toggles LifeState to opposite of current LifeState.
            universe[x, y].SetLifeState(!universe[x, y].Alive);
        }
    }
}