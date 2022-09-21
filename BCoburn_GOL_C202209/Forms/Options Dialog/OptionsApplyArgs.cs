using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCoburn_GOL_C202209
{
    // Delegate Event Handler
    public delegate void ApplyOptionsEventHandler(object sender, OptionsApplyArgs e);

    public class OptionsApplyArgs : EventArgs
    {
        // Timer Interval to Pass
        public int TimerInterval { get; private set; }

        // Width to Pass
        public int Width { get; private set; }

        // Height to Pass
        public int Height { get; private set; }

        // Passes the arguments over to the Handler
        public OptionsApplyArgs(int timerInterval, int width, int height)
        {
            // Sets the properties to equal the values passed according to user input
            TimerInterval = timerInterval;
            Width = width;
            Height = height;
        }
    }
}
