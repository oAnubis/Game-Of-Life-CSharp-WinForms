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
    public partial class OptionsModalDialog : Form
    {
        // Event for handling applying the user inputs
        public event ApplyOptionsEventHandler Apply;

        // Constructor for the Modal Dialog
        public OptionsModalDialog()
        {
            // Initializes the components from the Designer
            InitializeComponent();

            // Sets the starting location of the Dialog
            this.Location = new Point(ActiveForm.Location.X + 239, ActiveForm.Location.Y + 100);
        }

        // Properties to pass on
        public int TimerInterval { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // When the Ok Button is pressed
        private void OkButton_Click(object sender, EventArgs e)
        {
            // Sets the Properties based on the values inputted by the user
            TimerInterval = (int)this.numericUpDownTimer.Value;
            Width = (int)this.numericUpDownWidth.Value;
            Height = (int)this.numericUpDownHeight.Value;

            // Applies the Properties using the OptionsApplyArgs Class
            if (Apply != null)
            {
                Apply(this, new OptionsApplyArgs(TimerInterval, Width, Height));
            }
        }

        
    }
}
