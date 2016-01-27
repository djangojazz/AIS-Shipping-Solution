CREATE PROCEDURE [dbo].[pFindMyAddress] @PersonId INT
AS
	SELECT a.*
	FROM dbo.teAddress a
		INNER JOIN dbo.tbPerson_Address pa ON a.AddressId = pa.AddressId
				AND pa.PersonId = @PersonId
RETURN 0
