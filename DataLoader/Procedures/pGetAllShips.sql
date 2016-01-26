CREATE PROCEDURE [Ships].[pGetAllShips]
AS
	Select ShipId, MMSI, ShipName, Latitude, Longitude, ShipTypeId
	FROM Ships.teShipDetail sd
		
RETURN 0