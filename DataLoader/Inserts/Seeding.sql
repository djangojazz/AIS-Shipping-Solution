/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
--DELETIONS
DELETE dbo.Orders
DBCC CHECKIDENT ('dbo.Orders', RESEED, 0);
DELETE dbo.Person
DBCC CHECKIDENT ('dbo.Person', RESEED, 0);
GO

--INSERTS
INSERT INTO dbo.Person (FirstName, LastName) VALUES ('Brett', 'Morin'),('Emily', 'Morin');
INSERT INTO dbo.[Order] ( PersonId, [Description]) VALUES (1, 'Shirt'),(1, 'Pants'),(1, 'Shoes'),(2, 'Dress'),(2, 'Shoes');
GO