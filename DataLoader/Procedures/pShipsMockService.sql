Create PROC dbo.pShipsMockService ( 
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
		ShipId
	,	MMSI
	,	ShipName
	,	CASE WHEN @Bit = 0 THEN Latitude + @Num ELSE Latitude - @Num END AS Latitude
	,	CASE WHEN @Bit = 0 THEN Longitude + @Num ELSE Longitude - @Num END AS Longitude
	FROM dbo.ShipDetail

END
GO