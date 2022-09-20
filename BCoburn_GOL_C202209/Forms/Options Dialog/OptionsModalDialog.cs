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
        public event ApplyOptionsEventHandler Apply;

        

        public OptionsModalDialog()
        {
            InitializeComponent();

            this.Location = new Point(ActiveForm.Location.X + 239, ActiveForm.Location.Y + 100);
        }

        public int TimerInterval { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        private void OkButton_Click(object sender, EventArgs e)
        {
            TimerInterval = (int)this.numericUpDownTimer.Value;
            Width = (int)this.numericUpDownWidth.Value;
            Height = (int)this.numericUpDownHeight.Value;
            if (Apply != null)
            {
                Apply(this, new OptionsApplyArgs(TimerInterval, Width, Height));
            }
        }

        
    }
}
