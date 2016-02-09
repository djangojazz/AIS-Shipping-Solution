CREATE PROCEDURE Ships.pInsertOrUpdateShipPosition 
	(
		@Incremenet INT
	,	@MMSI INT	
	,	@ShipName VARCHAR(256)
	,	@Latitude INT	
  ,	@Longitude INT
  ,	@ShipTypeId INT
	)

AS

BEGIN
	SET NOCOUNT ON;
	DECLARE @FifteenMinutesOfNow DATETIME = CASE WHEN (DATEPART(MINUTE, GETDATE()) % @Incremenet) * 60 + DATEPART(SECOND, GETDATE()) < 449 
		THEN DATEADD(minute, datediff(minute,0, GETDATE()) / @Incremenet * @Incremenet, 0)
		ELSE DATEADD(minute, (DATEDIFF(minute,0, GETDATE()) / @Incremenet * @Incremenet) + @Incremenet, 0) END

	IF EXISTS (SELECT 1 FROM Ships.teShipDetail WHERE MMSI = @MMSI)
		BEGIN
			MERGE Ships.teShipPastLocation AS t
			USING Ships.teShipDetail AS s
			ON t.ShipId = s.ShipId AND t.Created = s.LastUpdated
			WHEN NOT MATCHED BY TARGET AND s.MMSI = @MMSI AND s.LastUpdated <> @FifteenMinutesOfNow
				THEN INSERT (ShipId, Latitude, Longitude, Created) VALUES (s.ShipId, s.Latitude, s.Longitude, s.LastUpdated)
			;
			Print 'Inserted History into Ships.teShipPastLocation'
			

			UPDATE Ships.teShipDetail
			SET Latitude = @Latitude, Longitude = @Longitude, LastUpdated = @FifteenMinutesOfNow
			WHERE MMSI = @MMSI AND LastUpdated <> @FifteenMinutesOfNow
			Print 'Updated Ships.teShipDetail'
			;    
		END
	ELSE
		BEGIN
		    INSERT INTO Ships.teShipDetail ( MMSI, ShipName, Latitude, Longitude, ShipTypeId, LastUpdated, Created )
		    VALUES  ( @MMSI, @ShipName, @Latitude, @Longitude, @ShipTypeId, @FifteenMinutesOfNow, @FifteenMinutesOfNow)
				Print 'Inserted into Ships.teShipDetail'
		END
	SET NOCOUNT OFF;
END