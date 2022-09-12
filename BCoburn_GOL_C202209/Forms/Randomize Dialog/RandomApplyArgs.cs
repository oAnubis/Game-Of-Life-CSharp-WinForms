using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCoburn_GOL_C202209
{
    public class RandomApplyArgs : EventArgs
    {
        int _seed;

        public int Seed
        {
            get { return _seed; }
            set { _seed = value; }
        }

        public RandomApplyArgs(int seed)
        {
            this.Seed = seed;
        }

        
    }
}
