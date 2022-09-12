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
    public delegate void ApplyEventHandler(object sender, RandomApplyArgs e);

    public partial class RandomizeModalDialog : Form
    {
        public event ApplyEventHandler Apply;

        public RandomizeModalDialog()
        {
            InitializeComponent();
            numericUpDown1.Minimum = Int32.MinValue;
            numericUpDown1.Maximum = Int32.MaxValue;
        }

        public int Seed { get; set; }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (Apply != null)
            {
                Apply(this, new RandomApplyArgs((int)this.numericUpDown1.Value));
            }

            
        }

        private void randomizeDialogButton_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            numericUpDown1.Value = rnd.Next(Int32.MaxValue);
        }
    }
}
