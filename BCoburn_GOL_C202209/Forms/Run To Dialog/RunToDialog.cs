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

        // Constructor
        public RunToDialog()
        {
            // Initializes the components from the designer
            InitializeComponent();

            // Sets the starting location for the form
            this.Location = new Point(ActiveForm.Location.X + 239, ActiveForm.Location.Y + 100);
        }
    }
}
