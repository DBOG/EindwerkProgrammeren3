using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace EindWerkProg3
{
    class WriteDB
    {
        //tool2
        static Dictionary<int, Knoop> alleKnopen;
        static Dictionary<int, List<Segment>> alleSegmenten;
        static Dictionary<int, Graaf> alleGrafen;
        static Dictionary<int, Straat> alleStraten;
        static Dictionary<int, Gemeente> alleGemeentes;
        static Dictionary<int, Provincie> alleProvincies;

        static string connectionString = "Data Source=adonetserver.database.windows.net;Initial Catalog=adonet;User ID=adminis;Password=";/// ADD PASSWORD HERE!!!!
        public static void RunSecondTool()
        {
            ReadBinaryFiles();
            Console.WriteLine("ReadFiles Completed..");
            ClearDB();
            Console.WriteLine("Dropped All database tables");
            CreateDBTables();
            Console.WriteLine("Created All database tables");
            WriteProvincieToDataBase();
            Console.WriteLine("Provincies added to DB");
            WriteGemeentesToDataBase();
            Console.WriteLine("Gemeentes added to DB");
            WriteProvincieID_GemeenteIDToDataBase();
            Console.WriteLine("Provincie ID and Gemeente ID added to DB");
            WriteStraatToDataBase();
            Console.WriteLine("Straat added to DB");
            WriteGemeenteID_StraatIDToDataBase();
            Console.WriteLine("Gemeente ID and Straat ID added to DB");
        }
        private static void ReadBinaryFiles()
        {
            string basePath = "D:\\Hogent\\Programmeren\\Programmeren 3\\LABO 1\\";
            IFormatter formatter = new BinaryFormatter();

            //Reading and deserializing alleKnopen.bin
            using (FileStream s = File.OpenRead(basePath + "alleKnopen.bin"))
            {
                alleKnopen = (Dictionary<int, Knoop>)formatter.Deserialize(s);
            }
            //Reading and deserializing alleSegmenten.bin
            using (FileStream s = File.OpenRead(basePath + "alleSegmenten.bin"))
            {
                alleSegmenten = (Dictionary<int, List<Segment>>)formatter.Deserialize(s);
            }
            //Reading and deserializing alleGrafen.bin
            using (FileStream s = File.OpenRead(basePath + "alleGrafen.bin"))
            {
                alleGrafen = (Dictionary<int, Graaf>)formatter.Deserialize(s);
            }
            //Reading and deserializing alleStraten.bin
            using (FileStream s = File.OpenRead(basePath + "alleStraten.bin"))
            {
                alleStraten = (Dictionary<int, Straat>)formatter.Deserialize(s);
            }
            //Reading and deserializing alleGemeentes.bin
            using (FileStream s = File.OpenRead(basePath + "alleGemeentes.bin"))
            {
                alleGemeentes = (Dictionary<int, Gemeente>)formatter.Deserialize(s);
            }
            //Reading and deserializing alleProvincies.bin
            using (FileStream s = File.OpenRead(basePath + "alleProvincies.bin"))
            {
                alleProvincies = (Dictionary<int, Provincie>)formatter.Deserialize(s);
            }
        }
        private static void ClearDB()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand(@"DROP TABLE dbo.ProvincieID_GemeenteID;
                                                                 DROP TABLE dbo.GemeenteID_StraatID;
                                                                 DROP TABLE dbo.Straat_Segment
                                                                 DROP TABLE dbo.Provincie;
                                                                 Drop TABLE dbo.Gemeente;
                                                                 DROP TABLE dbo.Segment;
                                                                 DROP TABLE dbo.Straat;", con))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private static void CreateDBTables()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    
                    using (SqlCommand command = new SqlCommand(@"CREATE TABLE [Provincie] (
	                                                             ProvincieID integer NOT NULL,
	                                                             ProvincieNaam varchar(255) NOT NULL,
                                                                 CONSTRAINT [PK_PROVINCIE] PRIMARY KEY CLUSTERED
                                                                 ([ProvincieID] ASC) WITH (IGNORE_DUP_KEY = OFF)
                                                                 )
                                                                 
                                                                 CREATE TABLE [Gemeente] (
	                                                             GemeenteID integer NOT NULL,
	                                                             GemeenteNaam varchar(255) NOT NULL,
                                                                 CONSTRAINT [PK_GEMEENTE] PRIMARY KEY CLUSTERED
                                                                 ([GemeenteID] ASC) WITH (IGNORE_DUP_KEY = OFF)
                                                                 )
                                                                 
                                                                 CREATE TABLE [ProvincieID_GemeenteID] (
	                                                             ProvincieID integer NOT NULL,
	                                                             GemeenteID integer NOT NULL
                                                                 )
                                                                 
                                                                 CREATE TABLE [Straat] (
	                                                             StraatID integer NOT NULL,
	                                                             StraatNaam varchar(255) NOT NULL,
	                                                             GraafID integer NOT NULL,
                                                                 CONSTRAINT [PK_STRAAT] PRIMARY KEY CLUSTERED
                                                                 ( [StraatID] ASC ) WITH (IGNORE_DUP_KEY = OFF)
                                                                 )
                                                                 
                                                                 CREATE TABLE [GemeenteID_StraatID] (
	                                                             GemeenteID integer NOT NULL,
	                                                             StraatID integer NOT NULL
                                                                 )
                                                                 
                                                                 CREATE TABLE [Segment] (
	                                                             SegmentID integer NOT NULL,
	                                                             Vertecies varchar(600) NOT NULL,
                                                                 CONSTRAINT [PK_SEGMENT] PRIMARY KEY CLUSTERED
                                                                 ( [SegmentID] ASC ) WITH (IGNORE_DUP_KEY = OFF)
                                                                 )
                                                                 
                                                                 CREATE TABLE [Straat_Segment] (
	                                                                StraatID integer NOT NULL,
	                                                                SegmentID integer NOT NULL
                                                                 )
                                                                 


                                                                 ALTER TABLE [ProvincieID_GemeenteID] WITH CHECK ADD CONSTRAINT [ProvincieID_GemeenteID_fk0] FOREIGN KEY ([ProvincieID]) REFERENCES [Provincie]([ProvincieID])
                                                                 ON UPDATE CASCADE
                                                                 
                                                                 ALTER TABLE [ProvincieID_GemeenteID] CHECK CONSTRAINT [ProvincieID_GemeenteID_fk0]
                                                                 
                                                                 ALTER TABLE [ProvincieID_GemeenteID] WITH CHECK ADD CONSTRAINT [ProvincieID_GemeenteID_fk1] FOREIGN KEY ([GemeenteID]) REFERENCES [Gemeente]([GemeenteID])
                                                                 ON UPDATE CASCADE
                                                                 
                                                                 ALTER TABLE [ProvincieID_GemeenteID] CHECK CONSTRAINT [ProvincieID_GemeenteID_fk1]
                                                                 

                                                                 ALTER TABLE [GemeenteID_StraatID] WITH CHECK ADD CONSTRAINT [GemeenteID_StraatID_fk0] FOREIGN KEY ([GemeenteID]) REFERENCES [Gemeente]([GemeenteID])
                                                                 ON UPDATE CASCADE
                                                                 
                                                                 ALTER TABLE [GemeenteID_StraatID] CHECK CONSTRAINT [GemeenteID_StraatID_fk0]
                                                                 
                                                                 ALTER TABLE [GemeenteID_StraatID] WITH CHECK ADD CONSTRAINT [GemeenteID_StraatID_fk1] FOREIGN KEY ([StraatID]) REFERENCES [Straat]([StraatID])
                                                                 ON UPDATE CASCADE
                                                                 
                                                                 ALTER TABLE [GemeenteID_StraatID] CHECK CONSTRAINT [GemeenteID_StraatID_fk1]
                                                                 
 

                                                                 ALTER TABLE [Straat_Segment] WITH CHECK ADD CONSTRAINT [Straat_Segment_fk0] FOREIGN KEY ([StraatID]) REFERENCES [Straat]([StraatID])
                                                                 ON UPDATE CASCADE
                                                                 
                                                                 ALTER TABLE [Straat_Segment] CHECK CONSTRAINT [Straat_Segment_fk0]
                                                                 
                                                                 ALTER TABLE [Straat_Segment] WITH CHECK ADD CONSTRAINT [Straat_Segment_fk1] FOREIGN KEY ([SegmentID]) REFERENCES [Segment]([SegmentID])
                                                                 ON UPDATE CASCADE
                                                                 
                                                                 ALTER TABLE [Straat_Segment] CHECK CONSTRAINT [Straat_Segment_fk1] ", con))
                    {
                        command.ExecuteNonQuery(); 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private static void WriteProvincieToDataBase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO dbo.Provincie (ProvincieID, ProvincieNaam) VALUES( @ProvincieID, @ProvincieNaam)";
                foreach(KeyValuePair<int, Provincie> provincie in alleProvincies)
                {

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            connection.Open();
                            command.Parameters.Add(new SqlParameter("@ProvincieID", SqlDbType.Int));
                            command.Parameters.Add(new SqlParameter("@ProvincieNaam", SqlDbType.NVarChar));
                            command.CommandText = query;
                            command.Parameters["@ProvincieID"].Value = provincie.Value.Id;
                            command.Parameters["@ProvincieNaam"].Value = provincie.Value.ProvincieNaam;
                            command.ExecuteNonQuery();
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
            }
        }
        private static void WriteGemeentesToDataBase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO dbo.Gemeente (GemeenteID, GemeenteNaam) VALUES( @GemeenteID, @GemeenteNaam)";
                foreach (KeyValuePair<int, Gemeente> gemeente in alleGemeentes)
                {

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            connection.Open();
                            command.Parameters.Add(new SqlParameter("@GemeenteID", SqlDbType.Int));
                            command.Parameters.Add(new SqlParameter("@GemeenteNaam", SqlDbType.NVarChar));
                            command.CommandText = query;
                            command.Parameters["@GemeenteID"].Value = gemeente.Value.Id;
                            command.Parameters["@GemeenteNaam"].Value = gemeente.Value.GemeenteNaam;
                            command.ExecuteNonQuery();
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
            }
        }
        private static void WriteProvincieID_GemeenteIDToDataBase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO dbo.ProvincieID_GemeenteID (ProvincieID, GemeenteID) VALUES( @ProvincieID, @GemeenteID)";
                foreach (KeyValuePair<int, Provincie> Provincie in alleProvincies)
                {
                    foreach(Gemeente gemeente in Provincie.Value.gemeentes)
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            try
                            {
                                connection.Open();
                                command.Parameters.Add(new SqlParameter("@ProvincieID", SqlDbType.Int));
                                command.Parameters.Add(new SqlParameter("@GemeenteID", SqlDbType.Int));
                                command.CommandText = query;
                                command.Parameters["@ProvincieID"].Value = Provincie.Value.Id;
                                command.Parameters["@GemeenteID"].Value = gemeente.Id;
                                command.ExecuteNonQuery();
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
                }
            }
        }
        private static void WriteStraatToDataBase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO dbo.Straat (StraatID, StraatNaam, GraafID) VALUES( @StraatID, @StraatNaam, @GraafID)";
                foreach(KeyValuePair<int, Straat> straat in alleStraten)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        try
                        {
                                connection.Open();
                                command.Parameters.Add(new SqlParameter("@StraatID", SqlDbType.Int));
                                command.Parameters.Add(new SqlParameter("@StraatNaam", SqlDbType.VarChar));
                                command.Parameters.Add(new SqlParameter("@GraafID", SqlDbType.Int));
                                command.CommandText = query;
                                command.Parameters["@StraatID"].Value = straat.Value.Id;
                                command.Parameters["@StraatNaam"].Value = straat.Value.StraatNaam;
                                command.Parameters["@GraafID"].Value = straat.Value.Graaf.graafId;
                                command.ExecuteNonQuery();
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
            }
        }
        private static void WriteGemeenteID_StraatIDToDataBase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO dbo.GemeenteID_StraatID (GemeenteID, StraatID) VALUES( @GemeenteID, @StraatID)";
                foreach (KeyValuePair<int, Gemeente> gemeente in alleGemeentes)
                {
                    foreach (Straat straat in gemeente.Value.Straten)
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            try
                            {
                                connection.Open();
                                command.Parameters.Add(new SqlParameter("@GemeenteID", SqlDbType.Int));
                                command.Parameters.Add(new SqlParameter("@StraatID", SqlDbType.Int));
                                command.CommandText = query;
                                command.Parameters["@GemeenteID"].Value = gemeente.Value.Id;
                                command.Parameters["@StraatID"].Value = straat.Id;
                                command.ExecuteNonQuery();
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
                }
            }
        }
        private static void WriteSegmentToDataBase()
        {

        }
        private static void WriteStraatID_SegmentIDToDataBase()
        {

        }
    }
}
