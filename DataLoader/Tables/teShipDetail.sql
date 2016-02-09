CREATE TABLE [Ships].[teShipDetail]
(
	[ShipId] INT IDENTITY NOT NULL CONSTRAINT PK_ShipDetail_ShipId PRIMARY KEY,
	[MMSI] INT NOT NULL,
	[ShipName] VARCHAR(128),
	[Latitude] FLOAT NOT NULL,
	[Longitude] FLOAT NOT NULL,
	[ShipTypeId] INT NOT NULL CONSTRAINT FK_ShipTypeID_teShipDetail_tdShipType FOREIGN KEY (ShipTypeId) REFERENCES Ships.tdShipType(ShipTypeId),
	[GeographyPoint]  AS ([geography]::STGeomFromText(((('POINT('+CONVERT([varchar](20),[Longitude]))+' ')+CONVERT([varchar](20),[Latitude]))+')',(4326))),
	[LastUpdated] DATETIME NOT NULL,
	[Created] DATETIME NOT NULL,
)
