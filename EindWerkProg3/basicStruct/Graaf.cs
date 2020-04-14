using System;
using System.Collections.Generic;
using System.Text;

namespace EindWerkProg3
{
    [Serializable]
    class Graaf
    {
        public int graafId { get; set; }
        public Dictionary<Knoop, List<Segment>> Map = new Dictionary<Knoop, List<Segment>>();

        public Graaf(int id)
        {
            graafId = id;
        }
        public List<Knoop> getKnopen()
        {
            List<Knoop> returnList = new List<Knoop>();
            foreach(KeyValuePair<Knoop, List<Segment>> knopen in Map)
            {
                returnList.Add(knopen.Key);
            }
            return returnList; ////////////////////////       || return list met alle aangrenzende knopen van de segmenten?????
        }
        public void BuildGraaf(List<Segment> segments)
        {
            List<Segment> tempList = new List<Segment>();
            foreach(Segment segment in segments)
            {
                if (!tempList.Contains(segment))
                {
                    if (Map.ContainsKey(segment.BeginKnoop))
                    {
                        if (!Map[segment.BeginKnoop].Contains(segment))
                            Map[segment.BeginKnoop].Add(segment);
                    }
                    else
                    {
                        List<Segment> list = new List<Segment>();
                        list.Add(segment);
                        Map.Add(segment.BeginKnoop, list);
                    }

                    if (Map.ContainsKey(segment.EindKnoop))
                    {
                        if(!Map[segment.EindKnoop].Contains(segment))
                            Map[segment.EindKnoop].Add(segment);

                    }
                    else
                    {
                        List<Segment> list = new List<Segment>();
                        list.Add(segment);
                        Map.Add(segment.EindKnoop, list);
                    }
                    tempList.Add(segment);
                }
            }
        }
        public void showGraaf()
        {
            Console.WriteLine("Graaf id: " + graafId);
            Console.WriteLine("MapCount: " + Map.Count);
            
        }
    }
}
