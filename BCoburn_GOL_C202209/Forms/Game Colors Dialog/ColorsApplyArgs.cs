using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCoburn_GOL_C202209
{
    // Delegate Event Handler to Apply All Colors (Game Colors Modal Dialog)
    public delegate void ApplyColorsEventHandler(object sender, ColorsApplyArgs e);

    // Delegate Event Handler to Apply 1 Color (Color Context Menu)
    public delegate void ApplyColorEventHandler(object sender, ColorsApplyArgs e, int componentNumber);

    public class ColorsApplyArgs : EventArgs
    {
        // The Colors to each component to pass on.
        public Color GridColor { get; private set; }
        public Color UniverseColor { get; private set; }
        public Color CellColor { get; private set; }
        public Color HUDColor { get; private set; }

        // Special Property used to Pass 1 Color for Context Menu (Allows 1 Component to be colored at a time)
        public Color AnyColor { get; private set; }

        // Constructor for the Game Color Dialog (Colors All Components at Once)
        public ColorsApplyArgs(Color gridColor, Color universeColor, Color cellColor, Color hudColor)
        {
            GridColor = gridColor;
            UniverseColor = universeColor;
            CellColor = cellColor;
            HUDColor = hudColor;
        }

        // Constructor for the Context Menu Coloring, Allows 1 Component to Be Colored at a time.
        public ColorsApplyArgs(Color anyOneColor)
        {
            AnyColor = anyOneColor;
        }

    }
}
