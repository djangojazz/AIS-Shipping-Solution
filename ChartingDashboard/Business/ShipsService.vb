Imports System.Linq
Imports Microsoft.Maps.MapControl.WPF

Public Class ShipsService
  Public Function TestLoadShipLocations() As IList(Of ShipModel)
    Dim dBships = DataAccess.DataConverter.ConvertTo(Of ShipDb)(New DataAccess.SQLTalker().GetData("EXEC Ships.pShipsMockService 's', 10000"))
    Return dBships.Select(Function(x) New ShipModel With
                          {
                          .MMSI = x.MMSI,
                          .ShipName = x.ShipName,
                          .ShipType = DirectCast(x.ShipTypeId, ShipType),
                          .Location = New Location() With {.Latitude = x.Latitude, .Longitude = x.Longitude}
                          }).ToList()
  End Function

  Public Async Function GetAverageLocation(ships As IList(Of ShipModel)) As Task(Of GeocodeService.GeocodeResult)
    Dim averageLat = ships.Average(Function(x) x.Location.Latitude)
    Dim averageLong = ships.Average(Function(x) x.Location.Longitude)

    Return New GeocodeService.GeocodeResult With {.Locations = {New GeocodeService.GeocodeLocation With {.Latitude = averageLat, .Longitude = averageLong}}}
  End Function

  Public Function GetRectangleOfLocation(ships As IList(Of ShipModel), Optional padding As Double = 0) As LocationRect
    Dim lowestLat = ships.OrderBy(Function(x) x.Location.Latitude).First().Location.Latitude - padding
    Dim highestLat = ships.OrderByDescending(Function(x) x.Location.Latitude).First().Location.Latitude + padding
    Dim lowestLong = ships.OrderBy(Function(x) x.Location.Longitude).First().Location.Longitude - padding
    Dim highestLong = ships.OrderByDescending(Function(x) x.Location.Longitude).First().Location.Longitude + padding

    Return New LocationRect With
    {
      .Northeast = New Location With {.Latitude = highestLat, .Longitude = highestLong},
      .Southwest = New Location With {.Latitude = lowestLat, .Longitude = lowestLong}
    }
  End Function

  'May never be needed
  'Public Async Function GeocodeAddress(input As String) As Task(Of GeocodeService.GeocodeResult)
  '  Using client As New GeocodeService.GeocodeServiceClient("CustomBinding_IGeocodeService")
  '    Dim request As New GeocodeService.GeocodeRequest()
  '    request.Credentials = New Credentials() With {
  '        .ApplicationId = TryCast(Application.Current.Resources("BingCredentials"), ApplicationIdCredentialsProvider).ApplicationId
  '      }
  '    request.Query = input
  '    Return client.Geocode(request).Results(0)
  '  End Using
  'End Function
End Class