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
    public partial class RunToDialog : Form
    {
        public RunToDialog()
        {
            InitializeComponent();

            this.Location = new Point(ActiveForm.Location.X + 239, ActiveForm.Location.Y + 100);
        }
    }
}
