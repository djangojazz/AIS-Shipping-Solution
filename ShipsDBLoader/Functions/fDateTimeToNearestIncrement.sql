CREATE FUNCTION dbo.fDateTimeToNearestIncrement
	(
	@Increment INT
	)

RETURNS DATETIME

As

BEGIN
Return CASE WHEN (DATEPART(MINUTE, GETDATE()) % @Increment) * 60 + DATEPART(SECOND, GETDATE()) < (@Increment * 60) / 2 - 1 
		THEN DATEADD(minute, datediff(minute,0, GETDATE()) / @Increment * @Increment, 0)
		ELSE DATEADD(minute, (DATEDIFF(minute,0, GETDATE()) / @Increment * @Increment) + @Increment, 0) END
END