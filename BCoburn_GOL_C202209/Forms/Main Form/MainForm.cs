using BCoburn_GOL_C202209.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace BCoburn_GOL_C202209
{
    public partial class MainForm : Form
    {
        #region Events

        public event ApplyColorEventHandler ApplyColor;

        #endregion Events

        //
        //

        #region Properties

        // Game object (Holds game logic).
        private Game _game;

        // The Timer class.
        private Timer _timer = new Timer();

        // Timer Interval Property (Speed of each generation, in milliseconds).
        public int TimerInterval { get; private set; }

        // Generation count.
        public int Generations { get; private set; }

        public BorderMode BorderMode { get; private set; }

        public DisplayNumbers DisplayNumbers { get; private set; }

        public ShowHUD ShowHUD { get; private set; }

        public ShowGrid ShowGrid { get; private set; }

        #endregion Properties

        //
        //

        #region Fields

        // Game 1st Start Tracker (Has the simulation been ran since the program was opened).
        private bool _isFirstLaunch = true;

        #endregion Fields

        //
        //

        #region Constructors

        public MainForm()
        {
            // Initializes all the components of the form (Buttons, Labels, MenuItems, etc...)
            InitializeComponent();

            // Initialize a new instance of the Game class.
            _game = new Game(this);

            // Sets the default interval for the timer, in milliseconds.
            TimerInterval = 20;

            // Loads the Application settings
            LoadSettings();

            // Setup the timer
            _timer.Interval = TimerInterval; // milliseconds
            _timer.Tick += Timer_Tick;
            _timer.Enabled = false; // Timer defaults to disabled on program launch.
        }

        #endregion Constructors

        //
        //

        #region Forms Main Methods

        // Sets the view menu items checked states to the correct value based on current enum values.
        private void SetViewMenu()
        {
            finiteViewMenuItem.Checked = BorderMode == BorderMode.Finite;

            toroidalViewMenuItem.Checked = BorderMode == BorderMode.Toroidal;

            showNumbersViewMenuToggle.Checked = DisplayNumbers == DisplayNumbers.Yes;

            showHUDViewMenuToggle.Checked = ShowHUD == ShowHUD.Yes;

            showGridViewMenuItem.Checked = ShowGrid == ShowGrid.Yes;
        }

        // Sets the Enum values to the default or saved values.
        private void LoadSettings()
        {
            BorderMode = Settings.Default.BorderMode;
            DisplayNumbers = Settings.Default.DisplayNumbers;
            ShowHUD = Settings.Default.ShowHUD;
            ShowGrid = Settings.Default.ShowGrid;

            TimerInterval = Settings.Default.TimeInterval;

            // Sets the view menu, the values were set above. Either default or user settings.
            SetViewMenu();
        }

        // Saves the current application settings to the Existing enum values.
        private void SaveSettings()
        {
            Settings.Default.BorderMode = BorderMode;
            Settings.Default.DisplayNumbers = DisplayNumbers;
            Settings.Default.ShowHUD = ShowHUD;
            Settings.Default.ShowGrid = ShowGrid;
            Settings.Default.TimeInterval = TimerInterval;
            Settings.Default.UniverseWidth = _game.Width;
            Settings.Default.UniverseHeight = _game.Height;
            Settings.Default.CellColor = _game.CellColor;
            Settings.Default.UniverseColor = _game.UniverseColor;
            Settings.Default.GridColor = _game.GridColor;
            Settings.Default.HUDColor = _game.HUDColor;
            Settings.Default.Save();
        }

        // Shows the next generation on the GraphicsPanel
        private void NextGeneration()
        {
            // Increment generation count
            Generations++;

            // Fires SwapBoards from the Game Class, Swapping Boards is what makes the next generation actually display.
            _game.SwapBoards();

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
            toolStripStatusLabelSeed.Text = "Current Seed = " + _game.Seed;

            // Updates the Cells Alive label on the Status Strip.
            toolStripStatusLabelAliveCount.Text = "Cells Alive = " + _game.CountTotalAlive().ToString();

            // Updates the Generations label on the Status Strip.
            toolStripStatusLabelGenerations.Text = "Generations = " + Generations.ToString();
        }

        /// Small helper method to reset the generation count to 0
        public void ResetGenerations()
        {
            Generations = 0;
        }

        /// Forms Paint event for the Main GraphicsPanel.
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Runs Game Rules to determine what to output in the Next Generation.
            _game.GameRules();

            _game.PaintBoard(graphicsPanel1, e);

            UpdateStausStrip();
        }

        // Forms Paint event for the HUDs Graphics Panel.
        private void PaintHud(object sender, PaintEventArgs e)
        {
            if (labelHUD.Visible)
            {
                string hudString = $"Cells Alive: {_game.CountTotalAlive().ToString()}\n" +
                    $"Generations: {Generations}\n" +
                    $"Boundry Mode: {BorderMode}\n" +
                    $"Universe Width: {_game.Width.ToString()}   Universe Height: {_game.Height.ToString()}";

                Font font = new Font("Arial", 12, FontStyle.Bold);
                using (Brush hudBrush = new SolidBrush(_game.HUDColor))
                {
                    e.Graphics.DrawString(hudString, font, hudBrush, ClientRectangle);
                }
            }
        }

        // GraphicPanels Event fired when the mouse is clicked.
        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // Toggles the Cell state of the clicked cell.
            _game.ToggleCell(graphicsPanel1, e);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void SaveGame()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file.
                // It appends a CRLF for you.
                writer.WriteLine("!This is my comment.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < _game.Height; y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < _game.Width; x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.

                        if (_game.GameBoard.UniverseGrid[x, y].Alive)
                        {
                            currentRow += "O";
                        }
                        else
                        {
                            currentRow += ".";
                        }

                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                    }

                    // Once the current row has been read through and the
                    // string constructed then write it to the file using WriteLine.

                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void LoadGame()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    if (row.StartsWith("!"))
                    {
                    }
                    else
                    {
                        maxHeight++;
                    }

                    maxWidth = row.Length;

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                _game.SetBoardSize(maxWidth, maxHeight);
                _game.GameBoard = new Universe(_game.Width, _game.Height);
                _game.ScratchPad = new Universe(_game.Width, _game.Height);

                graphicsPanel1.Invalidate();
                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                int rowNum = 0;
                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.StartsWith("!"))
                    {
                    }
                    else
                    {
                        // If the row is not a comment then
                        // it is a row of cells and needs to be iterated through.
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {
                            // If row[xPos] is a 'O' (capital O) then
                            // set the corresponding cell in the universe to alive.
                            if (row[xPos] == 'O')
                            {
                                _game.GameBoard.UniverseGrid[xPos, rowNum].SetLifeState(true);
                            }

                            if (row[xPos] == '.')
                            {
                                _game.GameBoard.UniverseGrid[xPos, rowNum].SetLifeState(false);
                            }
                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                        }
                        rowNum++;
                    }
                }

                // Close the file.
                reader.Close();
            }
        }

        #endregion Forms Main Methods

        //
        //

        #region Game Commands Menu Item Controls

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runButton_Click(sender, e);
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pauseButton_Click(sender, e);
        }

        #endregion Game Commands Menu Item Controls

        //
        //

        #region ToolStrip Button Methods

        // Creates a brand new game, set to default settings.
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            _game.GameBoard = new Universe(_game.Width, _game.Height);

            _game.Seed = 0;

            graphicsPanel1.Invalidate();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveGame();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            LoadGame();
        }

        // Clears the board. Fired when the Clear button on the tool strip is clicked.
        private void clearButton_Click(object sender, EventArgs e)
        {
            // Calls the ClearUniverse method in the Game class. (Sets all cells to dead LifeState)
            _game.ClearUniverse();

            // Resets generations to 0.
            Generations = 0;

            // Tells the program the graphics panel needs to be repainted.
            graphicsPanel1.Invalidate();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            if (!_timer.Enabled)
            {
                _timer.Enabled = true;
                runButton.Enabled = false;
                runToolStripMenuItem.Enabled = false;
                pauseButton.Enabled = true;
                pauseToolStripMenuItem.Enabled = true;
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                runButton.Enabled = true;
                runToolStripMenuItem.Enabled = true;
                pauseButton.Enabled = false;
                pauseToolStripMenuItem.Enabled = false;
            }
        }

        // Stops the Simulation, and Resets the counts and universe. Fired when the Stop button on the tool strip is clicked.
        private void stopButton_Click(object sender, EventArgs e)
        {
            //TODO: Redo Commenting
            // Checks if the timer is running.
            if (_timer.Enabled)
            {
                // Calls the logic in the game flow buttons method. (Pause and Resume Simulation)
                pauseButton_Click(sender, e);
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
            if (!_timer.Enabled)
            {
                // Calls the Next Generation method (Advances 1 generation)
                NextGeneration();
            }
        }

        // Randomizes the board based on the current seed (Shown in the status strip).
        private void currentSeedRandomizeButton_Click(object sender, EventArgs e)
        {
            // Variable holding the Array of the current games universe.
            Cell[,] universeGrid = _game.GameBoard.UniverseGrid;

            // Randomly fills the current board according the Games current seed value
            _game.GameBoard.RandomFillUniverse(universeGrid, _game.Seed);

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
            Cell[,] universe = _game.GameBoard.UniverseGrid;

            // Instantiates a new Random Generator for the Random output.
            Random rnd = new Random();

            // Sets current Games seed value according to a random seed.
            _game.Seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            // Randomly fills the universe with the generated seed.
            _game.GameBoard.RandomFillUniverse(universe, _game.Seed);

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
            Cell[,] universe = _game.GameBoard.UniverseGrid;

            // Instantiates a new Random Generated, Seed is based on the current times milliseconds.
            Random rnd = new Random(DateTime.Now.Millisecond);

            // Sets the current seed to the generated output of the RNG.
            _game.Seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            // Randomly fills the universe based on the outputted seed from the RNG.
            _game.GameBoard.RandomFillUniverse(universe, _game.Seed);

            // Resets generations to 0.
            ResetGenerations();

            // Updates the status strip.
            UpdateStausStrip();

            // Tell the program to repaint the board.
            graphicsPanel1.Invalidate();
        }

        #endregion ToolStrip Button Methods

        //
        //

        #region Randomize Settings Dialog Methods

        private void randomizeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Instantiates a Modal Dialog object
            RandomizeModalDialog randomizeSettingsDialog = new RandomizeModalDialog();

            randomizeSettingsDialog.Apply += new ApplyEventHandler(RandomizeSettingsDialog_Apply);

            int seedValue = (int)randomizeSettingsDialog.numericUpDown1.Value;

            randomizeSettingsDialog.numericUpDown1.Value = _game.Seed;

            if (randomizeSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                seedValue = _game.Seed;
                ResetGenerations();
                Cell[,] universe = _game.GameBoard.UniverseGrid;
                _game.GameBoard.RandomFillUniverse(universe, seedValue);
                UpdateStausStrip();
                graphicsPanel1.Invalidate();
            }
            else
            {
            }

            randomizeSettingsDialog.Dispose();
        }

        private void RandomizeSettingsDialog_Apply(object sender, RandomApplyArgs e)
        {
            int seed = e.Seed;
            _game.Seed = seed;
        }

        #endregion Randomize Settings Dialog Methods

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
            optionsDialog.numericUpDownTimer.Value = TimerInterval;

            // Sets the Widths numeric display to the current Games Width vale.
            optionsDialog.numericUpDownWidth.Value = _game.Width;

            // Sets the Heights numeric display to the current Games Height value.
            optionsDialog.numericUpDownHeight.Value = _game.Height;

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
            TimerInterval = e.TimerInterval;

            // Updates the timer objects Interval.
            _timer.Interval = e.TimerInterval;

            // Checks if any changes were made to the Width or Height. If not Skips Updating the Games Board.
            if (e.Height != _game.Height || e.Width != _game.Width)
            {
                // Sets the Games universeGrid based on what the user inputs.
                _game.SetBoardSize(e.Width, e.Height);

                // Creates a new universe using the inputs for width and height.
                _game.GameBoard = new Universe(e.Width, e.Height);

                // Creates a new scratchpad to match the new universe.
                _game.ScratchPad = new Universe(e.Width, e.Height);

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

        #region Game Colors Dialog

        private void gameColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameColorsDialog gameColorsDialog = new GameColorsDialog();

            gameColorsDialog.GridColor = _game.GridColor;
            gameColorsDialog.UniverseColor = _game.UniverseColor;
            gameColorsDialog.CellColor = _game.CellColor;
            gameColorsDialog.HUDColor = _game.HUDColor;

            gameColorsDialog.cellColorPreview.BackColor = gameColorsDialog.CellColor;
            gameColorsDialog.universeColorPreview.BackColor = gameColorsDialog.UniverseColor;
            gameColorsDialog.gridColorPreview.BackColor = gameColorsDialog.GridColor;
            gameColorsDialog.HUDColorPreview.BackColor = gameColorsDialog.HUDColor;

            gameColorsDialog.ApplyColors += _game.Game_ApplyColors;

            if (gameColorsDialog.ShowDialog() == DialogResult.OK)
            {
                graphicsPanel1.Invalidate();
            }
        }

        #endregion Game Colors Dialog

        //
        //

        #region Run To Dialog

        private void RunToDialogHandler(object sender, EventArgs e)
        {
            RunToDialog runToDialog = new RunToDialog();

            int runTo = 0;

            if (runToDialog.ShowDialog() == DialogResult.OK)
            {
                runTo = (int)runToDialog.generationsNumeric.Value;

                RunTo(sender, e, runTo);
            }
        }

        private async void RunTo(object sender, EventArgs e, int runTo)
        {
            runButton_Click(sender, e);
            for (int i = 0; i < runTo; i++)
            {
                await Task.Delay(TimerInterval);
            }
            pauseButton_Click(sender, e);
        }

        #endregion Run To Dialog

        //
        //

        #region View Menu Item Methods

        private void toroidalViewMenuItem_Clicked(object sender, EventArgs e)
        {
            if (toroidalViewMenuItem.Checked)
            {
                finiteViewMenuItem.Checked = false;
                BorderMode = BorderMode.Toroidal;
            }

            graphicsPanel1.Invalidate();
        }

        private void finiteViewMenuItem_Clicked(object sender, EventArgs e)
        {
            if (finiteViewMenuItem.Checked)
            {
                toroidalViewMenuItem.Checked = false;
                BorderMode = BorderMode.Finite;
            }

            graphicsPanel1.Invalidate();
        }

        private void showNumbersViewMenuToggle_CheckStateChanged(object sender, EventArgs e)
        {
            if (showNumbersViewMenuToggle.Checked)
            {
                DisplayNumbers = DisplayNumbers.Yes;
                aliveNeighborsContext.Checked = true;
            }
            else
            {
                DisplayNumbers = DisplayNumbers.No;
                aliveNeighborsContext.Checked = false;
            }

            graphicsPanel1.Invalidate();
        }

        private void showHUDViewMenuToggle_CheckStateChanged(object sender, EventArgs e)
        {
            if (showHUDViewMenuToggle.Checked)
            {
                labelHUD.Visible = true;
                ShowHUD = ShowHUD.Yes;
                hUDToolStripMenuItem.Checked = true;
            }
            else
            {
                hUDToolStripMenuItem.Checked = false;
                labelHUD.Visible = false;
                ShowHUD = ShowHUD.No;
            }
        }

        private void showGridViewMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (showGridViewMenuItem.Checked)
            {
                ShowGrid = ShowGrid.Yes;
                showGridContext.Checked = true;
            }
            else
            {
                ShowGrid = ShowGrid.No;
                showGridContext.Checked = false;
            }

            graphicsPanel1.Invalidate();
        }

        #endregion View Menu Item Methods

        private void revertToLastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadSettings();

            _game.RevertSettings();

            graphicsPanel1.Invalidate();
        }

        private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetSettingsToDefault();
        }

        private void ResetSettingsToDefault()
        {
            BorderMode = Settings.Default.BorderModeDefault;
            DisplayNumbers = Settings.Default.DisplayNumbersDefault;
            ShowHUD = Settings.Default.ShowHUDDefault;
            ShowGrid = Settings.Default.ShowGridDefault;
            TimerInterval = Settings.Default.TimeIntervalDefault;

            _game.DefaultSettings();
            SetViewMenu();

            graphicsPanel1.Invalidate();
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            ImportPattern();
        }

        private void ImportPattern()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            string initialdir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            string patterns = initialdir + @"\Resources\Patterns";

            dlg.InitialDirectory = patterns;

            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(dlg.FileName);
                int rowNum = 0;
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.StartsWith("!"))
                    {
                    }
                    else
                    {
                        // If the row is not a comment then
                        // it is a row of cells and needs to be iterated through.
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {
                            // If row[xPos] is a 'O' (capital O) then
                            // set the corresponding cell in the universe to alive.
                            if (row[xPos] == '*')
                            {
                                _game.GameBoard.UniverseGrid[xPos, rowNum].SetLifeState(true);
                            }

                            if (row[xPos] == '.')
                            {
                                _game.GameBoard.UniverseGrid[xPos, rowNum].SetLifeState(false);
                            }
                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                        }
                        rowNum++;
                    }
                }
                // Close the file.
                reader.Close();
            }

            graphicsPanel1.Invalidate();
        }

        private void runToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunToDialogHandler(sender, e);
        }

        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showHUDViewMenuToggle.Checked = !showHUDViewMenuToggle.Checked;
        }

        private void aliveNeighborsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showNumbersViewMenuToggle.Checked = !showNumbersViewMenuToggle.Checked;
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGridViewMenuItem.Checked = !showGridViewMenuItem.Checked;
        }

        private void hUDColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            ApplyColor += _game.Game_ApplyOneColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (ApplyColor != null)
                {
                    ApplyColor(this, new ColorsApplyArgs(colorDialog.Color), 4);
                }
            }

            graphicsPanel1.Invalidate();
        }

        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            ApplyColor += _game.Game_ApplyOneColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (ApplyColor != null)
                {
                    ApplyColor(this, new ColorsApplyArgs(colorDialog.Color), 1);
                }
            }

            graphicsPanel1.Invalidate();
        }

        private void universeColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            ApplyColor += _game.Game_ApplyOneColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (ApplyColor != null)
                {
                    ApplyColor(this, new ColorsApplyArgs(colorDialog.Color), 3);
                }
            }

            graphicsPanel1.Invalidate();
        }

        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            ApplyColor += _game.Game_ApplyOneColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (ApplyColor != null)
                {
                    ApplyColor(this, new ColorsApplyArgs(colorDialog.Color), 2);
                }
            }

            graphicsPanel1.Invalidate();
        }
    }
}