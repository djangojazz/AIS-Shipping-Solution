CREATE VIEW [dbo].[vPersonOrders]
AS
SELECT
	p.FirstName
,	p.LastName
,	o.Description
FROM dbo.tePerson p
	JOIN dbo.teOrder o ON p.PersonId = o.PersonId