using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspTest.Genetic
{
    public struct City
    {
        public string Name { set; get; }
        public PointF Position { set; get; }
        public double DistanceTo(City? another)
        {
            return Math.Sqrt(Math.Pow(Position.X - another.Position.X, 2) + Math.Pow(Position.Y - another.Position.Y, 2));
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = ((City)obj).Position;
            return t.X == this.Position.X && t.Y == this.Position.Y;
        }
    }
}
