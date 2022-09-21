using System;
using System.Drawing;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    public partial class GameColorsDialog : Form
    {
        // Event Handler to Apply the Colors after the OK button is pressed on the form.
        public event ApplyColorsEventHandler ApplyColors;

        // Colors of the Game Dialog
        public Color GridColor { get; set; }
        public Color UniverseColor { get; set; }
        public Color CellColor { get; set; }
        public Color HUDColor { get; set; }

        // Constructor for the GameColorDialog
        public GameColorsDialog()
        {
            // Initialized the Components of the form from the designer.
            InitializeComponent();

            // Sets the default location of the Dialog (I found this default location to better work with the default location of the Color Selector Menu)
            // These numbers were obtained by putting a Point locator on click, and outputting that point as a string in a message box.
            this.Location = new Point(ActiveForm.Location.X + 239, ActiveForm.Location.Y + 100);
        }

        // Handles clicking on the Universe Color Button
        private void universeColorButton_Click(object sender, EventArgs e)
        {
            // If the Ok Button is pressed
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Sets the Preview square color in the dialog.
                universeColorPreview.BackColor = colorDialog.Color;

                // Sets this instances Universe Color Property.
                UniverseColor = colorDialog.Color;
            }
        }

        // Handles clicking on the Cell Color Button
        private void cellColorButton_Click(object sender, EventArgs e)
        {
            // If the Ok Button is pressed
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Sets the Preview square color in the dialog
                cellColorPreview.BackColor = colorDialog.Color;

                // Sets this instances Cell Color Property
                CellColor = colorDialog.Color;
            }
        }

        // Handles clicking on the Grid Color Button
        private void gridColorButton_Click(object sender, EventArgs e)
        {
            // If the OK Button is Pressed
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Sets the Preview square color in the dialog
                gridColorPreview.BackColor = colorDialog.Color;

                // Sets this instances Grid Color Property
                GridColor = colorDialog.Color;
            }
        }

        // Handles Clicking on the HUD Color Button
        private void HUDColorButton_Click(object sender, EventArgs e)
        {
            // If the Ok Button is pressed
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Sets the Preview square color in the dialog
                HUDColorPreview.BackColor = colorDialog.Color;

                // Sets this instances HUD Color Property
                HUDColor = colorDialog.Color;
            }
        }

        // Handles clicking on the OK Button
        private void OKButton_Click(object sender, EventArgs e)
        {
            // If Something is able to be passed
            if (ApplyColors != null)
            {
                // Applys the Color arguments which are passed to the method to change the colors in game.
                ApplyColors(this, new ColorsApplyArgs(GridColor, UniverseColor, CellColor, HUDColor));
            }
        }

        
    }
}