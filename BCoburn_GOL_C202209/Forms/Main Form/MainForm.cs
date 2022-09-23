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

        // Event Handler to Apply 1 Color on the Context Menu
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

        // Enum for the Border Mode (Finite, Toroidal) 
        public BorderMode BorderMode { get; private set; }

        // Enum for the Display Numbers (Numbers of alive neighbors in each cell)
        public DisplayNumbers DisplayNumbers { get; private set; }

        // Enum for ShowHUD (HUD On/Off)
        public ShowHUD ShowHUD { get; private set; }

        // Enum for ShowGrid (Grid On/Off)
        public ShowGrid ShowGrid { get; private set; }

        #endregion Properties
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
        #region Settings Methods

        // Sets the view menu items checked states to the correct value based on current enum values.
        private void SetViewMenu()
        {
            // Finite is checked if the border mode Enum is set to finite
            finiteViewMenuItem.Checked = BorderMode == BorderMode.Finite;

            // Toroidal is checked if Border mode Enum is set to toroidal
            toroidalViewMenuItem.Checked = BorderMode == BorderMode.Toroidal;

            // Show Numbers is checked if Display Numbers Enum is set to Yes
            showNumbersViewMenuToggle.Checked = DisplayNumbers == DisplayNumbers.Yes;

            // Show Hud is checked if the ShowHUD enum is set to Yes
            showHUDViewMenuToggle.Checked = ShowHUD == ShowHUD.Yes;

            // Show Grid is checked is the ShowGrid Enum is set to Yes
            showGridViewMenuItem.Checked = ShowGrid == ShowGrid.Yes;
        }

        // Sets the Enum values to the default or saved values.
        private void LoadSettings()
        {
            // Each Enum value for this instance is set to the saved settings (Allows persistence between program sessions per user)
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
            // Each user setting is set to the current values of the program
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

            // The user settings are saved
            Settings.Default.Save();
        }

        // Handles Resetting the settings to application defaults
        private void ResetSettingsToDefault()
        {
            // Sets each Enum property to the values of the Application defaults
            BorderMode = Settings.Default.BorderModeDefault;
            DisplayNumbers = Settings.Default.DisplayNumbersDefault;
            ShowHUD = Settings.Default.ShowHUDDefault;
            ShowGrid = Settings.Default.ShowGridDefault;
            TimerInterval = Settings.Default.TimeIntervalDefault;

            // Sets the games setting to the application defaults
            _game.DefaultSettings();

            // Sets the view menu to match the default settings (Now Current)
            SetViewMenu();

            // Repaints the graphics panel
            graphicsPanel1.Invalidate();
        }

        // Reverts to User settings
        private void revertToLastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Loads the user settings
            LoadSettings();

            // Loads the user settings on the Game Class
            _game.RevertSettings();

            // Repaints the graphics panel
            graphicsPanel1.Invalidate();
        }

        // Resets to Application Settings
        private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Resets all setting the the Applications default settings
            ResetSettingsToDefault();
        }

        #endregion
        //
        //
        #region Save/Load/Import Methods

        // Saves the Game
        private void SaveGame()
        {
            // New SaveFile Dialog
            SaveFileDialog dlg = new SaveFileDialog();

            // Saves the file as a .cell file
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";

            // If the Ok button is pressed
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // New StreamWriter to write to the save file
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file.
                // It appends a CRLF for you.
                
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
                            // Else if the universe[x,y] is dead then append '.' (period)
                            // to the row string.
                            currentRow += ".";
                        }
                    }

                    // Once the current row has been read through and the
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }
                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        // Loads a Save File
        private void LoadGame()
        {
            // New Open File Dialog
            OpenFileDialog dlg = new OpenFileDialog();

            // Looks for existing .cells files
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            // If the OK button is pressed
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // New StreamReader to read from the save file
                StreamReader reader = new StreamReader(dlg.FileName);

                // Variables for the Width and Height
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.StartsWith("!"))
                    {
                    }
                    else
                    {
                        // If the row is not a comment then it is a row of cells.
                        // Increment the maxHeight variable for each row read.
                        maxHeight++;
                    }
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }
                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                _game.SetBoardSize(maxWidth, maxHeight);
                _game.GameBoard = new Universe(_game.Width, _game.Height);
                _game.ScratchPad = new Universe(_game.Width, _game.Height);

                // Repaint the graphics panel
                graphicsPanel1.Invalidate();

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Variable to hold the current row number (Y Dimension)
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
                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                            if (row[xPos] == '.')
                            {
                                _game.GameBoard.UniverseGrid[xPos, rowNum].SetLifeState(false);
                            }
                            
                        }
                        // Increments the Row Number (Next Row being written in
                        rowNum++;
                    }
                }
                // Close the file.
                reader.Close();
            }
        }

        // When the import button is clicked
        private void importButton_Click(object sender, EventArgs e)
        {
            // Opens the Patterns folder to allow importing
            ImportPattern();
        }

        private void ImportPattern()
        {
            // New OpenFileDialog
            OpenFileDialog dlg = new OpenFileDialog();

            // Sets a string variable to be the Base Path of this Application
            string initialdir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            // Concentates the Patterns folder path to the Base directory path declared above
            string patterns = initialdir + @"\Resources\Patterns";

            // Sets the initial directory to the Patterns folder I set to be included in the build
            dlg.InitialDirectory = patterns;

            // Looks for .cell files
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Tracks the row number
                int rowNum = 0;

                // Runs until reaching the end of the file
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
                            // If the read in character is '*' the cell is alive
                            if (row[xPos] == '*')
                            {
                                _game.GameBoard.UniverseGrid[xPos, rowNum].SetLifeState(true);
                            }
                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                            if (row[xPos] == '.')
                            {
                                _game.GameBoard.UniverseGrid[xPos, rowNum].SetLifeState(false);
                            }
                        }
                        // Increments the row number (Going to the next line)
                        rowNum++;
                    }
                }
                // Close the file.
                reader.Close();
            }

            // Sets the BorderMode to Toroidal (Most patterns run better in Toroidal, Such as gliders and spaceships, This is mostly a quality of life addition)
            BorderMode = BorderMode.Toroidal;

            // Repaints the graphics panel
            graphicsPanel1.Invalidate();
        }

        #endregion
        //
        //
        #region Forms Main Methods

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
            // Call NextGeneration
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
            // Set Generations to 0
            Generations = 0;
        }

        /// Forms Paint event for the Main GraphicsPanel.
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Runs Game Rules to determine what to output in the Next Generation.
            _game.GameRules();

            // Calls Paintboard from the Game Class (Handles the actual painting logic)
            _game.PaintBoard(graphicsPanel1, e);

            // Updates the status strip to match current values
            UpdateStausStrip();
        }

        // Forms Paint event for the HUDs Graphics Panel.
        private void PaintHud(object sender, PaintEventArgs e)
        {
            // If the HUD is currently visible
            if (labelHUD.Visible)
            {
                // String for the HUD (Uses string interpolation passing in the current values for each HUD Element)
                string hudString = $"Cells Alive: {_game.CountTotalAlive().ToString()}\n" +
                    $"Generations: {Generations}\n" +
                    $"Boundry Mode: {BorderMode}\n" +
                    $"Universe Width: {_game.Width.ToString()}   Universe Height: {_game.Height.ToString()}";

                // New Font For the HUD (TODO: Personal, Make Customizable)
                Font font = new Font("Arial", 12, FontStyle.Bold);

                // Using statement with a new brush, destroys after use (Using statement because it is not looped through so can be destroyed right after use)
                using (Brush hudBrush = new SolidBrush(_game.HUDColor))
                {
                    // Draws the HUD onto the screen
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // When the form is in the process of being closed, save the current settings (Enables persistence between program launches)
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Saves the settings
            SaveSettings();
        }

        #endregion Forms Main Methods
        //
        //
        #region Game Commands Menu Item Controls

        // When the Run Menu Item is pressed in the Game Commands Menu 
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Method Handles Running the Game
            runButton_Click(sender, e); 
        }

        // When the Pause Menu Item is pressed in the Game Commands Menu
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Method Handles Pausing the Game
            pauseButton_Click(sender, e);
        }

        private void runToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunToDialogHandler(sender, e);
        }


        #endregion Game Commands Menu Item Controls
        //
        //
        #region View Menu Item Methods

        // When the HUD menu items checked state is changed (Check state changed instead of clicked to work well with the context menu options)
        private void showHUDViewMenuToggle_CheckStateChanged(object sender, EventArgs e)
        {
            // If the Menu item is Checked
            if (showHUDViewMenuToggle.Checked)
            {
                // Makes the HUD Visible
                labelHUD.Visible = true;

                // Changes the ShowHUD Enum to Yes
                ShowHUD = ShowHUD.Yes;
                
                // Changes the HUD Context menu to be checked
                hUDToolStripMenuItem.Checked = true;
            }
            else
            {
                // Changes the menu item to checked
                hUDToolStripMenuItem.Checked = false;

                // Makes the HUD invisible
                labelHUD.Visible = false;

                // Changes the ShowHUD enum to No
                ShowHUD = ShowHUD.No;
            }
        }

        // When the Alive Neighbors menu items checked state is changed (Check state changed instead of clicked to work well with the context menu options)
        private void showNumbersViewMenuToggle_CheckStateChanged(object sender, EventArgs e)
        {
            // If Alive Neighbors is checked
            if (showNumbersViewMenuToggle.Checked)
            {
                // Changes the DisplayNumbers Enum to Yes
                DisplayNumbers = DisplayNumbers.Yes;

                // Changes the Alive Neighbors context menu checked state to checked
                aliveNeighborsContext.Checked = true;
            }
            else
            {
                // Changes the DisplayNumbers Enum to No
                DisplayNumbers = DisplayNumbers.No;

                // Changes the Alive Neighbors context menu checked state to unchecked
                aliveNeighborsContext.Checked = false;
            }

            // Repaints the graphics panel
            graphicsPanel1.Invalidate();
        }

        // When the Grid menu items checked state is changed (Check state changed instead of clicked to work well with the context menu options)
        private void showGridViewMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            // If the Grid menu item is checked
            if (showGridViewMenuItem.Checked)
            {
                // Changes the ShowGrid enum to Yes
                ShowGrid = ShowGrid.Yes;

                // Changes the ShowGrid Context item to be checked
                showGridContext.Checked = true;
            }
            else
            {
                // Changes the ShowGrid Enum to No
                ShowGrid = ShowGrid.No;

                // Changes the ShowGrid Context item to be unchecked
                showGridContext.Checked = false;
            }

            // Repaints the graphics panel
            graphicsPanel1.Invalidate();
        }

        // When the Toroidal menu item is clicked
        private void toroidalViewMenuItem_Clicked(object sender, EventArgs e)
        {
            // if the Toroidal Menu item is checked
            if (toroidalViewMenuItem.Checked)
            {
                // Changes the finite menu item to be unchecked (Prevents conflict)
                finiteViewMenuItem.Checked = false;

                // Changes the BorderMode enum to be Toroidal
                BorderMode = BorderMode.Toroidal;
            }

            // Repaints the graphics panel
            graphicsPanel1.Invalidate();
        }

        // When the Finite menu item is clicked
        private void finiteViewMenuItem_Clicked(object sender, EventArgs e)
        {
            // If the finite menu item is checked
            if (finiteViewMenuItem.Checked)
            {
                // Changes Toroidal menu item to be unchecked (Prevents conflicts)
                toroidalViewMenuItem.Checked = false;

                // Changes the BorderMode Enum to Finite
                BorderMode = BorderMode.Finite;
            }

            // Repaints the graphics panel
            graphicsPanel1.Invalidate();
        }

        #endregion View Menu Item Methods
        //
        //
        #region ToolStrip Button Methods

        // Listed in the order the appear on the Button Toolstrip

        // When the new game button is pressed, Creates a brand new game, set to current user settings.
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            // Creates a new Universe using the current Width and Height Settings
            _game.GameBoard = new Universe(_game.Width, _game.Height);

            // Resets the game seed to 0 (It's a new game)
            _game.Seed = 0;

            // Resets Generations to 0
            ResetGenerations();

            // If the timer is enabled
            if (_timer.Enabled)
            {
                // Runs the Pause button logic
                pauseButton_Click(sender, e);
            }

            // Repaints the graphics panel
            graphicsPanel1.Invalidate();
        }

        // When the save button is pressed, Saves the game (TODO: Make this a quick save button)
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            // Opens Save Dialog and Saves the Game
            SaveGame();
        }

        // When the open button is pressed, opens a saved file
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            // Opens a Open File Dialog, allowing a saved file to be loaded
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

        // When the Run Button is Clicked
        private void runButton_Click(object sender, EventArgs e)
        {
            // If the timer is NOT running
            if (!_timer.Enabled)
            {
                // Starts the timer
                _timer.Enabled = true;

                // Grey out the Run Button & Menu item
                runButton.Enabled = false;
                runToolStripMenuItem.Enabled = false;

                // Enable the Pause Button & Menu item
                pauseButton.Enabled = true;
                pauseToolStripMenuItem.Enabled = true;

                // Grey out the Next Button & Menu Item (Can't progress by 1 when program is running)
                nextButton.Enabled = false;
                nextToolStripMenuItem.Enabled = false;
            }
        }

        // When the Pause Button is Clicked
        private void pauseButton_Click(object sender, EventArgs e)
        {
            // If the timer is enabled
            if (_timer.Enabled)
            {
                // Stop the timer
                _timer.Stop();

                // Enable the run button & menu items
                runButton.Enabled = true;
                runToolStripMenuItem.Enabled = true;

                // Grey out the pause button & menu items
                pauseButton.Enabled = false;
                pauseToolStripMenuItem.Enabled = false;

                // Enable the Next button & menu items
                nextButton.Enabled = true;
                nextToolStripMenuItem.Enabled = true;
            }
        }

        // When the Stop Button is Clicked
        private void stopButton_Click(object sender, EventArgs e)
        {
            // Checks if the timer is running.
            if (_timer.Enabled)
            {
                // Calls the Pause Button logic
                pauseButton_Click(sender, e);
            }

            // Resets generations to 0.
            ResetGenerations();

            // Calls the logic in the clear buttons method. Resets the board to be blank.
            clearButton_Click(sender, e);

            // Tells the program the GraphicsPanel needs to be repainted.
            graphicsPanel1.Invalidate();
        }

        // When the Next button is Clicked (Manually progress 1 Generation)
        private void nextButton_Click(object sender, EventArgs e)
        {
            // Check to make sure the timer is not running (Game is in a paused state, Button should be greyed out, but checking to be sure)
            if (!_timer.Enabled)
            {
                // Calls the Next Generation method (Advances 1 generation)
                NextGeneration();
            }
        }

        // When the Current Seed Randomize Button is clicked - Randomizes the board based on the current seed (Shown in the status strip).
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

        // When the Random Seed Randomize Button is Clicked - Randomizes the board based on a new random seed
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

        // When the Time Seed Randomize Button is Clicked - Randomizes the board based on a seed generated by the current time.
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

        private void seedSelectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Instantiates a Modal Dialog object
            RandomizeModalDialog randomizeSettingsDialog = new RandomizeModalDialog();

            // Subscribe to the Apply Event
            randomizeSettingsDialog.Apply += RandomizeSettingsDialog_Apply;

            // Sets the seed value for the Dialog.
            int seedValue = (int)randomizeSettingsDialog.numericUpDown1.Value;

            // Sets the value shows for the numeric control
            randomizeSettingsDialog.numericUpDown1.Value = _game.Seed;

            // If the OK Button is Pressed (This needs refactored, it does function now)
            if (randomizeSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                seedValue = _game.Seed;
                // Resets Generations to 0
                ResetGenerations();

                // Variable holding the Main Universe Grid
                Cell[,] universe = _game.GameBoard.UniverseGrid;

                // Randomly fills the grid based on the inputted seed value
                _game.GameBoard.RandomFillUniverse(universe, seedValue);

                // Updates the Status Strip with the new seed value
                UpdateStausStrip();

                // Tells the program to repaint the graphics panel
                graphicsPanel1.Invalidate();
            }
            
            // Clears the resources held by the Dialog
            randomizeSettingsDialog.Dispose();
        }

        private void RandomizeSettingsDialog_Apply(object sender, RandomApplyArgs e)
        {
            // Sets seed to equal the values from the Event
            int seed = e.Seed;

            // Sets the games seed value
            _game.Seed = seed;
        }

        #endregion Randomize Settings Dialog Methods
        //
        //
        #region Game Options Dialog

        // When the Options Menu Item is clicked
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

        
        // Applies the values received from the user input, passed through the ApplyOptionsEventHandler.
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

        // When the Game Colors Menu item is Clicked
        private void gameColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // New GameColorsDialog
            GameColorsDialog gameColorsDialog = new GameColorsDialog();

            // Sets the Properties of the Dialog instance
            gameColorsDialog.GridColor = _game.GridColor;
            gameColorsDialog.UniverseColor = _game.UniverseColor;
            gameColorsDialog.CellColor = _game.CellColor;
            gameColorsDialog.HUDColor = _game.HUDColor;

            // Sets the Colors of the preview squares
            gameColorsDialog.cellColorPreview.BackColor = gameColorsDialog.CellColor;
            gameColorsDialog.universeColorPreview.BackColor = gameColorsDialog.UniverseColor;
            gameColorsDialog.gridColorPreview.BackColor = gameColorsDialog.GridColor;
            gameColorsDialog.HUDColorPreview.BackColor = gameColorsDialog.HUDColor;

            // Subscribes the Apply Event Handler to the Games Apply Colors method (Changes the colors of the game)
            gameColorsDialog.ApplyColors += _game.Game_ApplyColors;

            // If the OK button is pressed
            if (gameColorsDialog.ShowDialog() == DialogResult.OK)
            {
                // Repaint the graphics panel
                graphicsPanel1.Invalidate();
            }
        }

        #endregion Game Colors Dialog
        //
        //
        #region Run To Dialog
        
        // I know this was not in the rubric, however I thought it would be fun to utilize some asynchronous programming
        // I know this may not be the optimal method of completing this task, I would be interested in some feedback around this area if time provides that opportunity.

        // Handles the RunTo Dialog
        private void RunToDialogHandler(object sender, EventArgs e)
        {
            // New RunToDialog
            RunToDialog runToDialog = new RunToDialog();

            // If the OK Button is clicked
            if (runToDialog.ShowDialog() == DialogResult.OK)
            {
                // Sets a variable equal to the player input
                int runTo = (int)runToDialog.generationsNumeric.Value;

                // Passes the input to the method for RunTo
                RunTo(sender, e, runTo);
            }
        }

        // An Asynchronous method to Run to the inputted value (Async to allow the for loop and awaiting the proper amount of time between play and pause)
        private async void RunTo(object sender, EventArgs e, int runTo)
        {
            // Runs the game
            runButton_Click(sender, e);

            // Iterates the amount of times user inputted in the dialog
            for (int i = 0; i < runTo; i++)
            {
                // Waits the length of time equal to the current games Interval in milliseconds before continuing to the next iteration
                await Task.Delay(TimerInterval);
            }

            // Pauses the game
            pauseButton_Click(sender, e);
        }

        #endregion Run To Dialog
        //
        //
        #region Context Menu Methods

        // When the HUD item in the Context menu is clicked
        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggles the Check State of the HUD Menu Item (HUD Menu item runs the logic on a change of Check State)
            showHUDViewMenuToggle.Checked = !showHUDViewMenuToggle.Checked;
        }

        // When the Alive Neighbors item in the context menu is clicked
        private void aliveNeighborsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggles the Check State of the Show Numbers Menu Item (Show Numbers Menu item runs the logic on a change of Check State)
            showNumbersViewMenuToggle.Checked = !showNumbersViewMenuToggle.Checked;
        }

        // When the Grid item in the context menu is clicked
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggles the Check State of the Show Grid Menu Item (ShowGrid Menu item runs the logic on a change of Check State)
            showGridViewMenuItem.Checked = !showGridViewMenuItem.Checked;
        }

        // When the Grid Color item in the context menu is clicked
        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // New Color Dialog
            ColorDialog colorDialog = new ColorDialog();

            // Subcribes ApplyColor to the Game Classes ApplyOneColor method
            ApplyColor += _game.Game_ApplyOneColor;

            // If the OK button is pressed
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (ApplyColor != null)
                {
                    // Send the inputted color over to the Games ApplyOneColor Method (The Integer is the Component Number)
                    ApplyColor(this, new ColorsApplyArgs(colorDialog.Color), 1);
                }
            }

            // Repaint the HUD
            graphicsPanel1.Invalidate();
        }

        // When the Cell Color item in the context menu is clicked
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // New Color Dialog
            ColorDialog colorDialog = new ColorDialog();

            // Subcribes ApplyColor to the Game Classes ApplyOneColor method
            ApplyColor += _game.Game_ApplyOneColor;

            // If the OK button is pressed
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (ApplyColor != null)
                {
                    // Send the inputted color over to the Games ApplyOneColor Method (The Integer is the Component Number)
                    ApplyColor(this, new ColorsApplyArgs(colorDialog.Color), 2);
                }
            }

            // Repaint the HUD
            graphicsPanel1.Invalidate();
        }

        // When the Universe Color item in the context menu is clicked
        private void universeColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // New Color Dialog
            ColorDialog colorDialog = new ColorDialog();

            // Subcribes ApplyColor to the Game Classes ApplyOneColor method
            ApplyColor += _game.Game_ApplyOneColor;

            // If the OK button is pressed
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (ApplyColor != null)
                {
                    // Send the inputted color over to the Games ApplyOneColor Method (The Integer is the Component Number)
                    ApplyColor(this, new ColorsApplyArgs(colorDialog.Color), 3);
                }
            }

            // Repaint the HUD
            graphicsPanel1.Invalidate();
        }

        // When the HUD Color item in the context menu is clicked
        private void hUDColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // New Color Dialog
            ColorDialog colorDialog = new ColorDialog();

            // Subcribes ApplyColor to the Game Classes ApplyOneColor method
            ApplyColor += _game.Game_ApplyOneColor;

            // If the OK button is pressed
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (ApplyColor != null)
                {
                    // Send the inputted color over to the Games ApplyOneColor Method (The Integer is the Component Number)
                    ApplyColor(this, new ColorsApplyArgs(colorDialog.Color), 4);
                }
            }

            // Repaint the HUD
            graphicsPanel1.Invalidate();
        }

        

        #endregion
        //
        //
    }
}