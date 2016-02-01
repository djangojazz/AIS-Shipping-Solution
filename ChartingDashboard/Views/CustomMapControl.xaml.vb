Imports System.Threading
Imports System.Windows.Threading
Imports Microsoft.Maps.MapControl.WPF

Public Class CustomMapControl

  Dim _textDictionary As Dictionary(Of String, String)
  '  Dim _parentDataContext As ChartingDashboardViewModel
  Dim _timerDispatcher As System.Windows.Threading.DispatcherTimer
  Dim _initialized As Boolean

  Public Sub New()
    InitializeComponent()

    '   _timerDispatcher = New DispatcherTimer(TimeSpan.FromSeconds(0), DispatcherPriority.ApplicationIdle, AddressOf Map_ViewChangeOnFrame, System.Windows.Application.Current.Dispatcher)
  End Sub

  'Private Sub Map_ViewChangeOnFrame(ByVal sender As Object, ByVal e As EventArgs)
  '  '  _parentDataContext = TryCast(DataContext, ChartingDashboardViewModel)
  '  SetDistanceThreshold(TryCast(DataContext, ChartingDashboardViewModel).ShipLocations.FirstOrDefault()?.Location)
  '  _timerDispatcher.Stop()
  '  _timerDispatcher = Nothing
  'End Sub

  Private Sub Map_ViewChangeOnFrame(ByVal sender As Object, ByVal e As MapEventArgs)
    '  _parentDataContext = TryCast(DataContext, ChartingDashboardViewModel)

    If _initialized Then
      TryCast(DataContext, ChartingDashboardViewModel).Visibility = Visibility.Visible

      'Only the first Location is needed as we just need the radius of one point and the rest are uniform with their field distances
      SetDistanceThreshold(TryCast(DataContext, ChartingDashboardViewModel).ShipLocations.FirstOrDefault()?.Location)

      'Dim thaiRoses = _parentDataContext.ShipLocations.FirstOrDefault(Function(x) x.MMSI = 2).Location
      'Dim seattle = dc.ShipLocations.FirstOrDefault(Function(x) x.MMSI = 3).Location

      'Dim collision = _parentDataContext.LocationsCollide(myHome, thaiRoses)

      '_textDictionary = New Dictionary(Of String, String) From {{"COLLISION", String.Empty}} '{"Collision:", $"{collision}"}

      eventsPanel.Children.Clear()
      AddTextBoxes()
    End If


    _initialized = True


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

  Public Sub SetDistanceThreshold(loc1 As Location)
    If map.ZoomLevel = 0 Then Exit Sub
    Dim point1 = New Point

    map.TryLocationToViewportPoint(loc1, point1)
    'I only need to get a buffer zone for the radius once as both locations should have the same field for reference
    Dim newLocation = map.ViewportPointToLocation(New Point(point1.X + (map.ZoomLevel * 15 / 2), point1.Y))

    Dim Dist = loc1.DistanceTo(newLocation, DistanceUnit.Miles)

    TryCast(DataContext, ChartingDashboardViewModel).DistanceThreshold = Dist
  End Sub

  Dim TaskToRunInOneSecond As Task
  Dim CancelToken As CancellationTokenSource

  Private Sub SetDistance(Distance As Double)


    If Not IsNothing(CancelToken) Then CancelToken.Cancel()
    CancelToken = New CancellationTokenSource
    Dim ct = CancelToken.Token
    Task.Factory.StartNew(Sub()
                            Threading.Thread.Sleep(1000)
                            If ct.IsCancellationRequested Then Exit Sub
                            TryCast(DataContext, ChartingDashboardViewModel).DistanceThreshold = Distance
                          End Sub, ct)
  End Sub

End Class
