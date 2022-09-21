using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    // Delegate Event Handler
    public delegate void ApplyEventHandler(object sender, RandomApplyArgs e);

    public partial class RandomizeModalDialog : Form
    {
        // Apply Event to allow Handler to subscribe to
        public event ApplyEventHandler Apply;

        // Constructor for the Randomize Modal Dialog
        public RandomizeModalDialog()
        {
            // Initializes the Components from the designer
            InitializeComponent();

            // Sets the minimum and maximum values for the numeric input
            numericUpDown1.Minimum = Int32.MinValue;
            numericUpDown1.Maximum = Int32.MaxValue;

            // Sets the starting location of the Dialog
            this.Location = new Point(ActiveForm.Location.X + 239, ActiveForm.Location.Y + 100);
        }

        // Seed to Pass (Not Used, Needs Refactored)
        public int Seed { get; set; }

        // When the Ok Button is clicked
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (Apply != null)
            {
                // Applies the users inputs the game
                Apply(this, new RandomApplyArgs((int)this.numericUpDown1.Value));
            }

            
        }

        // Small convenience feature allowing quick randomizing right from the dialog
        private void randomizeDialogButton_Click(object sender, EventArgs e)
        {
            // New RNG
            Random rnd = new Random();

            // Sets the Numeric Input to a Random Number with a max value equal to the Numeric inputs max allowable value (Int32.MaxValue)
            numericUpDown1.Value = rnd.Next(Int32.MaxValue);
        }
    }
}
