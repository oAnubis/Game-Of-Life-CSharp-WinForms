using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCoburn_GOL_C202209
{
    public delegate void ApplyColorsEventHandler(object sender, ColorsApplyArgs e);

    public delegate void ApplyColorEventHandler(object sender, ColorsApplyArgs e, int componentNumber);

    public class ColorsApplyArgs : EventArgs
    {
        public Color GridColor { get; private set; }

        public Color UniverseColor { get; private set; }

        public Color CellColor { get; private set; }

        public Color HUDColor { get; private set; }

        public Color AnyColor { get; private set; }

        public ColorsApplyArgs(Color gridColor, Color universeColor, Color cellColor, Color hudColor)
        {
            GridColor = gridColor;
            UniverseColor = universeColor;
            CellColor = cellColor;
            HUDColor = hudColor;
        }

        public ColorsApplyArgs(Color anyOneColor)
        {
            AnyColor = anyOneColor;
        }

    }
}
