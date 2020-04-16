using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace EindWerkProg3
{
    class ReadFiles
    {
        //tool1

        #region FilePaths:

        static string BasePath = "D:\\Hogent\\Programmeren\\Programmeren 3\\LABO 1\\WRdata-master\\";

        static string StratenPath = BasePath + "WRstraatnamen.csv";
        static string wrDataPath = BasePath + "WRdata.csv";
        static string wrGemeenteNaamPath = BasePath + "WRGemeentenaam.csv";
        static string straatIdGemeenteIdPath = BasePath + "StraatnaamID_gemeenteID.csv";
        static string ProvincieInfoPath = BasePath + "ProvincieInfo.csv";
        #endregion

        public static void RunFirstTool()
        {
            ImportData();
            Console.WriteLine("Data Imported..");
            CreateKnopen();
            Console.WriteLine("Knopen Created..");
            CreateSegmenten();
            Console.WriteLine("Segmenten Created..");
            CreateStraten();
            Console.WriteLine("Straten Created..");
            CreateGemeentes();
            Console.WriteLine("Gemeentes Created..");
            CreateProvincies();
            Console.WriteLine("Provincies Created..");
            MakeRapport();
            Console.WriteLine("Rapport Created..");
            //CreateDataFile();
            Testing();
            Console.WriteLine("Data Files Created..");
        }
        public static void ImportData()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string line;
            using (StreamReader r = new StreamReader(wrDataPath))
            {
                while ((line = r.ReadLine()) != null)
                {
                    string[] data;
                    if (!line.StartsWith("w"))
                    {
                        data = line.Split(";");
                        Data.wrData.Add(int.Parse(data[0]), data);
                    }
                }
            }
            using (StreamReader r = new StreamReader(StratenPath))
            {
                while ((line = r.ReadLine()) != null)
                {
                    if (!line.StartsWith("E") && !line.StartsWith("-9"))
                    {
                        Data.straatIdEnStraatNaam.Add(int.Parse(line.Split(";")[0]), line.Split(";")[1]);
                    }
                }
            }
            using (StreamReader r = new StreamReader(wrGemeenteNaamPath))
            {
                while((line = r.ReadLine()) != null)
                {
                    string[] data = line.Split(";");
                    if(!data[0].StartsWith("g") && data[2] == "nl")
                    {
                        Data.wrGemeenteNaam.Add(int.Parse(data[0]), data);
                    }
                }
            }
            using (StreamReader r = new StreamReader(straatIdGemeenteIdPath))
            {
                while((line = r.ReadLine())!= null)
                {
                    string[] data = line.Split(";");
                    if (!data[0].StartsWith("s"))
                    {
                        Data.wrGemeenteId.Add(int.Parse(data[0]), int.Parse(data[1]));
                    }
                }
            }
            using (StreamReader r = new StreamReader(ProvincieInfoPath))
            {
                while((line = r.ReadLine()) != null)
                {
                    string[] data = line.Split(";");
                    if(!data[0].StartsWith("g") && data[2] == "nl")
                    {
                        Data.wrProvincieInfo.Add(int.Parse(data[0]), data);
                    }
                }
            }

        }
        public static void CreateKnopen()
        {
            foreach (KeyValuePair<int, string[]> data in Data.wrData)
            {
                string geos = getBetween(data.Value[1], "LINESTRING (", ")");

                string[] geo = geos.Split(",");
                int beginKnoopId = int.Parse(data.Value[4]);
                int eindKnoopId = int.Parse(data.Value[5]);
                for (int i = 0; i < geo.Length; i++)
                {
                    if(i == 0 && !Data.alleKnopen.ContainsKey(beginKnoopId))
                    {
                        string[] xEnY = geo[0].Split(" ");
                        Data.alleKnopen.Add(beginKnoopId, new Knoop(beginKnoopId, new Punt(double.Parse(xEnY[0]), double.Parse(xEnY[1]))));
                    }
                    else if(i == geo.Length - 1 && !Data.alleKnopen.ContainsKey(eindKnoopId))
                    {
                        string[] xEnY = geo[geo.Length - 1].Split(" ");
                        Data.alleKnopen.Add(eindKnoopId, new Knoop(eindKnoopId, new Punt(double.Parse(xEnY[1]), double.Parse(xEnY[2]))));
                    }
                }
            }
        }
        public static void CreateSegmenten()
        {
            int segmentID;
            string[] geo;
            Knoop beginKnoop;
            Knoop eindKnoop;
            int linkerstraatID;
            int rechterstraatID;

            foreach (KeyValuePair<int, string[]> data in Data.wrData)
            {
                if(int.Parse(data.Value[6]) == -9 || int.Parse(data.Value[7]) == -9) { }
                else
                {

                    segmentID = int.Parse(data.Value[0]);
                    beginKnoop = Data.alleKnopen[int.Parse(data.Value[4])];
                    eindKnoop = Data.alleKnopen[int.Parse(data.Value[5])];
                    linkerstraatID = int.Parse(data.Value[6]);
                    rechterstraatID = int.Parse(data.Value[7]);
                    geo = getBetween(data.Value[1], "LINESTRING (", ")").Split(",");
                    
                    List<Punt> vertices = new List<Punt>();
                    foreach(string s in geo)
                    {
                        string[] xEnY = s.Split(" ");
                        vertices.Add(new Punt(double.Parse(xEnY[xEnY.Length - 2]), double.Parse(xEnY[xEnY.Length - 1])));
                    }
                    Segment seg = new Segment(segmentID, beginKnoop, eindKnoop, vertices);
                    
                   
                    if(linkerstraatID == rechterstraatID)
                    {
                        if (Data.alleSegmenten.ContainsKey(linkerstraatID))
                        {
                            Data.alleSegmenten[linkerstraatID].Add(seg);
                        }
                        else
                        {
                            List<Segment> list = new List<Segment>();
                            list.Add(seg);
                            Data.alleSegmenten.Add(linkerstraatID, list);
                        }
                    }
                    else
                    {
                        List<Segment> list = new List<Segment>();
                        list.Add(seg);

                        if (Data.alleSegmenten.ContainsKey(linkerstraatID))
                            Data.alleSegmenten[linkerstraatID].Add(seg);
                        else
                            Data.alleSegmenten.Add(linkerstraatID, list);


                        if (Data.alleSegmenten.ContainsKey(rechterstraatID))
                            Data.alleSegmenten[rechterstraatID].Add(seg);
                        else
                            Data.alleSegmenten.Add(rechterstraatID, list);
                    }
                    
                }
            }
        }
        public static void CreateStraten()
        {
            int graagID = 0;
            foreach(KeyValuePair<int, List<Segment>> segment in Data.alleSegmenten)
            {
                int straatId = segment.Key;
                Graaf graaf = new Graaf(graagID++);
                graaf.BuildGraaf(segment.Value);
                Straat straat = new Straat(straatId, Data.straatIdEnStraatNaam[segment.Key], graaf);
                Data.alleStraten.Add(straatId, straat);
            }
        }
        public static void CreateGemeentes()
        {
            foreach(KeyValuePair<int, Straat> straat in Data.alleStraten)
            {
                int straatId = straat.Value.Id;
                if (Data.wrGemeenteId.ContainsKey(straatId))
                {
                    int gemeenteId = Data.wrGemeenteId[straatId];
                    if (Data.wrGemeenteNaam.ContainsKey(gemeenteId))
                    {
                        string gemeenteNaam = Data.wrGemeenteNaam[gemeenteId][3];
                        if (Data.alleGemeentes.ContainsKey(gemeenteId))
                            Data.alleGemeentes[gemeenteId].Straten.Add(straat.Value);
                        else
                        {
                            Gemeente gemeente = new Gemeente(gemeenteNaam, gemeenteId);
                            gemeente.Straten.Add(straat.Value);
                            Data.alleGemeentes.Add(gemeenteId, gemeente);
                        }
                    }
                }
            }
        }
        public static void CreateProvincies()
        {
            Data.provIds.Add(1);
            Data.provIds.Add(2);
            Data.provIds.Add(4);
            Data.provIds.Add(5);
            Data.provIds.Add(8);
            foreach(KeyValuePair<int, string[]> provincie in Data.wrProvincieInfo)
            {
                int provincieId = int.Parse(provincie.Value[1]);
                if (Data.provIds.Contains(provincieId))
                {
                    int gemeenteId = int.Parse(provincie.Value[0]);
                    string provNaam = provincie.Value[3];
                    if (Data.alleProvincies.ContainsKey(provincieId))
                    {
                        if(Data.alleGemeentes.ContainsKey(gemeenteId))
                            Data.alleProvincies[provincieId].gemeentes.Add(Data.alleGemeentes[gemeenteId]);
                    }
                    else
                    {
                        Provincie p = new Provincie(provNaam, provincieId);
                        if(Data.alleGemeentes.ContainsKey(gemeenteId))
                            p.gemeentes.Add(Data.alleGemeentes[gemeenteId]);
                        Data.alleProvincies.Add(p.Id, p);
                    }
                }
            }
        }
        public static void MakeRapport()
        {
            string path = "D:\\Hogent\\Programmeren\\Programmeren 3\\LABO 1\\Rapport.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Create(path))
            {
                AddText(fs, $"totaal aantal straten:  {Data.alleStraten.Count}\n");
                AddText(fs, $"aantalStraten per provincie:\n");
                foreach (KeyValuePair<int, Provincie> provincie in Data.alleProvincies)
                {
                    int aantalStraten = 0;
                    foreach (Gemeente g in provincie.Value.gemeentes)
                    {
                        aantalStraten += g.Straten.Count;
                    }
                    AddText(fs, $"     {provincie.Value.ProvincieNaam}: {aantalStraten}\n");
                }
                foreach (KeyValuePair<int, Provincie> provincie in Data.alleProvincies)
                {
                    AddText(fs, $"StraatInfo: {provincie.Value.ProvincieNaam}\n");
                    foreach (Gemeente g in provincie.Value.gemeentes)
                    {
                        AddText(fs, $"     {g.GemeenteNaam}: {g.Straten.Count}, {g.getTotaleLengte()}\n");
                        AddText(fs, $"            Kortste straat: {g.getKorsteStraat().Id}, {g.getKorsteStraat().StraatNaam.TrimEnd()}, {g.getKorsteStraat().getStraatLengte()}\n");
                        AddText(fs, $"            Langste straat: {g.getLangsteStaat().Id}, {g.getLangsteStaat().StraatNaam.TrimEnd()}, {g.getLangsteStaat().getStraatLengte()}\n");

                    }
                }
            }
        }
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
        public static void CreateDataFile()
        {
            string basePath = "D:\\Hogent\\Programmeren\\Programmeren 3\\LABO 1\\";
            IFormatter formatter = new BinaryFormatter();

            //Writing alleKnopen to binaryFile

            if (File.Exists(basePath + "alleKnopen.bin"))
                File.Delete(basePath + "alleKnopen.bin");

            using (FileStream s = File.Create(basePath + "alleKnopen.bin"))
            {
                formatter.Serialize(s, Data.alleKnopen);
            }

            //Writing alleSegmenten to binaryFile

            if (File.Exists(basePath + "alleSegmenten.bin"))
                File.Delete(basePath + "alleSegmenten.bin");

            using (FileStream s = File.Create(basePath + "alleSegmenten.bin"))
            {
                formatter.Serialize(s, Data.alleSegmenten);
            }

            //Writing alleGrafen to binaryFile

            if (File.Exists(basePath + "alleGrafen.bin"))
                File.Delete(basePath + "alleGrafen.bin");

            using (FileStream s = File.Create(basePath + "alleGrafen.bin"))
            {
                formatter.Serialize(s, Data.alleGrafen);
            }

            //Writing alleStraten to binaryFile

            if (File.Exists(basePath + "alleStraten.bin"))
                File.Delete(basePath + "alleStraten.bin");

            using (FileStream s = File.Create(basePath + "alleStraten.bin"))
            {
                formatter.Serialize(s, Data.alleStraten);
            }

            //Writing alleGemeentes to binaryFile

            if (File.Exists(basePath + "alleGemeentes.bin"))
                File.Delete(basePath + "alleGemeentes.bin");

            using (FileStream s = File.Create(basePath + "alleGemeentes.bin"))
            {
                formatter.Serialize(s, Data.alleGemeentes);
            }

            //Writing alleProvincies to binaryFile
            if (File.Exists(basePath + "alleProvincies.bin"))
                File.Delete(basePath + "alleProvincies.bin");

            using (FileStream s = File.Create(basePath + "alleProvincies.bin"))
            {
                formatter.Serialize(s, Data.alleProvincies);
            }
        }
        public static void Testing()/// changing datafile()
        {
            string basePath = "D:\\Hogent\\Programmeren\\Programmeren 3\\LABO 1\\";

            //Writing alleSegmenten to textFile
            if (File.Exists(basePath + "alleSegmenten.txt"))
                File.Delete(basePath + "alleSegmenten.txt");

            using (StreamWriter s = File.CreateText(basePath + "alleSegmenten.txt"))
            {
                foreach(KeyValuePair<int, List<Segment>> seg in Data.alleSegmenten)
                {
                    foreach(Segment segment in seg.Value)
                    {
                        string vertecies = "";
                        foreach(Punt p in segment.Vertices)
                        {
                            vertecies += $"{p.X}|{p.Y}";
                        }

                        s.WriteLine($"{segment.SegmentID} ,{vertecies}");
                    }
                    
                }
            }
            //Writing alleStraten to textFile

            if (File.Exists(basePath + "alleStraten.txt"))
                File.Delete(basePath + "alleStraten.txt");

            using (StreamWriter s = File.CreateText(basePath + "alleStraten.txt"))
            {
                foreach(KeyValuePair<int, Straat> straat in Data.alleStraten)
                {
                    s.WriteLine($"{straat.Value.Id}, {straat.Value.StraatNaam}");
                }
            }

            //Writing alleGemeentes to textFile

            if (File.Exists(basePath + "alleGemeentes.txt"))
                File.Delete(basePath + "alleGemeentes.txt");

            using (StreamWriter s = File.CreateText(basePath + "alleGemeentes.txt"))
            {
                foreach (KeyValuePair<int, Gemeente> gemeente in Data.alleGemeentes)
                {
                    s.WriteLine($"{gemeente.Value.Id}, {gemeente.Value.GemeenteNaam}");
                }
            }

            //Writing alleProvincies to textFile
            if (File.Exists(basePath + "alleProvincies.txt"))
                File.Delete(basePath + "alleProvincies.txt");

            using (StreamWriter s = File.CreateText(basePath + "alleProvincies.txt"))
            {
                foreach(KeyValuePair<int, Provincie> provincie in Data.alleProvincies)
                {
                    s.WriteLine($"{provincie.Value.Id}, {provincie.Value.ProvincieNaam}");
                }
            }




            // CONNECTION FILES //

            //Writing ProvincieID_GemeenteID to textFile
            if (File.Exists(basePath + "ProvincieID_GemeenteID.txt"))
                File.Delete(basePath + "ProvincieID_GemeenteID.txt");

            using (StreamWriter s = File.CreateText(basePath + "ProvincieID_GemeenteID.txt"))
            {
                foreach(KeyValuePair<int, Provincie> provincie in Data.alleProvincies)
                {
                    foreach(Gemeente gemeente in provincie.Value.gemeentes)
                    {
                        s.WriteLine($"{provincie.Value.Id},{gemeente.Id}");
                    }
                }
            }

            //Writing GemeenteID_StraatID to textFile
            if (File.Exists(basePath + "GemeenteID_StraatID.txt"))
                File.Delete(basePath + "GemeenteID_StraatID.txt");

            using (StreamWriter s = File.CreateText(basePath + "GemeenteID_StraatID.txt"))
            {
                foreach (KeyValuePair<int, Gemeente> gemeente in Data.alleGemeentes)
                {
                    foreach (Straat straat in gemeente.Value.Straten)
                    {
                        s.WriteLine($"{gemeente.Value.Id},{straat.Id}");
                    }
                }
            }

            //Writing StraatID_SegmentID to textFile
            if (File.Exists(basePath + "StraatID_SegmentID.txt"))
                File.Delete(basePath + "StraatID_SegmentID.txt");

            using (StreamWriter s = File.CreateText(basePath + "StraatID_SegmentID.txt"))
            {
                foreach(KeyValuePair<int, Straat> straat in Data.alleStraten)
                {
                    foreach(KeyValuePair<Knoop, List<Segment>> segmenten in straat.Value.Graaf.Map)
                    {
                        foreach(Segment segment in segmenten.Value)
                        {
                            s.WriteLine($"{straat.Value.Id}, {segment.SegmentID}");
                        }
                    }
                }
            }

        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
