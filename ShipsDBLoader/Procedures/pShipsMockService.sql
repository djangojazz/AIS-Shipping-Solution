Create PROC Ships.pShipsMockService ( 
	@Datepart			NVARCHAR(3) 
, @Denominator	INT
)
AS 
BEGIN
	DECLARE 
		@SQL NVARCHAR(248)
	,	@Num DECIMAL(24,8)
	,	@Bit bit
  ;

	SELECT @SQL = 'SELECT @Num = CAST( (DATEDIFF(' + @Datepart + ', DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE()) * 1.0) / ' + CAST(@Denominator AS NVARCHAR) + ' AS DECIMAL(24,8))'
	SELECT @Bit = CRYPT_GEN_RANDOM(1) % 2

	EXEC sp_executesql @SQL, N'@Num decimal(24,8) output', @Num = @Num OUTPUT
  
	SELECT
		sd.ShipId
	,	sd.MMSI
	,	sd.ShipName
	,	CASE WHEN @Bit = 0 THEN sd.Latitude + @Num ELSE sd.Latitude - @Num END AS Latitude
	,	CASE WHEN @Bit = 0 THEN sd.Longitude + @Num ELSE sd.Longitude - @Num END AS Longitude
	,	sd.ShipTypeId
	,	sv.ShipVolumeId
	,	sv.BoatHale
	,	sv.ExpectedVolume
	,	sv.CatchTypeID
	FROM Ships.teShipDetail sd
		LEFT JOIN Ships.teShipVolume sv ON sd.ShipId = sv.ShipId

END
GO