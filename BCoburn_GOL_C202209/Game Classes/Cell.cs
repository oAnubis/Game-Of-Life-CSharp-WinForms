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
        // bool value, holds an individual cells state (true = alive, false = dead)
        public bool Alive { get; private set; }

        // int value, holds the amount of Neighbors whose state is set to true (alive)
        public int AliveNeighbors { get; set; }

        // Cell Constructor, Defaults each Cell to dead. (Not called in current version, provides safe fallback for future changes)
        public Cell()
        {
            Alive = false;
        }

        // Cell Constructor, Sets lifeState based on the bool value parameter passed into it.
        public Cell(bool lifeState)
        {
            Alive = lifeState;
        }

        // Accesses and sets a Cells LifeState (Required because setter is private)
        public void SetLifeState(bool alive)
        {
            Alive = alive;
        }
    }
}
