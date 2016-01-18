Imports System.Linq
Imports Microsoft.Maps.MapControl.WPF

Public Class ShipsService
  Public Async Function LoadShipLocations() As Task(Of IList(Of ShipModel))
    Dim dBships = DataAccess.DataConverter.ConvertTo(Of ShipDb)(New DataAccess.SQLTalker().GetData("EXEC dbo.pGetAllShips"))
    Return dBships.Select(Function(x) New ShipModel With
                          {
                          .MMSI = x.MMSI,
                          .ShipName = x.ShipName,
                          .Location = New Location() With {.Latitude = x.Latitude, .Longitude = x.Longitude}
                          }).ToList()

  End Function

  Public Function GeocodeAddress(input As String) As GeocodeService.GeocodeResult
    Using client As New GeocodeService.GeocodeServiceClient("CustomBinding_IGeocodeService")
      Dim request As New GeocodeService.GeocodeRequest()
      request.Credentials = New Credentials() With {
          .ApplicationId = TryCast(Application.Current.Resources("BingCredentials"), ApplicationIdCredentialsProvider).ApplicationId
        }
      request.Query = input
      Return client.Geocode(request).Results(0)
    End Using
  End Function
End Class