using System.Drawing;
using System.Windows.Forms;
using BCoburn_GOL_C202209.Properties;

namespace BCoburn_GOL_C202209
{
    public class Game
    {
        // PROPERTIES FOR THE GAME
        //TODO: Change most logic from form to here in the form of Methods

        // Main universe, holds an array of cells. This is what is shown (Current generation)
        public Universe GameBoard;

        // Scratchpad, holds an array of cells. This holds the next generation to be shown
        public Universe ScratchPad;

        // Hold the value of the current MainForm
        private MainForm _mainForm;

        // The width (x dimension) of the current gameBoard.
        public int Width { get; private set; }

        // The height (y dimension) of the current gameBoard.
        public int Height { get; private set; }

        // The current seed set in the game.
        public int Seed;

        // Determines the color of the gridlines
        //TODO: Allow to be customizable
        // Determines the Color of the Grid Lines (Component: 1)
        public Color GridColor { get; private set; }

        // Determines the color of Alive Cells. (Component: 2)
        public Color CellColor { get; private set; }

        // Determines the Color of the Universe (Component: 3)
        public Color UniverseColor { get; private set; }

        // Determines the Color of the HUD Text (Component: 4)
        public Color HUDColor { get; private set; }

        // Sets the Font of the numbers shown inside each Cell (If the setting in the view menu is checked).
        private Font _cellFont = new Font("Cascadia Mono", 8f, FontStyle.Bold);

        // Instantiates a new StringFormat object, this allows the numbers to be centered in each rectangle in the gameBoard.
        private StringFormat _cellStringFormat = new StringFormat();

        //Game constructor - Calls the IntializeObjects method. Also stores the current MainForm.
        public Game(MainForm form)
        {
            InitializeObjects();
            _mainForm = form;
        }

        public void Game_ApplyColors(object sender, ColorsApplyArgs e)
        {
            GridColor = e.GridColor;
            CellColor = e.CellColor;
            UniverseColor = e.UniverseColor;
            HUDColor = e.HUDColor;
        }

        public void Game_ApplyOneColor(object sender, ColorsApplyArgs e, int componentNumber)
        {
            switch (componentNumber)
            {
                case 1:
                    GridColor = e.AnyColor;
                    break;

                case 2:
                    CellColor = e.AnyColor;
                    break;

                case 3:
                    UniverseColor = e.AnyColor;
                    break;

                case 4:
                    HUDColor = e.AnyColor;
                    break;
            }
        }

        /// <summary>
        /// Intialize the starting Properties and Game Objects.
        /// </summary>
        private void InitializeObjects()
        {
            RevertSettings();
            GameBoard = new Universe(Width, Height);
            ScratchPad = new Universe(Width, Height);
        }

        public void DefaultSettings()
        {
            Height = Settings.Default.UniverseHeightDefault;
            Width = Settings.Default.UniverseWidthDefault;
            CellColor = Settings.Default.CellColorDefault;
            UniverseColor = Settings.Default.UniverseColorDefault;
            GridColor = Settings.Default.GridColorDefault;
            HUDColor = Settings.Default.HUDColorDefault;
        }

        public void RevertSettings()
        {
            Height = Settings.Default.UniverseHeight;
            Width = Settings.Default.UniverseWidth;
            CellColor = Settings.Default.CellColor;
            UniverseColor = Settings.Default.UniverseColor;
            GridColor = Settings.Default.GridColor;
            HUDColor = Settings.Default.HUDColor;
        }

        /// <summary>
        /// Sets the Width and Height properties. This controls the size of the grid (x = Width, y = Height).
        /// </summary>
        /// <param name="width"> The width to set the gameBoard to (x dimension) </param>
        /// <param name="height"> The height to set the gameBoard to (y dimension) </param>
        public void SetBoardSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Handles drawing of the gameBoard to the GraphicsPanel
        /// </summary>
        /// <param name="panel"> The GraphicsPanel to Paint to. </param>
        /// <param name="graphics"> The Graphics object, allows drawing to the GraphicsPanel. </param>
        public void PaintBoard(Panel panel, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)panel.ClientSize.Width / Width;
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)panel.ClientSize.Height / Height;

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(GridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(CellColor);

            Brush universeBrush = new SolidBrush(UniverseColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < Height; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < Width; x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Checks the life state of the Cell.
                    if (GameBoard.LifeState(x, y) == true)
                    {
                        // Fills the rectangle with the Color selected for living cells.
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }
                    else
                    {
                        //TODO: Add ability to color dead cell.
                        // Fills the rectangle with the Color Selected for dead cells.
                        e.Graphics.FillRectangle(universeBrush, cellRect);
                    }

                    //TODO: Reimplement Images

                    if (_mainForm.ShowGrid == ShowGrid.Yes)
                    {
                        // Outline the cell with a pen
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellWidth, cellHeight);
                    }

                    // Checks if showNumbers is true (Defaults to yes).
                    if (_mainForm.DisplayNumbers == DisplayNumbers.Yes)
                    {
                        // Checks if the cell has any living neighbors (Only draws a number if there is at least 1 living neighbor)
                        if (GameBoard.UniverseGrid[x, y].AliveNeighbors > 0)
                        {
                            // Calls the PrintNumbers method, Handles drawing the number inside the Cell.
                            PrintNumbers(e, x, y, cellRect);
                        }
                    }
                }
            }

            // Releases the resources for the gridPen
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        /// <summary>
        /// Draws the number of living neighbors inside the cell. (Number is centered).
        /// </summary>
        /// <param name="graphics"> Allows the DrawString Method to be called on the GraphicsPanel. </param>
        /// <param name="x"> The x index of the Cell. </param>
        /// <param name="y"> The y index of the Cell. </param>
        /// <param name="cellRect"> Visually represents the cell as a RectangleF. </param>
        public void PrintNumbers(PaintEventArgs e, int x, int y, RectangleF cellRect)
        {
            // Sets the alignment of the number to be centered inside each Cell.
            _cellStringFormat.Alignment = StringAlignment.Center;
            _cellStringFormat.LineAlignment = StringAlignment.Center;

            // Checks if the cell will be Alive in the next generation (scratchPad holds the next generation state)
            // This is what allows the numbers to be colored based on if they will be alive the next generation or not.
            if (ScratchPad.UniverseGrid[x, y].Alive)
            {
                // Colors the numbers in the Cells that will be alive next generation green.
                e.Graphics.DrawString(GameBoard.GetAliveNeighbors(x, y).ToString(), _cellFont, Brushes.Green, cellRect, _cellStringFormat);
            }
            else
            {
                // Colors the numbers in the Cells that will be dead next generation red.
                e.Graphics.DrawString(GameBoard.GetAliveNeighbors(x, y).ToString(), _cellFont, Brushes.Red, cellRect, _cellStringFormat);
            }
        }

        /// <summary>
        /// Method to swap the universe and scratchpad arrays (Make Next generation the current generation)
        /// Utilized the SetUniverse method in the Universe class (Required because the Universe Setter is private.
        /// </summary>
        public void SwapBoards()
        {
            Cell[,] temp = GameBoard.UniverseGrid;
            GameBoard.SetUniverse(ScratchPad.UniverseGrid);
            ScratchPad.SetUniverse(temp);
        }

        /// <summary>
        /// Method that holds the rules of the game - Determines if the cell lives or dies in the next generation.
        /// </summary>
        public void GameRules()
        {
            // Variables for Universe and Scratchpad 2D Arrays.
            Cell[,] scratch = ScratchPad.UniverseGrid;

            int count = 0;

            // Iterate through y Dimension of the Universe:  y = Top to Bottom.
            for (int y = 0; y < Height; y++)
            {
                // Iterate through x Dimension of the Universe: x = Left to Right.
                for (int x = 0; x < Width; x++)
                {
                    if (_mainForm.BorderMode == BorderMode.Finite)
                    {
                        // Holds value of how many living neighbors, Calculated with the CountNeighborsFinite method.
                        count = GameBoard.CountNeighborsFinite(x, y);
                    }
                    else
                    {
                        // Holds value of how many living neighbors, Calculated with the CountNeighborsToroidal method.
                        count = GameBoard.CountNeighborsToroidal(x, y);
                    }

                    // Holds the LifeState of the currently selected cell
                    bool lifeState = GameBoard.LifeState(x, y);

                    //TODO: Possible refactor opportunity (?Check life state, then run the rules?)

                    //Rule: Any living cell with less than 2 living neighbors dies, as if by under-population.
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
                        scratch[x, y].SetLifeState(GameBoard.LifeState(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Called when Clear Button on the form is pressed, makes all cells LifeState "dead". (Empties the grid).
        /// </summary>
        public void ClearUniverse()
        {
            // Variable to hold the actual array in the Universe object (The gameBoard).
            Cell[,] universe = GameBoard.UniverseGrid;

            // Iterates over the y dimension (Height)
            for (int y = 0; y < Height; y++)
            {
                // Iterates over the x dimension (Width)
                for (int x = 0; x < Width; x++)
                {
                    // Checks if the selected cells life state is alive,
                    if (universe[x, y].Alive)
                    {
                        // If the Cell is alive, set it to dead.
                        universe[x, y].SetLifeState(!universe[x, y].Alive);
                    }
                }
            }
        }

        /// <summary>
        /// Called when the GraphicsPanel is Clicked, Handles toggling the life state of the clicked Cell.
        /// </summary>
        /// <param name="panel"> The GraphicsPanel that was clicked on. </param>
        /// <param name="e"> The events that are passed when the GraphicsPanel is clicked on, stores several different variables. </param>
        public void ToggleCell(Panel panel, MouseEventArgs e)
        {
            // A variable holding the actual array of the Universe object (gameBoard)
            Cell[,] universe = GameBoard.UniverseGrid;

            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)panel.ClientSize.Width / Width;
                float cellHeight = (float)panel.ClientSize.Height / Height;

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = e.Y / cellHeight;

                // Toggles LifeState to opposite of current life state. Handled with the SetLifeState method in the Cell class.
                universe[(int)x, (int)y].SetLifeState(!universe[(int)x, (int)y].Alive);

                // Tells the system to repaint the GraphicsPanel.
                panel.Invalidate();
            }
        }

        /// <summary>
        /// Counts the total amount of living cells in the Universe.
        /// </summary>
        /// <returns> Returns the number of living cells. </returns>
        public int CountTotalAlive()
        {
            // An int value to hold the number of counted living cells.
            int totalAlive = 0;

            // Iterates the y dimension of the Universe (Height)
            for (int y = 0; y < Height; y++)
            {
                // Iterates the x dimension of the Universe (Width)
                for (int x = 0; x < Width; x++)
                {
                    // Checks if the specified cell is living.
                    if (GameBoard.UniverseGrid[x, y].Alive)
                    {
                        // Increments totalAlive variable (Counts the living cells).
                        totalAlive++;
                    }
                }
            }
            // Return the total number of living cells after iterating through the entire Universe.
            return totalAlive;
        }
    }
}