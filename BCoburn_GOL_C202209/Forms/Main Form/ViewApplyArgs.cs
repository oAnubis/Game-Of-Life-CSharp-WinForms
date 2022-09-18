using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCoburn_GOL_C202209
{
    public delegate void ApplyViewEventHandler(object sender, ViewApplyArgs e);

    public class ViewApplyArgs : EventArgs
    {
        public bool ShowNumbers { get; private set; }

        public bool ShowGrid { get; private set; }

        public bool Finite { get; private set; }

        public ViewApplyArgs(bool showNumbers, bool showGrid, bool finite)
        {
            ShowNumbers = showNumbers;
            ShowGrid = showGrid;
            Finite = finite;
        }
    }
}
