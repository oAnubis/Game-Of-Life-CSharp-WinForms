using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCoburn_GOL_C202209
{
    // Class for each Cell in the Universe 2D Array (Each Rectangle).
    public class Cell
    {
        // Bool value for Cell LifeState (true = alive, false = dead)
        public bool Alive { get; private set; }

        private int _aliveNeighbors;

        public int AliveNeighbors
        {
            get { return _aliveNeighbors; }
            set { _aliveNeighbors = value; }
        }

        // Cell Constructor, Defaults each Cell to dead.
        public Cell()
        {
            Alive = false;
        }

        // Cell Constructor, Sets lifeState based on Parameter passed
        public Cell(bool lifeState)
        {
            Alive = lifeState;
        }

        // Accesses and sets a Cells LifeState (Required because properties set is private)
        public void SetLifeState(bool alive)
        {
            Alive = alive;
        }

        // Returns a Cells current LifeState (Not required because Cells get is public, but helps code to be more clear)
        public bool GetLifeState()
        {
            return Alive;
        }

        public void CalculateNeighbors(int x, int y)
        {
            //this.UniverseGrid[]
        }

    }
}
