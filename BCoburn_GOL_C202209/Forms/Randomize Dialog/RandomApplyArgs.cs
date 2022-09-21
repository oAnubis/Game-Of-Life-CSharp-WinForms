using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCoburn_GOL_C202209
{
    public class RandomApplyArgs : EventArgs
    {
        // Seed field to pass
        int _seed;

        // Gets and Sets the _seed values
        public int Seed
        {
            get { return _seed; }
            set { _seed = value; }
        }

        // Applies the Seed inputted by the user
        public RandomApplyArgs(int seed)
        {
            // Sets the field _seed to the value inputted
            this.Seed = seed;
        }

        
    }
}
