Imports Microsoft.Maps.MapControl.WPF

Public Class CustomMapControl

  Dim Dictn As Dictionary(Of String, String)

  Public Sub New()
    InitializeComponent()
    AddHandler map.ViewChangeOnFrame, AddressOf Map_ViewChangeOnFrame
  End Sub

  Private Sub Map_ViewChangeOnFrame(ByVal sender As Object, ByVal e As MapEventArgs)
    'Gets the map that raised this event
    Dim map As Map = CType(sender, Map)
    'Gets the bounded rectangle for the current frame
    Dim bounds As LocationRect = map.BoundingRectangle
    Dim averageLong = (bounds.Northwest.Longitude + bounds.Northeast.Longitude) / 2
    Dim averageLat = (bounds.Northeast.Latitude + bounds.Southeast.Latitude) / 2

    Dim distanceNS = MapHelpers.HaversineDistance(bounds.Northwest, bounds.Southwest, DistanceUnit.Miles)
    Dim distanceEW = MapHelpers.HaversineDistance(bounds.Northwest, bounds.Northeast, DistanceUnit.Miles)

    Dim dc = TryCast(DataContext, ChartingDashboardViewModel)

    Dim myHome = dc.ShipLocations.FirstOrDefault(Function(x) x.MMSI = 1).Location

    'Dim thaiRoses = dc.ShipLocations.FirstOrDefault(Function(x) x.MMSI = 2).Location
    'Dim seattle = dc.ShipLocations.FirstOrDefault(Function(x) x.MMSI = 3).Location

    'Dim homeThaiRoses = MapHelpers.HaversineDistance(myHome, thaiRoses, DistanceUnit.Miles)
    'Dim homeSeattle = MapHelpers.HaversineDistance(myHome, seattle, DistanceUnit.Miles)

    'Dim seattle = New Location With {.Latitude = 47.6149942, .Longitude = -122.4759882}

    Dictn = New Dictionary(Of String, String) From {
      {"MAP SIZING", String.Empty},
      {"Mapsize Height:", $"{map.ActualHeight}"},
      {"Mapsize Width", $"{map.ActualWidth}"},
      {"Northwest", $"{bounds.Northwest:F5}"},
      {"Northeast", $"{bounds.Northeast:F5}"},
      {"Southwest", $"{bounds.Southwest:F5}"},
      {"Southeast", $"{bounds.Southeast:F5}"},
      {"BOAT SIZE", String.Empty},
      {"Boat Length", $"{dc.Dimension}"}
      }

    '{"KNOWN DISTANCES", String.Empty},
    '{"Distance Home To Thai Roses", $"{homeThaiRoses}"},
    '{"Distance Home To Seattle", $"{homeSeattle}"}
    '{"MAP DISTANCES", String.Empty},
    '{"DistanceNS", $"{distanceNS}"},
    '{"DistanceEW", $"{distanceEW}"},

    eventsPanel.Children.Clear()
    AddTextBoxes()
  End Sub

  Private Sub Pushpin_MouseEnter(sender As Object, e As MouseEventArgs)
    Dim pin As FrameworkElement = TryCast(sender, FrameworkElement)
    Dim pos = MapLayer.GetPosition(pin)

    Dim rightEdge = map.ActualWidth * 0.5

    Dim point = New Point

    map.TryLocationToViewportPoint(pos, point)
    Dim dc = TryCast(DataContext, ChartingDashboardViewModel)

    Dim newLocation = map.ViewportPointToLocation(New Point(point.X + (dc.Dimension / 2), point.Y))

    Dim newPin As New Pushpin()
    newPin.Location = newLocation

    map.Children.Add(newPin)

    Dim location = DirectCast(pin.Tag, ShipModel)

    Dictn = New Dictionary(Of String, String) From {
      {"MMSI", $"{location.MMSI}"},
      {"ShipName", $"{location.ShipName}"},
      {"Position", $"{pos}"},
      {"Mapsize Height:", $"{map.ActualHeight}"},
      {"Mapsize Width:", $"{map.ActualWidth}"}
    }

    '{"ViewPoint", $"{viewPoint}"},

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
End Class
