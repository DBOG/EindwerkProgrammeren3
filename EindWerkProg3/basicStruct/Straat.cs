using System;
using System.Collections.Generic;
using System.Text;

namespace EindWerkProg3
{
    [Serializable]
    class Straat
    {
        public int Id { get; set; }
        public string StraatNaam { get; set; }
        public Graaf Graaf { get; set; }



        public Straat(int id, string strNaam, Graaf g)
        {
            Id = id;
            StraatNaam = strNaam;
            Graaf = g;

        }
        public List<Knoop> getKnopen()
        {
            return default;
        }
        public void showStraat()
        {
            Console.WriteLine("Straat ID: " + Id);
            Console.WriteLine("Straat Naam: " + StraatNaam);
            Graaf.showGraaf();
        }
        public double getStraatLengte()
        {
            double lengte = 0;
            foreach(KeyValuePair<Knoop, List<Segment>> segmenten in Graaf.Map)
            {
                foreach(Segment segment in segmenten.Value)
                {
                    for (int i = 0; i < segment.Vertices.Count - 1; i++)
                    {
                        double x1 = segment.Vertices[i].X;
                        double x2 = segment.Vertices[i + 1].X;

                        double y1 = segment.Vertices[i].Y;
                        double y2 = segment.Vertices[i + 1].Y;
                        lengte += Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
                    }
                }
            }

            return lengte;
        }
        
    }
}
