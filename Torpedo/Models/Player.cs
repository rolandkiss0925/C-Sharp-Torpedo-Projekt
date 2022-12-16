﻿using System;
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

        public void AddShip(params Ship[] ships)
        {
            for (int i = 0; i < ships.Length; i++)
            {
                this.ShipList.Add(ships[i]);
                foreach (var segment in ships[i].Segments)
                {
                    AllShipSegments.Add(segment);
                }
            }
        }

        public List<Ship> RemoveAllShip()
        {
            List<Ship> returnList = this.ShipList;

            this.AllShipSegments.Clear();
            this.ShipList.Clear();

            return returnList;
        }
    }

}
