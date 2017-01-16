using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public enum Power
    {
        none, rivers, water, climbing
    }
    public class Player
    {
        public int Supplies { get { return supplies; } set { supplies = value; } }
        private int supplies;

        private Power superPower = Power.none;

        public Power SuperPower { get { return superPower; } set { superPower = value; } }

        public Player()
        {
            supplies = 5;
        }

        // 1 supp - ruch 
        //na start 5 supp
        // co 5 sekund niech dodaje 2 supplies
    }
}
