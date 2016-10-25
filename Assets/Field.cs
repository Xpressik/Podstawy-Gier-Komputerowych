using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Field
    {
        public Castle camp;
        public int supply;
        public int contraband; // waluta
        public int garrison;

        public Player owner;

        public Field(Castle camp, int supply, int contraband, int garrison, Player owner)
        {
            this.camp = camp;
            this.supply = supply;
            this.contraband = contraband;
            this.garrison = garrison;
            this.owner = owner;
        }
    }
}
