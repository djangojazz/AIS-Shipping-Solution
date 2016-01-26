CREATE TABLE [dbo].[teShipDetail]
(
	[ShipId] INT IDENTITY NOT NULL CONSTRAINT PK_ShipDetail_ShipId PRIMARY KEY,
	[MMSI] INT NOT NULL,
	[ShipName] VARCHAR(128),
	[Latitude] FLOAT NOT NULL,
	[Longitude] FLOAT NOT NULL,
	[GeographyPoint]  AS ([geography]::STGeomFromText(((('POINT('+CONVERT([varchar](20),[Longitude]))+' ')+CONVERT([varchar](20),[Latitude]))+')',(4326)))
)
