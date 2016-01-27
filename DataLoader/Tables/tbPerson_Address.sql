CREATE TABLE [dbo].[tbPerson_Address]
(
	Person_AddressId INT IDENTITY NOT NULL CONSTRAINT PK_tbPerson_Address_Person_AddressId PRIMARY KEY,
	PersonId INT NOT NULL CONSTRAINT FK_tbPerson_Address_Person_PersonID FOREIGN KEY (PersonId) REFERENCES dbo.tePerson(PersonId),
	AddressId INT NOT NULL CONSTRAINT FK_tbPerson_Address_Address_AddressID FOREIGN KEY (PersonId) REFERENCES dbo.teAddress(AddressId)
)
