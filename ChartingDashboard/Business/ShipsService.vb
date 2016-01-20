﻿Imports System.Linq
Imports Microsoft.Maps.MapControl.WPF

Public Class ShipsService
  Public Async Function LoadShipLocations() As Task(Of IList(Of ShipModel))
    Dim dBships = DataAccess.DataConverter.ConvertTo(Of ShipDb)(New DataAccess.SQLTalker().GetData("EXEC dbo.pShipsMockService 's', 5000"))
    Return dBships.Select(Function(x) New ShipModel With
                          {
                          .MMSI = x.MMSI,
                          .ShipName = x.ShipName,
                          .Location = New Location() With {.Latitude = x.Latitude, .Longitude = x.Longitude}
                          }).ToList()
  End Function

  Public Async Function GetAverageLocation(ships As IList(Of ShipModel)) As Task(Of GeocodeService.GeocodeResult)
    Dim averageLat = ships.Average(Function(x) x.Location.Latitude)
    Dim averageLong = ships.Average(Function(x) x.Location.Longitude)

    Return New GeocodeService.GeocodeResult With {.Locations = {New GeocodeService.GeocodeLocation With {.Latitude = averageLat, .Longitude = averageLong}}}
  End Function

  Public Async Function GetRectangleOfLocation(ships As IList(Of ShipModel)) As Task(Of LocationRect)
    Dim lowestLat = ships.OrderBy(Function(x) x.Location.Latitude).First().Location.Latitude
    Dim highestLat = ships.OrderByDescending(Function(x) x.Location.Latitude).First().Location.Latitude
    Dim lowestLong = ships.OrderBy(Function(x) x.Location.Longitude).First().Location.Longitude
    Dim highestLong = ships.OrderByDescending(Function(x) x.Location.Longitude).First().Location.Longitude

    Return New LocationRect With
    {
      .Northeast = New Location With {.Latitude = highestLat, .Longitude = highestLong},
      .Southwest = New Location With {.Latitude = lowestLat, .Longitude = lowestLong}
    }
  End Function

  Public Async Function GeocodeAddress(input As String) As Task(Of GeocodeService.GeocodeResult)
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