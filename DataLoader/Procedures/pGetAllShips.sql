CREATE PROCEDURE [Ships].[pGetAllShips]
AS
	Select 
		sd.ShipId
	,	sd.MMSI
	, sd.ShipName
	, sd.Latitude
	, sd.Longitude
	, sd.ShipTypeId
	,	sv.ShipVolumeId
	,	sv.BoatHale
	,	sv.ExpectedVolume
	,	sv.CatchTypeID
	FROM Ships.teShipDetail sd
		LEFT JOIN Ships.teShipVolume sv ON sd.ShipId = sv.ShipId
		
RETURN 0