Select * from Straat






--DROP TABLE dbo.ProvincieID_GemeenteID;
--DROP TABLE dbo.GemeenteID_StraatID;
--DROP TABLE dbo.Straat_Segment
--DROP TABLE dbo.Provincie;
--Drop TABLE dbo.Gemeente;
--DROP TABLE dbo.Segment;
--DROP TABLE dbo.Straat;

--CREATE TABLE [Provincie] (
--	ProvincieID integer NOT NULL,
--	ProvincieNaam varchar(255) NOT NULL,
--  CONSTRAINT [PK_PROVINCIE] PRIMARY KEY CLUSTERED
--  (
--  [ProvincieID] ASC
--  ) WITH (IGNORE_DUP_KEY = OFF)

--)
--CREATE TABLE [Gemeente] (
--	GemeenteID integer NOT NULL,
--	GemeenteNaam varchar(255) NOT NULL,
--  CONSTRAINT [PK_GEMEENTE] PRIMARY KEY CLUSTERED
--  (
--  [GemeenteID] ASC
--  ) WITH (IGNORE_DUP_KEY = OFF)

--)
--CREATE TABLE [ProvincieID_GemeenteID] (
--	ProvincieID integer NOT NULL,
--	GemeenteID integer NOT NULL
--)
--CREATE TABLE [Straat] (
--	StraatID integer NOT NULL,
--	StraatNaam varchar(255) NOT NULL,
--	GraafID integer NOT NULL,
--  CONSTRAINT [PK_STRAAT] PRIMARY KEY CLUSTERED
--  (
--  [StraatID] ASC
--  ) WITH (IGNORE_DUP_KEY = OFF)

--)
--CREATE TABLE [GemeenteID_StraatID] (
--	GemeenteID integer NOT NULL,
--	StraatID integer NOT NULL
--)
--CREATE TABLE [Segment] (
--	SegmentID integer NOT NULL,
--	Vertecies varchar(600) NOT NULL,
--  CONSTRAINT [PK_SEGMENT] PRIMARY KEY CLUSTERED
--  (
--  [SegmentID] ASC
--  ) WITH (IGNORE_DUP_KEY = OFF)

--)
--CREATE TABLE [Straat_Segment] (
--	StraatID integer NOT NULL,
--	SegmentID integer NOT NULL
--)


--ALTER TABLE [ProvincieID_GemeenteID] WITH CHECK ADD CONSTRAINT [ProvincieID_GemeenteID_fk0] FOREIGN KEY ([ProvincieID]) REFERENCES [Provincie]([ProvincieID])
--ON UPDATE CASCADE
--ALTER TABLE [ProvincieID_GemeenteID] CHECK CONSTRAINT [ProvincieID_GemeenteID_fk0]
--ALTER TABLE [ProvincieID_GemeenteID] WITH CHECK ADD CONSTRAINT [ProvincieID_GemeenteID_fk1] FOREIGN KEY ([GemeenteID]) REFERENCES [Gemeente]([GemeenteID])
--ON UPDATE CASCADE
--ALTER TABLE [ProvincieID_GemeenteID] CHECK CONSTRAINT [ProvincieID_GemeenteID_fk1]


--ALTER TABLE [GemeenteID_StraatID] WITH CHECK ADD CONSTRAINT [GemeenteID_StraatID_fk0] FOREIGN KEY ([GemeenteID]) REFERENCES [Gemeente]([GemeenteID])
--ON UPDATE CASCADE
--ALTER TABLE [GemeenteID_StraatID] CHECK CONSTRAINT [GemeenteID_StraatID_fk0]
--ALTER TABLE [GemeenteID_StraatID] WITH CHECK ADD CONSTRAINT [GemeenteID_StraatID_fk1] FOREIGN KEY ([StraatID]) REFERENCES [Straat]([StraatID])
--ON UPDATE CASCADE
--ALTER TABLE [GemeenteID_StraatID] CHECK CONSTRAINT [GemeenteID_StraatID_fk1]


--ALTER TABLE [Straat_Segment] WITH CHECK ADD CONSTRAINT [Straat_Segment_fk0] FOREIGN KEY ([StraatID]) REFERENCES [Straat]([StraatID])
--ON UPDATE CASCADE
--ALTER TABLE [Straat_Segment] CHECK CONSTRAINT [Straat_Segment_fk0]
--ALTER TABLE [Straat_Segment] WITH CHECK ADD CONSTRAINT [Straat_Segment_fk1] FOREIGN KEY ([SegmentID]) REFERENCES [Segment]([SegmentID])
--ON UPDATE CASCADE
--ALTER TABLE [Straat_Segment] CHECK CONSTRAINT [Straat_Segment_fk1]
