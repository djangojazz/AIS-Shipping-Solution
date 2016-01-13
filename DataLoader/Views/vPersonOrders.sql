CREATE VIEW [dbo].[vPersonOrders]
AS
SELECT
	p.FirstName
,	p.LastName
,	o.Description
FROM dbo.Person p
	JOIN dbo.Orders o ON p.PersonId = o.PersonId