Imports System.Runtime.CompilerServices
Imports Microsoft.Maps.MapControl.WPF

Public Module MapHelpers
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

  'Public Function HaversineDistance(pos1 As Location, pos2 As Location, unit As DistanceUnit) As Double
  '  Dim R As Double = If((unit = DistanceUnit.Miles), 3960, 6371)
  '  Dim lat = ToRadians(pos2.Latitude - pos1.Latitude)
  '  Dim lng = ToRadians(pos2.Longitude - pos1.Longitude)
  '  Dim h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) + Math.Cos(ToRadians(pos1.Latitude)) * Math.Cos(ToRadians(pos2.Latitude)) * Math.Sin(lng / 2) * Math.Sin(lng / 2)
  '  Dim h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)))
  '  Return R * h2
  'End Function

  <Extension>
  Public Function DistanceTo(pos1 As Location, pos2 As Location, unit As DistanceUnit) As Double
    Dim R As Double = If((unit = DistanceUnit.Miles), 3960, 6371)
    Dim lat = ToRadians(pos2.Latitude - pos1.Latitude)
    Dim lng = ToRadians(pos2.Longitude - pos1.Longitude)
    Dim h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) + Math.Cos(ToRadians(pos1.Latitude)) * Math.Cos(ToRadians(pos2.Latitude)) * Math.Sin(lng / 2) * Math.Sin(lng / 2)
    Dim h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)))
    Return R * h2
  End Function

  Public Function ToRadians(val As Double) As Double
    Return (Math.PI / 180) * val
  End Function

  Public Enum DistanceUnit
    Miles
    Kilometers
  End Enum

  Public Function SetDistanceThreshold(zoomLevel As Double) As Double
    Dim bingMap = New Map()

    Dim oldLocation = bingMap.ViewportPointToLocation(New Point(0, 0))
    Dim newLocation = bingMap.ViewportPointToLocation(New Point(bingMap.ZoomLevel * (15 / 2), 0))
    Return oldLocation.DistanceTo(newLocation, DistanceUnit.Miles)
  End Function

End Module
