﻿/*
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
TRUNCATE TABLE dbo.ShipDetail
GO

--INSERTS
SET IDENTITY_INSERT dbo.Person ON;
INSERT INTO dbo.Person (PersonId, FirstName, LastName) VALUES (1, 'Brett', 'Morin'),(2, 'Emily', 'Morin');
SET IDENTITY_INSERT dbo.Person OFF;
SET IDENTITY_INSERT dbo.Orders ON;
INSERT INTO dbo.Orders ( OrderId, PersonId, [Description]) VALUES (1, 1, 'Shirt'),(2, 1, 'Pants'),(3, 1, 'Shoes'),(4, 2, 'Dress'),(5, 2, 'Shoes');
SET IDENTITY_INSERT dbo.Orders OFF;
INSERT INTO dbo.ShipDetail (MMSI, ShipName, Latitude, Longitude) VALUES (367197230, 'Buck & Ann', 46.451859, -124.322418),(367150410, 'Major', 46.333302, -124.116333)
,(367002170, 'Edward Brusco', 46.256516, -124.023048),(367002180, 'David Brusco', 47.625912, -124.761200),(316023833, 'Raw Spirit', 50.769775, -127.362129)
GO