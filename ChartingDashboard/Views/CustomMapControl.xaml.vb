Imports System.Threading
Imports System.Windows.Threading
Imports Microsoft.Maps.MapControl.WPF

Public Class CustomMapControl

  Dim _textDictionary As Dictionary(Of String, String)

  Public Sub New()
    InitializeComponent()
  End Sub


  Private Sub Map_ViewChangeOnFrame(ByVal sender As Object, ByVal e As MapEventArgs)
    SetDistanceThreshold()
  End Sub

  Private Sub Pushpin_MouseEnter(sender As Object, e As MouseEventArgs)
    Dim pin As FrameworkElement = TryCast(sender, FrameworkElement)
    Dim pos = MapLayer.GetPosition(pin)
    Dim dc = TryCast(DataContext, ChartingDashboardViewModel)

    Dim point = New Point

    map.TryLocationToViewportPoint(pos, point)

    Dim location = DirectCast(pin.Tag, ShipModel)

    _textDictionary = New Dictionary(Of String, String) From {
      {"MMSI", $"{location.MMSI}"},
      {"ShipName", $"{location.ShipName}"},
      {"Mapsize Height:", $"{map.ActualHeight}"},
      {"Mapsize Width:", $"{map.ActualWidth}"},
      {"Collision", $"{location.Collision}"},
      {"Position", $"{pos}"}
    }

    '{"buffer Radius", $"{bufferRadius}"},
    'eventsPanel.Children.Clear()
    AddTextBoxes()
  End Sub

  Private Sub AddTextBoxes()
    Dim brushToUse = CType(Application.Current.Resources("brush.Foreground.MainBoard"), LinearGradientBrush)
    _textDictionary.ToList().ForEach(Sub(x) eventsPanel.Children.Add(New TextBlock With {.Text = $"{x.Key} {x.Value}", .Foreground = brushToUse, .FontSize = 14, .FontWeight = FontWeights.Bold}))
  End Sub

  Private Sub Pushpin_MouseLeave(sender As Object, e As MouseEventArgs)
    eventsPanel.Children.Clear()
  End Sub

  Public Sub SetDistanceThreshold()
    If map.ZoomLevel = 0 Then Exit Sub

    Dim oldLocation = map.ViewportPointToLocation(New Point(0, 0))
    Dim newLocation = map.ViewportPointToLocation(New Point(map.ZoomLevel * 15 / 2, 0))
    Dim Dist = oldLocation.DistanceTo(newLocation, DistanceUnit.Miles)

    TryCast(DataContext, ChartingDashboardViewModel).DistanceThreshold = Dist
  End Sub

End Class
