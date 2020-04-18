using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace EindWerkProg3
{
    class ReadDB
    {
        //tool3

        static string connectionString = "Data Source=adonetserver.database.windows.net;Initial Catalog=adonet;User ID=adminis;Password=";/// ADD PASSWORD HERE!!!!
        public static void RunThirdTool()
        {
            Console.WriteLine("Press 1 : voor alle straat id's in een bepaalde gemeente.");
            Console.WriteLine("Press 2 : voor een straat op te vragen met het straat id.");
            Console.WriteLine("Press 3 : voor een straat op te vragen met straatnaam en gemeentenaam");
            Console.WriteLine("Press 4 : voor alle straten op te vragen in een bepaalde gemeente");
            int.TryParse(Console.ReadLine(), out int anwser);
            switch (anwser)
            {
                case 1:
                    Console.WriteLine("\nGeeft een gemeentenaam in: ");
                    string gemeente = Console.ReadLine();
                    GetStraatIdWithGemeenteNaam(gemeente);
                    break;
                case 2:
                    Console.WriteLine("\nGeef een straat ID in:");
                    int id = int.Parse(Console.ReadLine());
                    GetStraatWithStraatID(id).showStraat();
                    break;
                case 3:
                    Console.WriteLine("\nGeef een gemeentenaam in:");
                    string gemeentenaam = Console.ReadLine();
                    Console.WriteLine("\nGeef een straatnaam in:");
                    string straatnaam = Console.ReadLine();
                    GetStraatWithStraatNaamAndGemeenteNaam(gemeentenaam, straatnaam).showStraat();
                    break;
                case 4:
                    List<Straat> stratenFromGemeente = GetAlleStratenFromGemeente();
                    break;
                default :
                    Console.WriteLine("Incorrect Input");
                    break;
            }


        }
        private static void GetStraatIdWithGemeenteNaam(string gemeente)
        {
            string query = "Select Straat.StraatID as straat from Straat inner join GemeenteID_StraatID  on Straat.StraatID = GemeenteID_StraatID.StraatID inner join Gemeente on GemeenteID_StraatID.GemeenteID = Gemeente.GemeenteID Where GemeenteNaam = @GemeenteNaam";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter gemeenteparam = new SqlParameter();
                gemeenteparam.ParameterName = "@GemeenteNaam";
                gemeenteparam.SqlDbType = SqlDbType.VarChar;
                gemeenteparam.Value = gemeente;
                command.Parameters.Add(gemeenteparam);
                connection.Open();
                Console.WriteLine("\nStraat id's van " + gemeente + ": \n");
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetInt32(0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private static Straat GetStraatWithStraatID(int id)
        {
            string straatnaam ="";
            List<string> vertecies = new List<string>();
            List<int> segmentID = new List<int>();
            List<int> beginKnoopID = new List<int>();
            List<int> eindKnoopID = new List<int>();
            string query = "SELECT Straat.StraatNaam, Segment.Vertecies, Segment.SegmentID, Segment.BeginKnoopID, Segment.EindKnoopID from Straat inner join StraatID_SegmentID on StraatID_SegmentID.StraatID = Straat.StraatID inner join Segment on StraatID_SegmentID.SegmentID = Segment.SegmentID where Straat.StraatID = @straatid";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter straatId = new SqlParameter();
                straatId.ParameterName = "@straatid";
                straatId.SqlDbType = SqlDbType.Int;
                straatId.Value = id;
                command.Parameters.Add(straatId);
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        straatnaam = reader.GetString(0);
                        vertecies.Add(reader.GetString(1));
                        segmentID.Add(reader.GetInt32(2));
                        beginKnoopID.Add(reader.GetInt32(3));
                        eindKnoopID.Add(reader.GetInt32(4));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return BuildStraat(id, straatnaam, vertecies, segmentID, beginKnoopID, eindKnoopID);
        }
        private static Straat GetStraatWithStraatNaamAndGemeenteNaam(string gemeentenaam ,string straatnaam)
        {
            string query = @"SELECT Straat.StraatNaam, Segment.Vertecies, Segment.SegmentID, Segment.BeginKnoopID, Segment.EindKnoopID, Gemeente.GemeenteNaam, Straat.StraatID
                                from Straat
                                inner join StraatID_SegmentID
                                   on StraatID_SegmentID.StraatID = Straat.StraatID
                                inner join Segment
                                   on StraatID_SegmentID.SegmentID = Segment.SegmentID
                                inner join GemeenteID_StraatID
                                   on GemeenteID_StraatID.StraatID = Straat.StraatID
                                inner join Gemeente
                                  on GemeenteID_StraatID.StraatID = Straat.StraatID
                           where Gemeente.GemeenteNaam = @gemeentenaam AND Straat.StraatNaam = @straatnaam";

            List<string> vertecies = new List<string>();
            List<int> segmentID = new List<int>();
            List<int> beginKnoopID = new List<int>();
            List<int> eindKnoopID = new List<int>();
            int straatID = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter gemeentenaamParam = new SqlParameter();
                gemeentenaamParam.ParameterName = "@gemeentenaam";
                gemeentenaamParam.SqlDbType = SqlDbType.VarChar;
                gemeentenaamParam.Value = gemeentenaam;

                SqlParameter straatnaamParam = new SqlParameter();
                straatnaamParam.ParameterName = "@straatnaam";
                straatnaamParam.SqlDbType = SqlDbType.VarChar;
                straatnaamParam.SqlValue = straatnaam;
                command.Parameters.Add(gemeentenaamParam);
                command.Parameters.Add(straatnaamParam);

                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        straatID = reader.GetInt32(6);
                        vertecies.Add(reader.GetString(1));
                        segmentID.Add(reader.GetInt32(2));
                        beginKnoopID.Add(reader.GetInt32(3));
                        eindKnoopID.Add(reader.GetInt32(4));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return BuildStraat(straatID, straatnaam, vertecies, segmentID, beginKnoopID, eindKnoopID);
        }
        private static List<Straat> GetAlleStratenFromGemeente()
        {


            return default;
        }
        private static Straat BuildStraat(int id, string straatnaam, List<string> vertecies,List<int>segmentID, List<int> beginKnoopID, List<int> eindKnoopID)
        {
            List<Segment> segmenten = new List<Segment>();

            for (int i = 0; i < vertecies.Count; i++)
            {
                double x = double.Parse(vertecies[i].Split(" ")[0].Split("|")[0]);
                double y = double.Parse(vertecies[i].Split(" ")[0].Split("|")[1]);
                Knoop beginKnoop = new Knoop(beginKnoopID[i], new Punt(x, y));

                double x2 = double.Parse(vertecies[i].Split(" ")[vertecies[i].Split(" ").Length - 2].Split("|")[0].Trim());
                double y2 = double.Parse(vertecies[i].Split(" ")[vertecies[i].Split(" ").Length - 2].Split("|")[1].Trim());
                Knoop eindKnoop = new Knoop(eindKnoopID[i], new Punt(x, y));

                List<Punt> verteciesSegment = new List<Punt>();
                for (int a = 0; a < vertecies[i].Split(" ").Length - 2; a++)
                {
                    verteciesSegment.Add(new Punt(double.Parse(vertecies[i].Split(" ")[a].Split("|")[0]), double.Parse(vertecies[i].Split(" ")[a].Split("|")[1])));
                }
                segmenten.Add(new Segment(segmentID[i], beginKnoop, eindKnoop, verteciesSegment));
            }
            Graaf graaf = new Graaf(id);
            graaf.BuildGraaf(segmenten);
            Straat straat = new Straat(id, straatnaam, graaf);

            return straat;
        }
    }
}
