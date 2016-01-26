CREATE TABLE [dbo].[teOrder]
(
	[OrderId]			INT IDENTITY	NOT NULL CONSTRAINT PK_Order_OrderId PRIMARY KEY,
	[PersonId]		INT						NOT NULL CONSTRAINT FK_Order_Person_PersonID FOREIGN KEY(PersonId) REFERENCES dbo.tePerson(PersonId),
	[Description] VARCHAR(32)
)
