CREATE TABLE [Ships].[teShipVolume]
(
	[ShipVolumeId] INT IDENTITY NOT NULL CONSTRAINT PK_teShipVolume_ShipVolumeID PRIMARY KEY,
	[ShipId] INT NOT NULL CONSTRAINT FK_teShipDetail_teShipVolume_ShipId FOREIGN KEY (ShipId) REFERENCES Ships.teShipDetail(ShipId),
	[BoatHale] INT,
  [ExpectedVolume] INT,
	[CatchTypeID] INT CONSTRAINT FK_teShipVolume_CatchTypeID FOREIGN KEY (CatchTypeID) REFERENCES Ships.tdCatchType(CatchTypeId)
)
