Imports Microsoft.Maps.MapControl.WPF

Public Class MapHelpers
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

  Public Function HaversineDistance(pos1 As Location, pos2 As Location, unit As DistanceUnit) As Double
    Dim R As Double = If((unit = DistanceUnit.Miles), 3960, 6371)
    Dim lat = (pos2.Latitude - pos1.Latitude).ToRadians()
    Dim lng = (pos2.Longitude - pos1.Longitude).ToRadians()
    Dim h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) + Math.Cos(pos1.Latitude.ToRadians()) * Math.Cos(pos2.Latitude.ToRadians()) * Math.Sin(lng / 2) * Math.Sin(lng / 2)
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
End Class
