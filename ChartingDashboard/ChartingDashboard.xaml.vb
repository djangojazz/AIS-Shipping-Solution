Imports Microsoft.Maps.MapControl.WPF
Imports System.Windows
Imports System.Windows.Input

Class ChartingDashboard
  Public Sub New()
    InitializeComponent()
    DataContext = New ChartingDashboardViewModel
  End Sub

  Private Sub Pushpin_MouseEnter(sender As Object, e As MouseEventArgs)
    Dim pin As FrameworkElement = TryCast(sender, FrameworkElement)
    MapLayer.SetPosition(ContentPopup, MapLayer.GetPosition(pin))
    MapLayer.SetPositionOffset(ContentPopup, New Point(20, -15))

    Dim location = DirectCast(pin.Tag, Ship)

    ContentPopupText.Text = location.ShipName
    ContentPopupDescription.Text = $"MMSI: {location.MMSI} Lat: {location.Latitude} Long: {location.Longitude}"
    ContentPopup.Visibility = Visibility.Visible
  End Sub

  Private Sub Pushpin_MouseLeave(sender As Object, e As MouseEventArgs)
    ContentPopup.Visibility = Visibility.Collapsed
  End Sub
End Class
