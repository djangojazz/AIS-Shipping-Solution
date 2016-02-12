CREATE PROCEDURE Ships.pBulkXmlShipLoader
	(
		@Increment INT
	,	@Xml XML
	,	@Output VARCHAR(1024) OUTPUT
	)
AS

BEGIN
    
SET NOCOUNT ON;

DECLARE 
	@ChangesCount		INT
,	@NewCount				INT
,	@AlterCount			INT
,	@NoCount				INT
,	@NL							CHAR = CHAR(10)
,	@IncrementTime	DATETIME = dbo.fDateTimeToNearestIncrement(@Increment)
;

IF OBJECT_ID('tempdb..#TempShip') IS NOT NULL 
	DROP TABLE #TempShip
;

WITH entryIn AS 
	(
	SELECT 
		node.v.value('@ShipId', 'int') AS ShipId
	,	node.v.value('@MMSI', 'int') AS MMSI
	,	node.v.value('@ShipName', 'varchar(256)') AS ShipName
	,	node.v.value('@Latitude', 'float') AS Latitude
	,	node.v.value('@Longitude', 'float') AS Longitude
	,	node.v.value('@ShipTypeId', 'int') AS ShipTypeId
	FROM @Xml.nodes('/ArrayOfShipDb/ShipDb') AS node(v)
	)
SELECT 
	ISNULL(s.ShipId, Null) AS ShipId
,	ISNULL(s.MMSI, i.MMSI) AS MMSI
,	ISNULL(s.ShipName, i.ShipName) AS ShipName
,	ISNULL(s.Latitude, i.Latitude) AS Latitude
,	ISNULL(s.Longitude, i.Longitude) AS Longitude
,	ISNULL(s.ShipTypeId, i.ShipTypeId) AS ShipTypeId
,	s.LastUpdated
,	@IncrementTime AS NewTime
,	CASE WHEN s.LastUpdated IS NULL THEN 2 
			WHEN s.LastUpdated <> @IncrementTime THEN 1 ELSE 0 END AS ShouldEnter
INTO #TempShip
FROM entryIn i
	LEFT JOIN Ships.teShipDetail s ON s.MMSI = i.MMSI

SELECT 
	@NewCount = CASE WHEN ShouldEnter = 2 THEN 1 ELSE 0 END
,	@AlterCount = CASE WHEN ShouldEnter = 1 THEN 1 ELSE 0 END
,	@NoCount = CASE WHEN ShouldEnter = 0 THEN 1 ELSE 0 End
FROM #TempShip

CREATE INDEX IX_tempdb_TempShip_ShipID ON #TempShip(ShipId)

IF @AlterCount > 0
	BEGIN
		MERGE Ships.teShipPastLocation AS t
		USING Ships.teShipDetail AS s 
			ON t.ShipId = s.ShipId AND t.Created = s.LastUpdated
		WHEN NOT MATCHED BY TARGET AND s.LastUpdated <> @IncrementTime
			THEN INSERT (ShipId, Latitude, Longitude, Created) VALUES (s.ShipId, s.Latitude, s.Longitude, s.LastUpdated)
		;

		SELECT @ChangesCount = @@ROWCOUNT

		IF @ChangesCount > 0
		BEGIN
			SELECT @Output = 'Inserted History into Ships.teShipPastLocation for ' + CAST(@ChangesCount AS VARCHAR) + ' records';   
		END

		UPDATE sd
		SET Latitude = s.Latitude, Longitude = s.Longitude, LastUpdated = @IncrementTime
		FROM #TempShip s
			JOIN Ships.teShipDetail sd ON s.MMSI = sd.MMSI
				AND s.ShouldEnter = 1
		;

		SELECT @ChangesCount = @@ROWCOUNT

		IF @ChangesCount > 0
		BEGIN
			SELECT @Output += @NL + 'Updated Ships.teShipDetail for ' + CAST(@ChangesCount AS VARCHAR) + ' records';    
		END
	END

IF @NewCount > 0
	BEGIN
		  INSERT INTO Ships.teShipDetail ( MMSI, ShipName, Latitude, Longitude, ShipTypeId, LastUpdated, Created )
			SELECT MMSI, ShipName, Latitude, Longitude, ShipTypeId, @IncrementTime, @IncrementTime
			FROM #TempShip WHERE ShouldEnter = 2
		  ;

			SELECT @ChangesCount = @@ROWCOUNT

			IF @ChangesCount > 0
			BEGIN
				SELECT @Output = 'Inserted ' + CAST(@ChangesCount AS VARCHAR) + ' into Ships.teShipDetail'
			END
	END

IF @NoCount > 0 AND @NewCount + @AlterCount = 0
	BEGIN
		SELECT @Output =  'There are no records to insert or update at this time'
	END

SET NOCOUNT OFF;
DROP TABLE #TempShip

END
GO