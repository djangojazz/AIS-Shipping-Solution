CREATE PROCEDURE [dbo].[pGetOrdersByPersonId] (@PersonId INT = 1)
AS
BEGIN
  SELECT
		p.PersonId
	,	p.FirstName
	,	p.LastName
	,	o.OrderId
	,	o.Description
	FROM dbo.tePerson p
		JOIN dbo.teOrder o ON p.PersonId = o.PersonId
	WHERE p.PersonId = @PersonId
RETURN 0  
END
	
