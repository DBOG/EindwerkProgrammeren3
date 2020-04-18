using System;
using System.Collections.Generic;
using System.Text;

namespace EindWerkProg3
{
    [Serializable]
    class Segment
    {
        public int SegmentID { get; set; }
        public Knoop BeginKnoop { get; set; }
        public Knoop EindKnoop { get; set; }
        public List<Punt> Vertices = new List<Punt>();
        public Segment(int id, Knoop BK, Knoop EK, List<Punt> LP)
        {
            SegmentID = id;
            BeginKnoop = BK;
            EindKnoop = EK;
            Vertices = LP;

        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public string VertecesToString()
        {
            string allePunten = " ";
            foreach (Punt p in Vertices)
            {
                allePunten += $" {p.ToString()}";
            }
            return allePunten;
        }
        public override string ToString()
        {
            return $"\n\tSegmentID: {SegmentID}\n\t\tBeginKnoop: {BeginKnoop.ToString()}\n\t\tEindKnoop: {EindKnoop.ToString()}\n\n\t\tVerteces: {VertecesToString()}";
        }
    }
}
