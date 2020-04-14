using System;
using System.Collections.Generic;
using System.Text;

namespace EindWerkProg3
{
    [Serializable]
    class Gemeente
    {
        public string GemeenteNaam { get; set; }
        public int Id { get; set; }
        public List<Straat> Straten = new List<Straat>();
        public Gemeente(string naam, int id)
        {
            GemeenteNaam = naam;
            Id = id;
        }
        public Straat getLangsteStaat()
        {
            Straat langsteStr = Straten[0];
            foreach(Straat s in Straten)
            {
                if(s.getStraatLengte() > langsteStr.getStraatLengte())
                    langsteStr = s;
            }


            return langsteStr;
        }
        public Straat getKorsteStraat()
        {
            Straat kortsteStr = Straten[0];
            foreach(Straat s in Straten)
            {
                if (s.getStraatLengte() < kortsteStr.getStraatLengte())
                    kortsteStr = s;
            }


            return kortsteStr;
        }
        public double getTotaleLengte()
        {
            double lengte = 0;
            foreach(Straat s in Straten)
            {
                lengte += s.getStraatLengte();
            }
            return lengte;
        }
    }
}
