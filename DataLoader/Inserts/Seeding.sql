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
DELETE dbo.tbPerson_Address
DBCC CHECKIDENT ('dbo.tbPerson_Address', RESEED, 0);
GO

DELETE dbo.teAddress
DBCC CHECKIDENT ('dbo.teAddress', RESEED, 0);
GO

DELETE dbo.teOrder
DBCC CHECKIDENT ('dbo.teOrder', RESEED, 0);

DELETE dbo.tePerson
DBCC CHECKIDENT ('dbo.tePerson', RESEED, 0);

DELETE Ships.teShipVolume
DBCC CHECKIDENT ('Ships.teShipVolume', RESEED, 0);

DELETE Ships.teShipDetail
DBCC CHECKIDENT ('Ships.teShipDetail', RESEED, 0);

DELETE Ships.tdShipType
DBCC CHECKIDENT ('Ships.tdShipType', RESEED, 0);
GO

DELETE Ships.tdCatchType
DBCC CHECKIDENT ('Ships.tdCatchType', RESEED, 0);
GO

--INSERTS
SET IDENTITY_INSERT dbo.teAddress ON;
INSERT INTO dbo.teAddress (AddressId, StreetAddress, City, [State], ZipCode, Latitude, Longitude) VALUES (1, '7560 SW Lara St.', 'Portland', 'OR', 97223, 45.4573057, -122.7565177);
SET IDENTITY_INSERT dbo.teAddress OFF;

SET IDENTITY_INSERT dbo.tePerson ON;
INSERT INTO dbo.tePerson (PersonId, FirstName, LastName) VALUES (1, 'Brett', 'Morin'),(2, 'Emily', 'Morin');
SET IDENTITY_INSERT dbo.tePerson OFF;

SET IDENTITY_INSERT dbo.tbPerson_Address ON;
INSERT INTO dbo.tbPerson_Address (Person_AddressId, PersonId, AddressId) VALUES (1, 1, 1);
SET IDENTITY_INSERT dbo.tbPerson_Address OFF;

SET IDENTITY_INSERT dbo.teOrder ON;
INSERT INTO dbo.teOrder ( OrderId, PersonId, [Description]) VALUES (1, 1, 'Shirt'),(2, 1, 'Pants'),(3, 1, 'Shoes'),(4, 2, 'Dress'),(5, 2, 'Shoes');
SET IDENTITY_INSERT dbo.teOrder OFF;

SET IDENTITY_INSERT Ships.tdShipType ON;
INSERT INTO Ships.tdShipType ( ShipTypeID, ShipTypeName) VALUES (1, 'Owned'),(2, 'Contractor'),(3, 'Other');
SET IDENTITY_INSERT Ships.tdShipType OFF;

SET IDENTITY_INSERT Ships.tdCatchType ON;
INSERT INTO Ships.tdCatchType ( CatchTypeId, CatchTypeName) VALUES (1, 'Rockfish'), (2, 'Shrimp'), (3, 'Salmon'), (4, 'Crab'), (5, 'DoverSole');
SET IDENTITY_INSERT Ships.tdCatchType OFF;

SET IDENTITY_INSERT Ships.teShipDetail ON;
INSERT INTO Ships.teShipDetail (ShipId, MMSI, ShipName, Latitude, Longitude, ShipTypeID) VALUES 
(1,111111111, 'Anne Sleuth', 46.851859, -129.322418, 1),
(2,367197230, 'Buck & Ann', 46.451859, -124.322418, 1),
(3,371321345, 'Chrimeney Jeeps', 51.421869, -135.322418, 1),
(4,316023833, 'Raw Spirit', 50.769775, -127.362129, 1),
(5,111112211, 'Bon Voyage', 47.871859, -130.322418, 2),
(6,381321345, 'Chappy Pappy', 51.456469, -131.135418, 3),
(7,371341345, 'Charlies Cheepo', 40.456469, -126.155418, 3),
(8,384841345, 'Dudes Dud', 43.756854, -124.456789, 3),
(9,397541345, 'Earles Agony', 43.897445, -124.557781, 3),
(10,471233478, 'Franks SingleFleet', 45.114445, -127.654811, 2),
(11,124835678, 'Forget Flying', 40.458123, -123.551451, 3),
(12,813545664, 'Genas Sinker', 46.124545, -127.654811, 3),
(13,876541242, 'Hectors Habit', 40.114445, -127.774811, 2),
(14,991233478, 'Icant Float', 45.548123, -129.811451, 3),
(15,871233478, 'Jacks Rwild', 42.712387, -133.887451, 3),
(16,888233478, 'Kelly CanUStop', 41.112387, -128.457451, 2),
(17,678433478, 'Larry Leaper', 40.512387, -125.877451, 3),
(18,367150410, 'Major Major', 46.333302, -124.116333, 3),
(19,387150410, 'Neato Torpedo', 47.333302, -124.056333, 3),
(20,487150410, 'October Ocean', 48.333302, -126.556333, 3),
(21,587230410, 'Pauls PushingIt', 46.583302, -127.586333, 3),
(22,467230410, 'Quest4 Johnny', 48.183302, -128.686333, 3),
(23,547230410, 'Roger DodgerOver', 49.283302, -124.786333, 3),
(24,847251152, 'Sarah Sleeting', 48.285102, -127.586333, 2),
(25,947252151, 'Tom And Jerry', 39.285102, -128.456333, 3),
(26,101252151, 'Ugotta B KiddingMe', 30.285102, -128.452233, 3),
(27,132452151, 'Versions Unknown', 33.285102, -140.454233, 3),
(28,242452151, 'Wild For The Sea', 40.518102, -145.334233, 3),
(29,587452151, 'X The Racer', 45.518102, -124.344233, 2),
(30,681452151, 'Yes I can', 40.418202, -123.486233, 3),
(31,876452151, 'Zuess', 41.418202, -128.546233, 2),
(32,367002170, 'Edward Brusco', 46.256516, -124.023048, 3),
(33,367002180, 'David Brusco', 47.625912, -124.761200, 2)
;
SET IDENTITY_INSERT Ships.teShipDetail OFF;

SET IDENTITY_INSERT Ships.teShipVolume ON;
INSERT INTO Ships.teShipVolume (ShipVolumeId, ShipId, BoatHale, ExpectedVolume, CatchTypeId) VALUES 
(1,1, 3500, 4000, 5),
(2,1, 5000, 20000, 3),
(3,2, 500, 2000, 2),
(4,3, 4000, 20000, 1)
;
SET IDENTITY_INSERT Ships.teShipVolume OFF;

GO