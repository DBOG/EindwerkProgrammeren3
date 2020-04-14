using System;
using System.Collections.Generic;
using System.Text;

namespace EindWerkProg3
{
    [Serializable]
    class Knoop
    {
        public const int LastKnoopId = 0;
        public int KnoopID { get; set; }
        public Punt punt { get; set; }
        public Knoop(int id, Punt p)
        {
            KnoopID = id;
            punt = p;
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
            return $"KnoopID: {KnoopID} || Punt: {punt.ToString()}";
        }
    }
}
