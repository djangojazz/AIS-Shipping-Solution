CREATE PROCEDURE [dbo].[pGetAllShips]
AS
	Select ShipId, MMSI, ShipName, Latitude, Longitude from dbo.teShipDetail
RETURN 0