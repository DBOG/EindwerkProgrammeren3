using System;
using System.Collections.Generic;
using System.Text;

namespace EindWerkProg3
{
    static class Data
    {
        //raw Data
        public static Dictionary<int, string> straatIdEnStraatNaam = new Dictionary<int, string>();
        public static Dictionary<int, string[]> wrData = new Dictionary<int, string[]>();
        public static Dictionary<int, string[]> wrGemeenteNaam = new Dictionary<int, string[]>();
        public static Dictionary<int, int> wrGemeenteId = new Dictionary<int, int>();//1,2,4,5,8
        public static Dictionary<int, string[]> wrProvincieInfo = new Dictionary<int, string[]>();
        public static List<int> provIds = new List<int>();



        //class Data
        public static Dictionary<int, Knoop> alleKnopen = new Dictionary<int, Knoop>();
        public static Dictionary<int, List<Segment>> alleSegmenten = new Dictionary<int, List<Segment>>();
        public static Dictionary<int, Graaf> alleGrafen = new Dictionary<int, Graaf>();
        public static Dictionary<int, Straat> alleStraten = new Dictionary<int, Straat>();
        public static Dictionary<int, Gemeente> alleGemeentes = new Dictionary<int, Gemeente>();
        public static Dictionary<int, Provincie> alleProvincies = new Dictionary<int, Provincie>();

    }
}
