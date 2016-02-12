CREATE PROCEDURE Ships.pInsertOrUpdateShipPosition 
	(
		@Increment INT
	,	@MMSI INT	
	,	@ShipName VARCHAR(256)
	,	@Latitude FLOAT	
  ,	@Longitude FLOAT
  ,	@ShipTypeId INT
	)

AS

BEGIN
	SET NOCOUNT ON;
	DECLARE @IncrementTime DATETIME = dbo.fDateTimeToNearestIncrement(@Increment)
	DECLARE @ShipLastUpdated DATETIME;
	
	SELECT @ShipLastUpdated = LastUpdated FROM Ships.teShipDetail WHERE MMSI = @MMSI

	IF (@ShipLastUpdated > '1901-01-01')
		BEGIN
			IF (@ShipLastUpdated <> @IncrementTime)
				BEGIN
					MERGE Ships.teShipPastLocation AS t
					USING Ships.teShipDetail AS s
					ON t.ShipId = s.ShipId AND t.Created = s.LastUpdated
					WHEN NOT MATCHED BY TARGET AND s.MMSI = @MMSI AND s.LastUpdated <> @IncrementTime
						THEN INSERT (ShipId, Latitude, Longitude, Created) VALUES (s.ShipId, s.Latitude, s.Longitude, s.LastUpdated)
					;

					Print 'Inserted History into Ships.teShipPastLocation'

					UPDATE Ships.teShipDetail
					SET Latitude = @Latitude, Longitude = @Longitude, LastUpdated = @IncrementTime
					WHERE MMSI = @MMSI AND LastUpdated <> @IncrementTime
					Print 'Updated Ships.teShipDetail'
					;    
				END	
			ELSE

				PRINT 'Increment has not been reached so no entry should be made'
		END
	ELSE
		BEGIN
		    INSERT INTO Ships.teShipDetail ( MMSI, ShipName, Latitude, Longitude, ShipTypeId, LastUpdated, Created )
		    VALUES  ( @MMSI, @ShipName, @Latitude, @Longitude, @ShipTypeId, @IncrementTime, @IncrementTime)
				Print 'Inserted into Ships.teShipDetail'
		END
	SET NOCOUNT OFF;
END