Select Gemeente.GemeenteNaam as Gemeente, Provincie.ProvincieNaam as Provincie
from ProvincieID_GemeenteID
	inner join Gemeente 
		on ProvincieID_GemeenteID.GemeenteID = Gemeente.GemeenteID
	INNER JOIN Provincie 
		on ProvincieID_GemeenteID.ProvincieID = Provincie.ProvincieID
