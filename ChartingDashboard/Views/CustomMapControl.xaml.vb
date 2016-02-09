Imports System.Threading
Imports System.Windows.Threading
Imports Microsoft.Maps.MapControl.WPF

Public Class CustomMapControl

  Private _textLegend As List(Of Tuple(Of String, String))
  Private _previousZoom As Integer

  Private Sub Map_ViewChangeOnFrame(ByVal sender As Object, ByVal e As MapEventArgs)
    Dim zm = CInt(bingMap.TargetZoomLevel)
    If (_previousZoom <> zm) Then
      _previousZoom = zm
      SetDistanceThreshold(zm)
      'Dim rectangle = TryCast(DataContext, ChartingDashboardViewModel).LocationRectangle
      'Dim newRect = bingMap.BoundingRectangle
    End If
  End Sub

  Private Sub Pushpin_MouseEnter(sender As Object, e As MouseEventArgs)
    Dim pin As FrameworkElement = TryCast(sender, FrameworkElement)
    Dim pos = MapLayer.GetPosition(pin)
    mapLegend.Visibility = Visibility.Visible

    Dim grouping = DirectCast(pin.Tag, ShipGroupingModel)

    If (grouping.Ships.Count > 1) Then

      _textLegend = New List(Of Tuple(Of String, String)) From {
        Add("LeadingLocation", $"{grouping.Location}"),
        Add("Grouping Of Highest ShipType", $"{grouping.ShipType}"),
        Add("MULTIPLE", $"SHIPS {grouping.Ships.Count}")
      }
    Else
      _textLegend = New List(Of Tuple(Of String, String)) From {
        Add("Location", $"{grouping.Location}"),
        Add("ShipType", $"{grouping.ShipType}")
      }

      AddDetailsToLegend(grouping)
    End If

    AddTextBoxes()
  End Sub

  Private Sub AddDetailsToLegend(grouping As ShipGroupingModel)
    For i = 1 To grouping.Ships.Count
      Dim currentShip = grouping.Ships(i - 1)
      Add(String.Empty, String.Empty)
      _textLegend.Add(Add("MMSI", $"{currentShip.MMSI}"))
      _textLegend.Add(Add("ShipName", $"{currentShip.ShipName}"))
    Next
  End Sub

  Private Function Add(item1 As String, item2 As String) As Tuple(Of String, String)
    Return New Tuple(Of String, String)(item1, item2)
  End Function

  Private Sub AddTextBoxes()
    Dim brushToUse = CType(Application.Current.Resources("brush.Foreground.MainBoard"), LinearGradientBrush)
    _textLegend.ToList().ForEach(Sub(x) eventsPanel.Children.Add(New TextBlock With {.Text = $"{x.Item1} {x.Item2}", .Foreground = brushToUse, .FontSize = 12}))
  End Sub

  Private Sub Pushpin_MouseLeave(sender As Object, e As MouseEventArgs)
    eventsPanel.Children.Clear()
    mapLegend.Visibility = Visibility.Collapsed
  End Sub

  Public Sub SetDistanceThreshold(zoomLevel As Integer)
    If bingMap.ZoomLevel = 0 Then Exit Sub

    Dim oldLocation = bingMap.ViewportPointToLocation(New Point(0, 0))
    Dim newLocation = bingMap.ViewportPointToLocation(New Point((20 - zoomLevel) * 2, 0))
    Dim Dist = oldLocation.DistanceTo(newLocation, DistanceUnit.Miles)

    TryCast(DataContext, ChartingDashboardViewModel).DistanceThreshold = Dist
  End Sub
End Class
