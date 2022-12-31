using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Torpedo.Models
{
    public class Ship
    {
        public List<Vector> Segments { get; set; }
        public int ShipLength { get; }
        public Vector StartSegment { get; }
        public Brush Color { get; set; }
        public Player Owner { get; set; }
        public List<Vector> HitSegments { get; set; }
        public bool IsDestroyed { get; set; }
        public Ship(Vector start, List<Vector> segments, Brush brush, Player owner)
        {
            this.Segments = segments;
            this.ShipLength = Segments.Count;
            this.StartSegment = start;
            this.Color = brush;
            this.Owner = owner;
            HitSegments = new List<Vector>();
            this.IsDestroyed = false;
        }

        public void Hit(Vector segment)
        {
            HitSegments.Add(segment);
        }

        public bool DestroyTest()
        {
            if (HitSegments.Count == ShipLength)
            {
                this.IsDestroyed = true;
                //System.Windows.MessageBox.Show("A SHIP IS DED");
            }
            return this.IsDestroyed;
        }

    }
}
