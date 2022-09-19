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

        public GameColorsDialog()
        {
            InitializeComponent();

            this.Location = new Point(ActiveForm.Location.X + 239, ActiveForm.Location.Y + 100);
        }

        private void universeColorButton_Click(object sender, EventArgs e)
        {
            if (universeColorDialog.ShowDialog() == DialogResult.OK)
            {
                universeColorPreview.BackColor = universeColorDialog.Color;

                UniverseColor = universeColorDialog.Color;
            }
        }

        private void cellColorButton_Click(object sender, EventArgs e)
        {
            if (cellColorDialog.ShowDialog() == DialogResult.OK)
            {
                cellColorPreview.BackColor = cellColorDialog.Color;

                CellColor = cellColorDialog.Color;
            }
        }

        private void gridColorButton_Click(object sender, EventArgs e)
        {
            if (gridColorDialog.ShowDialog() == DialogResult.OK)
            {
                gridColorPreview.BackColor = gridColorDialog.Color;

                GridColor = gridColorDialog.Color;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (ApplyColors != null)
            {
                ApplyColors(this, new ColorsApplyArgs(GridColor, UniverseColor, CellColor));
            }
        }
    }
}