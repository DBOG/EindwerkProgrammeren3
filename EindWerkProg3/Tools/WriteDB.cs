using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace EindWerkProg3
{
    class WriteDB
    {
        //tool2
        static DataSet dataset;

        static string connectionString = "Data Source=adonetserver.database.windows.net;Initial Catalog=adonet;User ID=adminis;Password=";/// ADD PASSWORD HERE!!!!
        public static void RunSecondTool()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            ClearDB();
            Console.WriteLine($"Dropped All database tables    <{stopwatch.ElapsedMilliseconds / 1000.0}>"); stopwatch.Reset();

            stopwatch.Start();
            CreateDBTables();
            Console.WriteLine($"Created All database tables    <{stopwatch.ElapsedMilliseconds / 1000.0}>"); stopwatch.Reset();

            stopwatch.Start();
            CreateDataSet();
            Console.WriteLine($"Created DataSet    <{stopwatch.ElapsedMilliseconds / 1000.0}>"); stopwatch.Reset();

            stopwatch.Start();
            ImportTxtFilesAndPopulateDataSet();
            Console.WriteLine($"Import TxtFiles And Populate DataSet    <{stopwatch.ElapsedMilliseconds / 1000.0}>"); stopwatch.Reset();

            stopwatch.Start();
            BulkCopyDataSet();
            Console.WriteLine($"BulkCopy dataset to DB    <{stopwatch.ElapsedMilliseconds / 1000.0}>"); stopwatch.Reset();


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
                                                                 DROP TABLE dbo.StraatID_SegmentID;
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
	                                                             StraatNaam varchar(255) NOT NULL
                                                                 CONSTRAINT [PK_STRAAT] PRIMARY KEY CLUSTERED
                                                                 ( [StraatID] ASC ) WITH (IGNORE_DUP_KEY = OFF)
                                                                 )
                                                                 
                                                                 CREATE TABLE [GemeenteID_StraatID] (
	                                                             GemeenteID integer NOT NULL,
	                                                             StraatID integer NOT NULL
                                                                 )
                                                                 
                                                                 CREATE TABLE [Segment] (
	                                                             SegmentID integer NOT NULL,
	                                                             Vertecies varchar(max) NOT NULL,
                                                                 BeginKnoopID integer NOT NULL,
                                                                 EindKnoopID integer NOT NULL,
                                                                 CONSTRAINT [PK_SEGMENT] PRIMARY KEY CLUSTERED
                                                                 ( [SegmentID] ASC ) WITH (IGNORE_DUP_KEY = OFF)
                                                                 )
                                                                 
                                                                 CREATE TABLE [StraatID_SegmentID] (
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
                                                                 
 

                                                                 ALTER TABLE [StraatID_SegmentID] WITH CHECK ADD CONSTRAINT [StraatID_SegmentID_fk0] FOREIGN KEY ([StraatID]) REFERENCES [Straat]([StraatID])
                                                                 ON UPDATE CASCADE
                                                                 
                                                                 ALTER TABLE [StraatID_SegmentID] CHECK CONSTRAINT [StraatID_SegmentID_fk0]
                                                                 
                                                                 ALTER TABLE [StraatID_SegmentID] WITH CHECK ADD CONSTRAINT [StraatID_SegmentID_fk1] FOREIGN KEY ([SegmentID]) REFERENCES [Segment]([SegmentID])
                                                                 ON UPDATE CASCADE
                                                                 
                                                                 ALTER TABLE [StraatID_SegmentID] CHECK CONSTRAINT [StraatID_SegmentID_fk1] ", con))
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
        private static void CreateDataSet()
        {
            dataset = new DataSet();
            DataTable provinceTable = new DataTable();
            provinceTable.TableName = "Provincie";
            provinceTable.Columns.Add(new DataColumn("ProvincieID", typeof(int)));
            provinceTable.Columns.Add(new DataColumn("ProvincieNaam", typeof(string)));
            dataset.Tables.Add(provinceTable);


            DataTable gemeenteTable = new DataTable();
            gemeenteTable.TableName = "Gemeente";
            gemeenteTable.Columns.Add(new DataColumn("GemeenteID", typeof(int)));
            gemeenteTable.Columns.Add(new DataColumn("GemeenteNaam", typeof(string)));
            dataset.Tables.Add(gemeenteTable);


            DataTable straatTable = new DataTable();
            straatTable.TableName = "Straat";
            straatTable.Columns.Add(new DataColumn("StraatID", typeof(int)));
            straatTable.Columns.Add(new DataColumn("StraatNaam", typeof(string)));
            dataset.Tables.Add(straatTable);


            DataTable segmentTable = new DataTable();
            segmentTable.TableName = "Segment";
            segmentTable.Columns.Add(new DataColumn("SegmentID", typeof(int)));
            segmentTable.Columns.Add(new DataColumn("Vertecies", typeof(string)));
            segmentTable.Columns.Add(new DataColumn("BeginKnoopID", typeof(int)));
            segmentTable.Columns.Add(new DataColumn("EindKnoopID", typeof(int)));
            dataset.Tables.Add(segmentTable);


            DataTable provincieID_GemeenteIDTable = new DataTable();
            provincieID_GemeenteIDTable.TableName = "ProvincieID_GemeenteID";
            provincieID_GemeenteIDTable.Columns.Add(new DataColumn("ProvincieID", typeof(int)));
            provincieID_GemeenteIDTable.Columns.Add(new DataColumn("GemeenteID", typeof(int)));
            dataset.Tables.Add(provincieID_GemeenteIDTable);


            DataTable GemeenteID_StraatIDTable = new DataTable();
            GemeenteID_StraatIDTable.TableName = "GemeenteID_StraatID";
            GemeenteID_StraatIDTable.Columns.Add(new DataColumn("GemeenteID", typeof(int)));
            GemeenteID_StraatIDTable.Columns.Add(new DataColumn("StraatID", typeof(int)));
            dataset.Tables.Add(GemeenteID_StraatIDTable);


            DataTable StraatID_SegmentIDTable = new DataTable();
            StraatID_SegmentIDTable.TableName = "StraatID_SegmentID";
            StraatID_SegmentIDTable.Columns.Add(new DataColumn("StraatID", typeof(int)));
            StraatID_SegmentIDTable.Columns.Add(new DataColumn("SegmentID", typeof(int)));
            dataset.Tables.Add(StraatID_SegmentIDTable);
        }
        private static void ImportTxtFilesAndPopulateDataSet()
        {
            string basePath = "D:\\Hogent\\Programmeren\\Programmeren 3\\LABO 1\\";

            List<string[]> alleProvincies = new List<string[]>();

            using (StreamReader sr = new StreamReader(basePath + "alleProvincies.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    alleProvincies.Add(line.Split(","));
                }
            };

            List<string[]> alleGemeentes = new List<string[]>();

            using (StreamReader sr = new StreamReader(basePath + "alleGemeentes.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    alleGemeentes.Add(line.Split(","));
                }
            };

            List<string[]> alleStraten = new List<string[]>();

            using (StreamReader sr = new StreamReader(basePath + "alleStraten.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    alleStraten.Add(line.Split(","));
                }
            };

            List<string[]> alleSegmenten = new List<string[]>();

            using (StreamReader sr = new StreamReader(basePath + "alleSegmenten.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    alleSegmenten.Add(line.Split(","));
                }
            };

            List<string[]> ProvincieID_GemeenteID = new List<string[]>();

            using (StreamReader sr = new StreamReader(basePath + "ProvincieID_GemeenteID.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    ProvincieID_GemeenteID.Add(line.Split(","));
                }
            };

            List<string[]> GemeenteID_StraatID = new List<string[]>();

            using (StreamReader sr = new StreamReader(basePath + "GemeenteID_StraatID.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    GemeenteID_StraatID.Add(line.Split(","));
                }
            };

            List<string[]> StraatID_SegmentID = new List<string[]>();

            using (StreamReader sr = new StreamReader(basePath + "StraatID_SegmentID.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    StraatID_SegmentID.Add(line.Split(","));
                }
            };


            foreach(string[] provincie in alleProvincies)
            {
                dataset.Tables["Provincie"].Rows.Add(int.Parse(provincie[0]), provincie[1].Trim());
            }
            foreach (string[] gemeente in alleGemeentes)
            {
                dataset.Tables["Gemeente"].Rows.Add(int.Parse(gemeente[0]), gemeente[1].Trim());
            }
            foreach (string[] straat in alleStraten)
            {
                dataset.Tables["Straat"].Rows.Add(int.Parse(straat[0]), straat[1].Trim());
            }
            List<int> usedIDs = new List<int>();
            foreach (string[] segment in alleSegmenten)
            {
                if (!usedIDs.Contains(int.Parse(segment[0])))
                {
                    dataset.Tables["Segment"].Rows.Add(int.Parse(segment[0]), segment[1], int.Parse(segment[2]), int.Parse(segment[3]));
                    usedIDs.Add(int.Parse(segment[0]));
                }// check if there are duplicate segment id's
            }
            foreach(string [] provIdGemId in ProvincieID_GemeenteID)
            {
                dataset.Tables["ProvincieID_GemeenteID"].Rows.Add(int.Parse(provIdGemId[0]), int.Parse(provIdGemId[1]));
            }
            foreach(string [] gemIdStrId in GemeenteID_StraatID)
            {
                dataset.Tables["GemeenteID_StraatID"].Rows.Add(int.Parse(gemIdStrId[0]), int.Parse(gemIdStrId[1]));
            }
            foreach(string [] strIDsegID in StraatID_SegmentID)
            {
                dataset.Tables["StraatID_SegmentID"].Rows.Add(int.Parse(strIDsegID[0]), int.Parse(strIDsegID[1]));
            }
        }
        static void BulkCopyDataSet()
        {
            foreach (DataTable table in dataset.Tables)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);
                    foreach (DataColumn column in table.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.DestinationTableName = table.TableName;
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.BatchSize = 10000;
                    try
                    {
                        connection.Open();
                        bulkCopy.WriteToServer(table);
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
