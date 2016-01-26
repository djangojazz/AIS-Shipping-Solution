CREATE TABLE [dbo].[tePerson]
(
	[PersonId] INT IDENTITY NOT NULL CONSTRAINT PK_Person_PersonID PRIMARY KEY,
	[FirstName] VARCHAR(128),
	[LastName] VARCHAR(128),
)
