using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Player
    {
        public int Supplies { get { return supplies; } set { supplies = value; } }
        private int supplies;

        public Player()
        {
            supplies = 5;
        }

        // 1 supp - ruch 
        //na start 5 supp
        // co 5 sekund niech dodaje 2 supplies
    }
}
