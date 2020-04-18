using System;
using System.Collections.Generic;
using System.Text;

namespace EindWerkProg3
{
    [Serializable]
    class Punt
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Punt(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"\n\t\t\tX: {X}|| Y: {Y}";
        }
    }
}
