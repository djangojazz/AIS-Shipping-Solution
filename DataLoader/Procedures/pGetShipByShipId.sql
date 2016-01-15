CREATE PROCEDURE [dbo].[pGetShipByShipId]
	@ShipId int
AS
BEGIN
  Select * from dbo.ShipDetail WHERE ShipId = @ShipId  
END