Imports System.Linq

Public Class ShipsService
  Public Async Function LoadShipLocations() As Task(Of List(Of Ship))
    Dim ships As List(Of Ship)
    Dim data = New DataAccess.SQLTalker().GetData("EXEC dbo.pGetAllShips")
    For Each row In data.Rows

    Next


  End Function
End Class