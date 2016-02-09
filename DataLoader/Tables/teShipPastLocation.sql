CREATE TABLE [Ships].[teShipPastLocation]
(
	[ShipPastLocationId] INT IDENTITY NOT NULL CONSTRAINT PK_teShipPastLocation_ShipPastLocatoinId PRIMARY KEY,
	[ShipId] INT NOT NULL CONSTRAINT FK_teShipPastLocation_ShipDetail_ShipId FOREIGN KEY (ShipId) REFERENCES Ships.teShipDetail(ShipId),
	[Latitude] FLOAT NOT NULL,
	[Longitude] FLOAT NOT NULL,
	[Created] DATETIME NOT NULL,
)