using System;
using System.Drawing;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    public partial class GameColorsDialog : Form
    {
        public event ApplyColorsEventHandler ApplyColors;

        public Color GridColor { get; set; }

        public Color UniverseColor { get; set; }

        public Color CellColor { get; set; }

        public Color HUDColor { get; set; }

        public GameColorsDialog()
        {
            InitializeComponent();

            this.Location = new Point(ActiveForm.Location.X + 239, ActiveForm.Location.Y + 100);
        }

        private void universeColorButton_Click(object sender, EventArgs e)
        {
            
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                universeColorPreview.BackColor = colorDialog.Color;

                UniverseColor = colorDialog.Color;
            }
        }

        private void cellColorButton_Click(object sender, EventArgs e)
        {
            
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                cellColorPreview.BackColor = colorDialog.Color;

                CellColor = colorDialog.Color;
            }
        }

        private void gridColorButton_Click(object sender, EventArgs e)
        {
            
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                gridColorPreview.BackColor = colorDialog.Color;

                GridColor = colorDialog.Color;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (ApplyColors != null)
            {
                ApplyColors(this, new ColorsApplyArgs(GridColor, UniverseColor, CellColor, HUDColor));
            }
        }

        private void HUDColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                HUDColorPreview.BackColor = colorDialog.Color;

                HUDColor = colorDialog.Color;
            }
        }
    }
}