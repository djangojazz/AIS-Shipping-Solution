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

    Dictn = New Dictionary(Of String, String) From {
      {"Northwest", $"{bounds.Northwest:F5}"},
      {"Northeast", $"{bounds.Northeast:F5}"},
      {"Southwest", $"{bounds.Southwest:F5}"},
      {"Southeast", $"{bounds.Southeast:F5}"}
      }

    eventsPanel.Children.Clear()
    AddTextBoxes()
  End Sub

  Private Sub Pushpin_MouseEnter(sender As Object, e As MouseEventArgs)
    Dim pin As FrameworkElement = TryCast(sender, FrameworkElement)
    Dim pos = MapLayer.GetPosition(pin)

    Dim rightEdge = map.ActualWidth * 0.5

    Dim viewPoint = map.ViewportPointToLocation(New Point(rightEdge, 0))

    Dim location = DirectCast(pin.Tag, ShipModel)

    Dictn = New Dictionary(Of String, String) From {
      {"MMSI", $"{location.MMSI}"},
      {"ShipName", $"{location.ShipName}"},
      {"Position", $"{pos}"},
      {"ViewPoint", $"{viewPoint}"},
      {"Mapsize Height:", $"{map.ActualHeight}"},
      {"Mapsize Width:", $"{map.ActualWidth}"}
    }

    eventsPanel.Children.Clear()
    AddTextBoxes()
  End Sub

  Private Sub AddTextBoxes()
    Dim brushToUse = CType(Application.Current.Resources("brush.Foreground.MainBoard"), LinearGradientBrush)
    Dictn.ToList().ForEach(Sub(x) eventsPanel.Children.Add(New TextBlock With {.Text = $"{x.Key} {x.Value}", .Foreground = brushToUse, .FontSize = 14, .FontWeight = FontWeights.Bold}))
  End Sub

  Private Sub Pushpin_MouseLeave(sender As Object, e As MouseEventArgs)
    'eventsPanel.Children.Clear()
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
