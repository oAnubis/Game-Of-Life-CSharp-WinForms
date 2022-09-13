using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace BCoburn_GOL_C202209
{
    public partial class MainForm : Form
    {
        // Game object (Holds game logic)
        private Game game;

        // The Timer class
        private Timer timer = new Timer();

        // Timer Interval Property
        public int timerInterval { get; private set; }

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
            timerInterval = 100;

            // Setup the timer
            timer.Interval = timerInterval; // milliseconds
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

        public void UpdateSeedLabel()
        {
            toolStripStatusLabelSeed.Text = "Current Seed = " + game._seed;
        }

        public void UpdateAliveLabel(int totalAlive)
        {
            toolStripStatusLabelAliveCount.Text = "Cells Alive = " + game.CountTotalAlive().ToString();
        }

        // Forms paint event
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            game.PaintBoard(graphicsPanel1, e.Graphics);

            UpdateAliveLabel(game.CountTotalAlive());
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            game.ToggleCell(graphicsPanel1, e);
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

        // Stops the Simulation, and Resets the counts and universe
        private void stopButton_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                gameFlowButton_Click(sender, e);
            }

            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            clearButton_Click(sender, e);
            graphicsPanel1.Invalidate();
        }

        private void randomizeStripButton1_Click(object sender, EventArgs e)
        {
            //TODO: Update Commenting
            //TODO: Reset Board and Clear Generations, Update Living Cells Members

            Cell[,] universe = game.gameBoard.UniverseGrid;

            Random rnd = new Random();

            game._seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            game.gameBoard.RandomFillUniverse(universe, game._seed);

            UpdateSeedLabel();

            graphicsPanel1.Invalidate();
        }

        private void timeRandomStripButton1_Click(object sender, EventArgs e)
        {
            Cell[,] universe = game.gameBoard.UniverseGrid;

            Random rnd = new Random(DateTime.Now.Millisecond);

            game._seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            game.gameBoard.RandomFillUniverse(universe, game._seed);

            UpdateSeedLabel();

            graphicsPanel1.Invalidate();
        }

        private void toolStripButtonCurrentSeed_Click(object sender, EventArgs e)
        {
            Cell[,] universe = game.gameBoard.UniverseGrid;

            game.gameBoard.RandomFillUniverse(universe, game._seed);

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

            RandomizeSettingsDialog.numericUpDown1.Value = game._seed;

            if (RandomizeSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                seedValue = game._seed;
                Cell[,] universe = game.gameBoard.UniverseGrid;
                game.gameBoard.RandomFillUniverse(universe, seedValue);
                UpdateSeedLabel();
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
            game._seed = seed;
        }

        #endregion Randomize Settings Dialog


        #region Game Options Dialog

        //TODO: Update Comments
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsModalDialog optionsDialog = new OptionsModalDialog();

            optionsDialog.Apply += OptionsDialog_Apply;

            optionsDialog.numericUpDownTimer.Value = timerInterval;

            optionsDialog.numericUpDownHeight.Value = game.gameBoard.Height;

            optionsDialog.numericUpDownWidth.Value = game.gameBoard.Width;

            if (DialogResult.OK == optionsDialog.ShowDialog())
            {
                optionsDialog.TimerInterval = (int)optionsDialog.numericUpDownTimer.Value;
                optionsDialog.Height = (int)optionsDialog.numericUpDownHeight.Value;
                optionsDialog.Width = (int)optionsDialog.numericUpDownWidth.Value;
            }

        }

        void OptionsDialog_Apply(object sender, OptionsApplyArgs e)
        {
            timerInterval = e.TimerInterval;
            timer.Interval = e.TimerInterval;
            //TODO: Track and Apply Seed When Making a New Game.
            if (e.Height != game.gameBoard.Height || e.Width != game.gameBoard.Width)
            {
                game = new Game(e.Width, e.Height);
                if (game._seed != 0)
                {
                    
                }
            }

            graphicsPanel1.Invalidate();
        }

        #endregion





    }
}