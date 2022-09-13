using System.Drawing;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    public class Game
    {
        // PROPERTIES FOR THE GAME
        //TODO: Change most logic from form to here in the form of Methods
        // Main universe, holds an array of cells. This is what is shown (Current generation)
        public Universe gameBoard;

        // Scratchpad, holds an array of cells. This holds the next generation to be shown
        public Universe scratchPad;

        public int _seed;

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

        public Game(int width, int height)
        {
            gameBoard = new Universe(width, height);
            scratchPad = new Universe(width, height);
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

        public void PaintBoard(Panel panel, Graphics graphics)
        {
            Cell[,] universe = gameBoard.UniverseGrid;

            Font cellFont = new Font("Arial", 10f);

            StringFormat cellStringFormat = new StringFormat();
            cellStringFormat.Alignment = StringAlignment.Center;
            cellStringFormat.LineAlignment = StringAlignment.Center;

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)panel.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)panel.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    if (universe[x, y].Alive)
                    {
                        graphics.FillRectangle(cellBrush, cellRect);
                    }
                    else
                    {
                    }

                    // Fill the cell with a Spider if alive, or a CobWeb if dead
                    //TODO: Reimplement Images
                    //if (gameBoard.UniverseGrid[x, y].Alive)
                    //{
                    //    graphics.DrawImage(spider, cellRect);
                    //}
                    //else
                    //{
                    //    graphics.DrawImage(spiderWeb, cellRect);
                    //}

                    // Outline the cell with a pen
                    graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                    
                    gameBoard.CountNeighborsFinite(x, y);

                    if (gameBoard.UniverseGrid[x, y].AliveNeighbors > 0)
                    {
                        graphics.DrawString(gameBoard.UniverseGrid[x, y].AliveNeighbors.ToString(), cellFont, Brushes.Black, cellRect, cellStringFormat);
                    }
                }
            }

            // Releases the resources for the gridPen
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        /// <summary>
        /// Method to swap the universe and scratchpad arrays (Make Next generation the current generation)
        /// </summary>
        public void SwapBoards()
        {
            Cell[,] temp = gameBoard.UniverseGrid;
            gameBoard.SetUniverse(scratchPad.UniverseGrid);
            scratchPad.SetUniverse(temp);
        }

        /// <summary>
        /// Method that holds the rules of the game - Determines if the cell lives or dies in the next generation.
        /// </summary>
        public void GameRules()
        {
            // Variables for Universe and Scratchpad 2D Arrays.
            Cell[,] universe = gameBoard.UniverseGrid;
            Cell[,] scratch = scratchPad.UniverseGrid;

            int totalAlive = 0;

            // Iterate through 1st Dimension of the Universe:  x = Left to Right.
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                // Iterate through 2nd Dimension of the Universe: y = Top to Bottom.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Holds value of how many living neighbors, Calculated with the CountNeighborsFinite Method.
                    int count = universe[x, y].AliveNeighbors;

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

        //TODO: Add a next generation method to determine if the cell would be alive the next generation (Used for painting numbers also)

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
        public void ToggleCell(Panel panel, MouseEventArgs e)
        {
            Cell[,] universe = gameBoard.UniverseGrid;
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)panel.ClientSize.Width / universe.GetLength(0);
                float cellHeight = (float)panel.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = e.Y / cellHeight;

                // Toggles LifeState to opposite of current LifeState.
                universe[(int)x, (int)y].SetLifeState(!universe[(int)x, (int)y].Alive);

                panel.Invalidate();
            }
        }

        public int CountTotalAlive()
        {
            int totalAlive = 0;
            for (int x = 0; x < gameBoard.UniverseGrid.GetLength(0); x++)
            {
                for (int y = 0; y < gameBoard.UniverseGrid.GetLength(1); y++)
                {
                    if (gameBoard.UniverseGrid[x, y].Alive)
                    {
                        totalAlive++;
                    }
                }
            }

            return totalAlive;
        }
    }
}