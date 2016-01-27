CREATE TABLE [dbo].[teAddress]
(
	AddressId INT IDENTITY NOT NULL CONSTRAINT PK_teAddress_AddressId PRIMARY KEY,
	StreetAddress VARCHAR(256) NOT NULL,
	StreetAddressExtended VARCHAR(256),
	City VARCHAR(256) NOT NULL,
	[State] CHAR(2) NOT NULL,
	ZipCode INT NOT NULL,
	Latitude FLOAT NOT NULL,
	Longitude FLOAT NOT NULL,
	GeographyPoint  AS ([geography]::STGeomFromText(((('POINT('+CONVERT([varchar](20),[Longitude]))+' ')+CONVERT([varchar](20),[Latitude]))+')',(4326)))
)
