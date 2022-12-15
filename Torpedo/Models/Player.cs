using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Torpedo.Models
{
    class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<Ship> ShipList { get; set; }
        public Canvas Canvas { get; set; }
        public List<Vector> AllShipSegments { get; set; }


        public Player(string name, Canvas canvas)
        {
            this.Name = name;
            this.Canvas = canvas;
            this.ShipList = new List<Ship>();
            this.Score = 0;
            this.AllShipSegments = new List<Vector>();
        }

        public void AddShip(params Ship[] ship)
        {
            for (int i = 0; i < ship.Length; i++)
            {
                this.ShipList.Add(ship[i]);
                foreach (var segment in ship[i].Segments)
                {
                    AllShipSegments.Add(segment);
                }
            }
            
        }

    }

}
