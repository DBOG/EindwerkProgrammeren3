--Select Straat.StraatNaam 
--	from Straat
--	inner join GemeenteID_StraatID 
--		on Straat.StraatID = GemeenteID_StraatID.StraatID
--	inner join Gemeente
--		on GemeenteID_StraatID.GemeenteID = Gemeente.GemeenteID
--where GemeenteNaam = ' Antwerpen' 


--select top 10  * from Gemeente

--SELECT Straat.StraatNaam as straat, Segment.Vertecies as vertecies, Segment.SegmentID as segmentid, Segment.BeginKnoopID, Segment.EindKnoopID
--	from Straat
--	inner join StraatID_SegmentID 
--		on StraatID_SegmentID.StraatID = Straat.StraatID
--	inner join Segment
--		on StraatID_SegmentID.SegmentID = Segment.SegmentID
--	where Straat.StraatID = '483'


	SELECT Straat.StraatNaam, Segment.Vertecies, Segment.SegmentID, Segment.BeginKnoopID, Segment.EindKnoopID, Gemeente.GemeenteNaam
	from Straat
	inner join StraatID_SegmentID 
		on StraatID_SegmentID.StraatID = Straat.StraatID
	inner join Segment
		on StraatID_SegmentID.SegmentID = Segment.SegmentID
	inner join GemeenteID_StraatID
		on GemeenteID_StraatID.StraatID = Straat.StraatID
	inner join Gemeente
		on GemeenteID_StraatID.StraatID = Straat.StraatID
	where Gemeente.GemeenteNaam = ' Antwerpen' AND Straat.StraatNaam = 'Burchtgracht'