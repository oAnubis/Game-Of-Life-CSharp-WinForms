using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCoburn_GOL_C202209
{
    public delegate void ApplyOptionsEventHandler(object sender, OptionsApplyArgs e);

    public class OptionsApplyArgs : EventArgs
    {
        public int TimerInterval { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public OptionsApplyArgs(int timerInterval, int width, int height)
        {
            TimerInterval = timerInterval;
            Width = width;
            Height = height;
        }
    }
}
