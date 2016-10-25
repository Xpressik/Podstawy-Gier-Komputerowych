using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Game
    {
        public List<Player> listOfPlayers;

        public Game()
        {
            World world = new World();
            CreatePlayers(3);
        }

        public void CreatePlayers(int number)
        {
            for (int i = 0; i < number; i++)
            {
                string name = "";
                string tribe = "";
                Player player = new Player(name, tribe);
                listOfPlayers.Add(player);
            }
        }
    }
}
