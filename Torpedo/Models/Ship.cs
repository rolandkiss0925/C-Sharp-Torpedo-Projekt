using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Torpedo.Models
{
    class Ship
    {
        public List<Vector> Segments { get; set; }
        public int ShipLength { get; }
        public Vector StartSegment { get; }
        public Brush Color { get; set; }
        public Player Owner { get; set; }
        public Ship(Vector start, List<Vector> segments, Brush brush, Player owner)
        {
            this.Segments = segments;
            this.ShipLength = Segments.Count;
            this.StartSegment = start;
            this.Color = brush;
            this.Owner = owner;
        }


    }
}
