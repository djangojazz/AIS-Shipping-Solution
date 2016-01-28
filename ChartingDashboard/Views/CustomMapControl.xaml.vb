Imports Microsoft.Maps.MapControl.WPF

Public Class CustomMapControl
  Private Sub Pushpin_MouseEnter(sender As Object, e As MouseEventArgs)
    Dim pin As FrameworkElement = TryCast(sender, FrameworkElement)
    Dim pos = MapLayer.GetPosition(pin)

    Dim rightEdge = map.ActualWidth * 0.5

    Dim viewPoint = map.ViewportPointToLocation(New Point(rightEdge, 0))

    MapLayer.SetPosition(ContentPopup, pos)
    'New Location With {.Latitude = rectange.North, .Longitude = rectange.North})

    MapLayer.SetPositionOffset(ContentPopup, New Point(80, 0))

    Dim location = DirectCast(pin.Tag, ShipModel)

    ContentPopupText.Text = location.MMSI.ToString
    ContentPopupDescription.Text = location.ShipName
    ContentPopupSize.Text = $"{viewPoint}"
    ContentPopupMapSize.Text = $"Height: {map.Height} ActualHeight: {map.ActualHeight} Width: {map.Width} ActualWidth: {map.ActualWidth}"

    'ContentPopup.Visibility = Visibility.Visible

    Dim dict As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
      {"MMSI", $"{location.MMSI}"},
      {"ShipName", $"{location.ShipName}"},
      {"Position", $"{pos}"},
      {"ViewPoint", $"{viewPoint}"},
      {"Mapsize Height:", $"{map.ActualHeight}"},
      {"Mapsize Width:", $"{map.ActualWidth}"}
    }


    eventsPanel.Children.Clear()

    Dim brushToUse = CType(Application.Current.Resources("brush.Foreground.MainBoard"), LinearGradientBrush)

    dict.ToList().ForEach(Sub(x) eventsPanel.Children.Add(New TextBlock With {.Text = $"{x.Key} {x.Value}", .Foreground = brushToUse, .FontSize = 14, .FontWeight = FontWeights.Bold}))
  End Sub

  Private Sub Pushpin_MouseLeave(sender As Object, e As MouseEventArgs)
    ContentPopup.Visibility = Visibility.Collapsed

    eventsPanel.Children.Clear()
  End Sub
End Class
