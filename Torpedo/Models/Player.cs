using System;
using System.Collections.Generic;
using System.Text;

namespace Torpedo.Models
{
    class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<Ship> ShipList { get; set; }


        public Player(string name /*List<Ship> shiplist*/)
        {
            this.Name = name;
            this.ShipList = new List<Ship>();
            this.Score = 0;
        }

        public void AddShip(params Ship[] ship)
        {
            for (int i = 0; i < ship.Length; i++)
            {
                this.ShipList.Add(ship[i]);
            }
            
        }
    }

}
