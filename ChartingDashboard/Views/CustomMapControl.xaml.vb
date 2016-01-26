Imports Microsoft.Maps.MapControl.WPF

Public Class CustomMapControl
  Private Sub Pushpin_MouseEnter(sender As Object, e As MouseEventArgs)
    Dim pin As FrameworkElement = TryCast(sender, FrameworkElement)
    MapLayer.SetPosition(ContentPopup, MapLayer.GetPosition(pin))
    MapLayer.SetPositionOffset(ContentPopup, New Point(20, -15))

    Dim location = DirectCast(pin.Tag, ShipModel)

    ContentPopupText.Text = location.MMSI.ToString
    ContentPopupDescription.Text = location.ShipName
    ContentPopup.Visibility = Visibility.Visible
  End Sub

  Private Sub Pushpin_MouseLeave(sender As Object, e As MouseEventArgs)
    ContentPopup.Visibility = Visibility.Collapsed
  End Sub
End Class
