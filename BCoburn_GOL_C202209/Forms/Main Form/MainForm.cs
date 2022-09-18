using System;
using System.Drawing;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    public enum BorderMode
    {
        Toroidal,
        Finite
    }

    public partial class MainForm : Form
    {
        #region Events

        public event ApplyViewEventHandler Apply;

        #endregion

        #region Properties

        // Game object (Holds game logic).
        private Game game;

        // The Timer class.
        private Timer timer = new Timer();

        // Timer Interval Property (Speed of each generation, in milliseconds).
        public int timerInterval { get; private set; }

        // Generation count.
        public int generations { get; private set; }

        public BorderMode borderMode { get; private set; }

        #endregion Properties

        #region Fields

        // Game 1st Start Tracker (Has the simulation been ran since the program was opened).
        private bool isFirstLaunch = true;

        // Holds whether the Adjacent Count in the View menu is checked.
        private bool _showNumbers = true;

        // Whether the Grid in the view menu is checked.
        private bool _showGrid = true;

        // Whether the Finite in the view menu is checked (Only 1 needs to be tracked to set the boundry mode)
        private bool _isFinite = true;

        // Whether HUD is checked in the view menu.
        private bool _showHUD = true;

        #endregion Fields

        #region Constructors

        public MainForm()
        {
            // Initializes all the components of the form (Buttons, Labels, MenuItems, etc...)
            InitializeComponent();

            // Sets the default border mode to Finite.
            this.borderMode = BorderMode.Finite;

            // Initialize a new instance of the Game class.
            game = new Game(this);

            // Sets the default interval for the timer, in milliseconds.
            timerInterval = 20;

            // Setup the timer
            timer.Interval = timerInterval; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // Timer defaults to disabled on program launch.
        }

        #endregion Constructors

        #region Getters

        public bool GetBorderMode()
        {
            return _isFinite;
        }

        #endregion

        #region Forms Main Methods

        // Shows the next generation on the GraphicsPanel
        private void NextGeneration()
        {
            // Increment generation count
            generations++;

            // Fires SwapBoards from the Game Class, Swapping Boards is what makes the next generation actually display.
            game.SwapBoards();

            // Tells the Panel it needs to redraw.
            graphicsPanel1.Invalidate();
        }

        // Calls the NextGeneration method each tick of the timer
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        // Updates the Status Strip
        public void UpdateStausStrip()
        {
            // Updates the Current Seed label on the Status Strip.
            toolStripStatusLabelSeed.Text = "Current Seed = " + game._seed;

            // Updates the Cells Alive label on the Status Strip.
            toolStripStatusLabelAliveCount.Text = "Cells Alive = " + game.CountTotalAlive().ToString();

            // Updates the Generations label on the Status Strip.
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
        }

        /// Small helper method to reset the generation count to 0
        public void ResetGenerations()
        {
            generations = 0;
        }

        /// Forms Paint event for the Main GraphicsPanel.
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Runs Game Rules to determine what to output in the Next Generation.
            game.GameRules();

            game.PaintBoard(graphicsPanel1, e);

            UpdateStausStrip();
        }

        // Forms Paint event for the HUDs Graphics Panel.
        private void paintHUD(object sender, PaintEventArgs e)
        {
            if (labelHUD.Visible)
            {
                string hudString = $"Cells Alive: {game.CountTotalAlive().ToString()}\n" +
                    $"Generations: {generations}\n" +
                    $"Boundry Mode: {borderMode}\n" +
                    $"Universe Width: {game.Width.ToString()}   Universe Height: {game.Height.ToString()}";

                Font font = new Font("Arial", 12, FontStyle.Bold);
                using (Brush hudBrush = new SolidBrush(Color.DarkViolet))
                {
                    e.Graphics.DrawString(hudString, font, hudBrush, ClientRectangle);
                }
            }
        }

        // GraphicPanels Event fired when the mouse is clicked.
        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // Toggles the Cell state of the clicked cell.
            game.ToggleCell(graphicsPanel1, e);
        }

        #endregion
        //
        //
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
        //
        //
        #region ToolStrip Button Methods

        // Clears the board. Fired when the Clear button on the tool strip is clicked.
        private void clearButton_Click(object sender, EventArgs e)
        {
            // Calls the ClearUniverse method in the Game class. (Sets all cells to dead LifeState)
            game.ClearUniverse();

            // Resets generations to 0.
            generations = 0;

            // Tells the program the graphics panel needs to be repainted.
            graphicsPanel1.Invalidate();
        }

        // Starts and pauses the simulation - Changes state based on game state (Running or Paused)
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

        // Stops the Simulation, and Resets the counts and universe. Fired when the Stop button on the tool strip is clicked.
        private void stopButton_Click(object sender, EventArgs e)
        {
            // Checks if the timer is running.
            if (timer.Enabled)
            {
                // Calls the logic in the game flow buttons method. (Pause and Resume Simulation)
                gameFlowButton_Click(sender, e);
            }

            // Resets generations to 0.
            ResetGenerations();

            // Calls the logic in the clear buttons method. Resets the board to be blank.
            clearButton_Click(sender, e);

            // Tells the program the GraphicsPanel needs to be repainted.
            graphicsPanel1.Invalidate();
        }

        // Manually proceed to the next generation. Fired when the Next button on the tool strip is clicked.
        private void nextButton_Click(object sender, EventArgs e)
        {
            // Check to make sure the timer is not running (Game is in a paused state)
            if (!timer.Enabled)
            {
                // Calls the Next Generation method (Advances 1 generation)
                NextGeneration();
            }
        }

        // Randomizes the board based on the current seed (Shown in the status strip).
        private void currentSeedRandomizeButton_Click(object sender, EventArgs e)
        {
            // Variable holding the Array of the current games universe.
            Cell[,] universeGrid = game.gameBoard.UniverseGrid;

            // Randomly fills the current board according the Games current seed value
            game.gameBoard.RandomFillUniverse(universeGrid, game._seed);

            // Resets Generations to 0
            ResetGenerations();

            // Update Status strips values.
            UpdateStausStrip();

            // Tell the program it needs to repaint the board.
            graphicsPanel1.Invalidate();
        }

        // Randomizes the board based on a new random seed
        private void randomSeedRandomizeButton_Click(object sender, EventArgs e)
        {
            // Variable holding the 2D array of the current universe.
            Cell[,] universe = game.gameBoard.UniverseGrid;

            // Instantiates a new Random Generator for the Random output.
            Random rnd = new Random();

            // Sets current Games seed value according to a random seed.
            game._seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            // Randomly fills the universe with the generated seed.
            game.gameBoard.RandomFillUniverse(universe, game._seed);

            // Resets generations to 0.
            ResetGenerations();

            // Updates the status strip.
            UpdateStausStrip();

            // Tells the program to repaint the board.
            graphicsPanel1.Invalidate();
        }

        // Randomizes the board based on a seed generated by the current time.
        private void timeSeedRandomizeButton_Click(object sender, EventArgs e)
        {
            // Variable holding the 2d Array of the current universe.
            Cell[,] universe = game.gameBoard.UniverseGrid;

            // Instantiates a new Random Generated, Seed is based on the current times milliseconds.
            Random rnd = new Random(DateTime.Now.Millisecond);

            // Sets the current seed to the generated output of the RNG.
            game._seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            // Randomly fills the universe based on the outputted seed from the RNG.
            game.gameBoard.RandomFillUniverse(universe, game._seed);

            // Resets generations to 0.
            ResetGenerations();

            // Updates the status strip.
            UpdateStausStrip();

            // Tell the program to repaint the board.
            graphicsPanel1.Invalidate();
        }

        #endregion ToolStrip Buttons
        //
        //
        #region Randomize Settings Dialog Methods

        private void randomizeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Instantiates a Modal Dialog object
            RandomizeModalDialog RandomizeSettingsDialog = new RandomizeModalDialog();

            RandomizeSettingsDialog.Apply += new ApplyEventHandler(RandomizeSettingsDialog_Apply);

            int seedValue = (int)RandomizeSettingsDialog.numericUpDown1.Value;

            RandomizeSettingsDialog.numericUpDown1.Value = game._seed;

            if (RandomizeSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                seedValue = game._seed;
                ResetGenerations();
                Cell[,] universe = game.gameBoard.UniverseGrid;
                game.gameBoard.RandomFillUniverse(universe, seedValue);
                UpdateStausStrip();
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
        //
        //
        #region Game Options Dialog

        /// <summary>
        /// Fires when the Options Menu item is clicked. (Located inside the Game Settings Menu)
        /// </summary>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Instantiates a new Modal Dialog
            OptionsModalDialog optionsDialog = new OptionsModalDialog();

            // Subscribes to the Apply event for the Options Dialog.
            optionsDialog.Apply += OptionsDialog_Apply;

            // Sets the Timer numeric display to the current timerInterval value.
            optionsDialog.numericUpDownTimer.Value = timerInterval;

            // Sets the Widths numeric display to the current Games Width vale.
            optionsDialog.numericUpDownWidth.Value = game.Width;

            // Sets the Heights numeric display to the current Games Height value.
            optionsDialog.numericUpDownHeight.Value = game.Height;

            // Checks if the Modal Dialog was closed with the OK button.
            if (DialogResult.OK == optionsDialog.ShowDialog())
            {
                // Updates the Timer Interval.
                optionsDialog.TimerInterval = (int)optionsDialog.numericUpDownTimer.Value;

                // Updates the Games Height.
                optionsDialog.Height = (int)optionsDialog.numericUpDownHeight.Value;

                // Updates the Games Width.
                optionsDialog.Width = (int)optionsDialog.numericUpDownWidth.Value;
            }
        }

        /// <summary>
        /// Applies the values received from the user input, passed through the ApplyOptionsEventHandler.
        /// </summary>
        private void OptionsDialog_Apply(object sender, OptionsApplyArgs e)
        {
            // Sets the Games timerInterval to the player input in the Modal Dialog.
            timerInterval = e.TimerInterval;

            // Updates the timer objects Interval.
            timer.Interval = e.TimerInterval;
            
            // Checks if any changes were made to the Width or Height. If not Skips Updating the Games Board.
            if (e.Height != game.Height || e.Width != game.Width)
            {
                // Sets the Games universeGrid based on what the user inputs.
                game.SetBoardSize(e.Width, e.Height);

                // Creates a new universe using the inputs for width and height.
                game.gameBoard = new Universe(e.Width, e.Height);

                // Creates a new scratchpad to match the new universe.
                game.scratchPad = new Universe(e.Width, e.Height);
                
                // Resets the generations to 0.
                ResetGenerations();

                // Updates the status strip.
                UpdateStausStrip();
            }

            // Tells the program to repaint the board.
            graphicsPanel1.Invalidate();
        }

        #endregion Game Options Dialog
        //
        //
        #region View Menu Item Methods

        private void toroidalViewMenuItem_Clicked(object sender, EventArgs e)
        {
            if (toroidalViewMenuItem.Checked)
            {
                finiteViewMenuItem.Checked = false;
                borderMode = BorderMode.Toroidal;
            }

            graphicsPanel1.Invalidate();
        }

        private void finiteViewMenuItem_Clicked(object sender, EventArgs e)
        {
            if (finiteViewMenuItem.Checked)
            {
                toroidalViewMenuItem.Checked = false;
                borderMode = BorderMode.Finite;
            }

            graphicsPanel1.Invalidate();
        }

        private void HUDToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (HUDToolStripMenuItem.Checked)
            {
                labelHUD.Visible = true;
            }
            else
            {
                labelHUD.Visible = false;
            }
        }

        private void adjacentCountToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            this.Apply += game.Game_Apply;

            _showNumbers = adjacentCountToolStripMenuItem.Checked;
            if (Apply != null)
            {
                Apply(this, new ViewApplyArgs(_showNumbers, _showGrid, _isFinite));
            }

            graphicsPanel1.Invalidate();
        }

        #endregion
    }
}