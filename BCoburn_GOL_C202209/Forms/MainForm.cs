using System;
using System.Drawing;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    public partial class MainForm : Form
    {
        // Game object (Holds game logic)
        private Game game;

        // The Timer class
        private Timer timer = new Timer();

        // Generation count
        private int generations = 0;

        // Game 1st Start Tracker
        private bool isFirstLaunch = true;

        private Color gridColor = Color.Black;

        private Color cellColor = Color.DarkGreen;


        public MainForm()
        {
            InitializeComponent();

            game = new Game();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // Timer defaults to disabled on program launch.
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            // Runs Game Rules to determine what to output in the Next Generation
            game.GameRules();

            // Tells the Panel it needs to redraw.
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        // Forms paint event
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            Cell[,] universe = game.gameBoard.UniverseGrid;

            //TODO: Function Extension (Make possible to choose different images, cell and background colors) Enum or other properties at the top of file?.
            // Spider and Spiderweb images, loading from Resources
            Image spiderWeb = global::BCoburn_GOL_C202209.Properties.Resources.SpiderWeb;
            Image spider = global::BCoburn_GOL_C202209.Properties.Resources.Spider;

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

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
                        e.Graphics.FillRectangle(cellBrush, cellRect);
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
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            // Releases the resources for the gridPen
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / game.gameBoard.UniverseGrid.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / game.gameBoard.UniverseGrid.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                game.ToggleCell(x, y);

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        // TODO: Add Button and Menu Item Functionality Here

        #region Game Commands Menu Item Controls

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion Game Commands Menu Item Controls

        #region ToolStrip Buttons

        /// <summary>
        /// Handles logic for the Game flow button - Changes state based on game state (Running or Paused)
        /// </summary>
        private void gameFlowButton_Click(object sender, EventArgs e)
        {
            // Switch which check if the game has been launched or if program was just opened
            switch (isFirstLaunch)
            {
                // If this is the first run this program launch, this will fire.
                case true:
                    // Enables the timer (First Run)
                    timer.Enabled = true;
                    // Tells program simulation has been ran for the first time.
                    isFirstLaunch = false;
                    // Changes the button to display a pause image.
                    gameFlowButton.Image = BCoburn_GOL_C202209.Properties.Resources.Pause;
                    // Changes the tool tip to Pause (ToolTip = Textbox that displays on mouse hover)
                    gameFlowButton.ToolTipText = "Pause";
                    break;

                // If this is NOT the first run this set of logic will run.
                case false:
                    // Check if timer is running.
                    if (timer.Enabled)
                    {
                        // Stops the timer.
                        timer.Stop();
                        // Changes the button to display a Play image.
                        gameFlowButton.Image = BCoburn_GOL_C202209.Properties.Resources.Play;
                        // Changes tool tip to display Continue. (ToolTip = Textbox that displays on mouse hover)
                        gameFlowButton.ToolTipText = "Continue";
                    }
                    // If timer is not running, this will fire.
                    else
                    {
                        // Restarts the timer.
                        timer.Start();
                        // Changes the button to display a Pause image.
                        gameFlowButton.Image = BCoburn_GOL_C202209.Properties.Resources.Pause;
                        // Changes the tooltip to display Pause. (ToolTip = Textbox that displays on mouse hover)
                        gameFlowButton.ToolTipText = "Pause";
                    }
                    break;
            }
        }

        // Fired when the Next button is clicked on.
        private void nextButton_Click(object sender, EventArgs e)
        {
            // Check to make sure the timer is not running (Game is in a paused state)
            if (!timer.Enabled)
            {
                // Calls the Next Generation method (Advances 1 generation)
                NextGeneration();
            }
        }

        // Fired when the Clear button is clicked.
        private void clearButton_Click(object sender, EventArgs e)
        {
            // Calls the ClearUniverse method in the Game class. (Sets all cells to dead LifeState)
            game.ClearUniverse();
            // Tells the program the graphics panel needs to be redrawn
            graphicsPanel1.Invalidate();
        }

        private void randomizeStripButton1_Click(object sender, EventArgs e)
        {
            //TODO: Update Commenting
            //TODO: Reset Board and Clear Generations, Update Living Cells Members

            Cell[,] universe = game.gameBoard.UniverseGrid;

            Random rnd = new Random();

            game.Seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            game.gameBoard.RandomFillUniverse(universe, game.Seed);

            graphicsPanel1.Invalidate();
        }

        #endregion ToolStrip Buttons

        #region Randomize Settings Dialog

        private void randomizeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Update Commenting
            // Instantiates a Modal Dialog object
            RandomizeModalDialog RandomizeSettingsDialog = new RandomizeModalDialog();

            RandomizeSettingsDialog.Apply += new ApplyEventHandler(RandomizeSettingsDialog_Apply);

            int seedValue = (int)RandomizeSettingsDialog.numericUpDown1.Value;

            RandomizeSettingsDialog.numericUpDown1.Value = game.Seed;

            if (RandomizeSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                seedValue = game.Seed;
                Cell[,] universe = game.gameBoard.UniverseGrid;
                game.gameBoard.RandomFillUniverse(universe, seedValue);
                graphicsPanel1.Invalidate();
            }
            else
            {
            }

            RandomizeSettingsDialog.Dispose();
        }

        private void RandomizeSettingsDialog_Apply(object sender, RandomApplyArgs e)
        {
            int seed = e.Seed;
            game.Seed = seed;
        }

        #endregion Randomize Settings Dialog

        private void stopButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}