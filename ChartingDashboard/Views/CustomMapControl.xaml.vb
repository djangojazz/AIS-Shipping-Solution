Imports Microsoft.Maps.MapControl.WPF

Public Class CustomMapControl

  Dim Dictn As Dictionary(Of String, String)

  Public Sub New()
    InitializeComponent()
  End Sub

  Private Sub Map_ViewChangeOnFrame(ByVal sender As Object, ByVal e As MapEventArgs)
    'Gets the map that raised this event
    Dim map As Map = CType(sender, Map)
    'Gets the bounded rectangle for the current frame
    Dim bounds As LocationRect = map.BoundingRectangle
    Dim averageLong = (bounds.Northwest.Longitude + bounds.Northeast.Longitude) / 2
    Dim averageLat = (bounds.Northeast.Latitude + bounds.Southeast.Latitude) / 2

    'Dim distanceNS = MapHelpers.HaversineDistance(bounds.Northwest, bounds.Southwest, DistanceUnit.Miles)
    'Dim distanceEW = MapHelpers.HaversineDistance(bounds.Northwest, bounds.Northeast, DistanceUnit.Miles)

    Dim dc = TryCast(DataContext, ChartingDashboardViewModel)

    Dim myHome = dc.ShipLocations.FirstOrDefault(Function(x) x.MMSI = 1).Location
    Dim thaiRoses = dc.ShipLocations.FirstOrDefault(Function(x) x.MMSI = 2).Location
    'Dim seattle = dc.ShipLocations.FirstOrDefault(Function(x) x.MMSI = 3).Location

    'Dim collision = LocationsCollide(myHome, thaiRoses)

    Dictn = New Dictionary(Of String, String) From {
      {"MAP SIZING", String.Empty},
      {"Mapsize Height:", $"{map.ActualHeight}"},
      {"Mapsize Width", $"{map.ActualWidth}"},
      {"Northwest", $"{bounds.Northwest:F5}"},
      {"Northeast", $"{bounds.Northeast:F5}"},
      {"Southwest", $"{bounds.Southwest:F5}"},
      {"Southeast", $"{bounds.Southeast:F5}"},
      {"BOAT SIZE", String.Empty},
      {"COLLISION", String.Empty}
      }

    '{"Collision:", $"{collision}"}
    '{"Boat Length", $"{dc.Dimension}"},

    eventsPanel.Children.Clear()
    AddTextBoxes()
  End Sub

  Private Sub Pushpin_MouseEnter(sender As Object, e As MouseEventArgs)
    Dim pin As FrameworkElement = TryCast(sender, FrameworkElement)
    Dim pos = MapLayer.GetPosition(pin)
    Dim dc = TryCast(DataContext, ChartingDashboardViewModel)

    Dim point = New Point

    map.TryLocationToViewportPoint(pos, point)

    'Test
    'Dim newLocation = map.ViewportPointToLocation(New Point(point.X + (dc.Dimension / 2), point.Y))
    'Dim bufferRadius = pos.DistanceTo(newLocation, DistanceUnit.Miles)

    'map.Children.Add(New Pushpin With {.Location = newLocation})

    Dim location = DirectCast(pin.Tag, ShipModel)

    Dictn = New Dictionary(Of String, String) From {
      {"MMSI", $"{location.MMSI}"},
      {"ShipName", $"{location.ShipName}"},
      {"Mapsize Height:", $"{map.ActualHeight}"},
      {"Mapsize Width:", $"{map.ActualWidth}"},
      {"Collision", $"{location.Overlaps}"},
      {"Position", $"{pos}"}
    }

    '{"buffer Radius", $"{bufferRadius}"},
    'eventsPanel.Children.Clear()
    AddTextBoxes()
  End Sub

  Private Sub AddTextBoxes()
    Dim brushToUse = CType(Application.Current.Resources("brush.Foreground.MainBoard"), LinearGradientBrush)
    Dictn.ToList().ForEach(Sub(x) eventsPanel.Children.Add(New TextBlock With {.Text = $"{x.Key} {x.Value}", .Foreground = brushToUse, .FontSize = 14, .FontWeight = FontWeights.Bold}))
  End Sub

  Private Sub Pushpin_MouseLeave(sender As Object, e As MouseEventArgs)
    eventsPanel.Children.Clear()
  End Sub

  Private Sub map_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
    e.Handled = True

    Dim mousePosition As Point = e.GetPosition(Me)

    'Convert the mouse coordinates to a locatoin on the map
    Dim pinLocation As Location = map.ViewportPointToLocation(mousePosition)

    Dictn = New Dictionary(Of String, String) From {
      {"Point", $"{mousePosition}"},
      {"Location", $"{pinLocation}"}
    }

    AddTextBoxes()

    ' The pushpin to add to the map.
    Dim pin As New Pushpin()
    pin.Location = pinLocation

    ' Adds the pushpin to the map.
    map.Children.Add(pin)

  End Sub

  Private Function SetDistanceThreshold(loc1 As Location) As Double
    Dim dc = TryCast(DataContext, ChartingDashboardViewModel)

    Dim point1 = New Point

    map.TryLocationToViewportPoint(loc1, point1)

    'I only need to get a buffer zone for the radius once as both locations should have the same field for reference
    Dim newLocation = map.ViewportPointToLocation(New Point(point1.X + (dc.Dimension / 2), point1.Y))
    Dim bufferRadius = loc1.DistanceTo(newLocation, DistanceUnit.Miles)

    'Dim distanceBetweenPoints = loc1.DistanceTo(loc2, DistanceUnit.Miles)

    'If ((bufferRadius * 2) > distanceBetweenPoints) Then
    '  Return True
    'Else
    '  Return False
    'End If
  End Function
End Class
