Imports System.Timers
Imports System.Collections.ObjectModel
Imports Microsoft.Maps.MapControl.WPF
Imports System.Windows.Threading

<NotifyPropertyChanged>
Public Class ChartingDashboardViewModel
  Inherits BaseViewModel
  Implements IDisposable

  'VARIABLES
  Private _totalFilteredCount As Integer
  Private _pagingMemoryOfFilteredShips As List(Of Integer) = New List(Of Integer)
  Private _acceptableShips As ShipType() = {ShipType.Owned, ShipType.Contractor}
  Private _timerRefresh As Timer
  Private _timerFilter As Timer
  Private _refreshInstance As Integer
  Private _Dispatcher As Dispatcher

  'CONSTRUCTOR
  Public Sub New()
    _timerRefresh = TimerHelper(TimeSpan.FromSeconds(0.5).TotalMilliseconds, Sub() RefreshShipsAndResetMap())
    _timerFilter = TimerHelper(TimeSpan.FromSeconds(MySettings.Default.DetailsRefreshFrequencyInSeconds).TotalMilliseconds, Sub() FilterRefreshShips())

    _Dispatcher = System.Windows.Application.Current.Dispatcher
  End Sub

  'PROPERTY
  Public Property LocationRectangle As LocationRect

  Public Property ShipLocations As ObservableCollection(Of ShipGroupingModel)

  Public Property ShipLocationsFiltered As ObservableCollection(Of ShipModel)

  Public Property Dimension As Double

  Public Property DistanceThreshold As Double

  Public Property OwnedShipsMarquee As CustomMarqueeViewModel

  Public Property ContractorShipsMarquee As CustomMarqueeViewModel

  Public Property ItemWidth As Double

  Private _zoomLevel As Integer

  Public Property ZoomLevel As Integer
    Get
      Return _zoomLevel
    End Get
    Set(ByVal value As Integer)
      If (_zoomLevel <> value) Then
        _zoomLevel = value
        Dimension = _zoomLevel * 15
        ShipLocations = New ObservableCollection(Of ShipGroupingModel)(RetrieveShipsAndDetermineCollision(TestLoadShipLocations().ToList(), DistanceThreshold))
      End If
    End Set
  End Property

  'METHODS
  Private Sub RefreshShipsAndResetMap()
    _timerRefresh.Stop()
    If (_refreshInstance > 1) Then
      _timerRefresh.Interval = TimeSpan.FromSeconds(MySettings.Default.MapRefreshFrequencyInSeconds).TotalMilliseconds
    End If

    ShipLocations = New ObservableCollection(Of ShipGroupingModel)(RetrieveShipsAndDetermineCollision(TestLoadShipLocations().ToList(), DistanceThreshold))
    LocationRectangle = GetRectangleOfLocation(ShipLocations.SelectMany(Function(x) x.Ships).ToList(), MySettings.Default.Padding)
    FilterRefreshShips()

    'This is happening so fast it made not be necessary to show it.
    'If (RefreshInstance < 1) Then
    '  ErrorMessage = "LOADING INITIAL MAP VALUES"
    'Else
    '  If ErrorMessage = "LOADING INITIAL MAP VALUES" Then
    '    ErrorMessage = String.Empty
    '  End If
    'End If

    _refreshInstance += 1
    _timerRefresh.Start()
  End Sub

  Private Sub FilterRefreshShips()
    _timerFilter.Stop()
    If (ShipLocations?.Count > 0) Then
      If (_totalFilteredCount <> _pagingMemoryOfFilteredShips?.Count) Then
        ObtainFilteredShips()
      Else
        _pagingMemoryOfFilteredShips.Clear()
        ObtainFilteredShips()
      End If
    End If
    _timerFilter.Start()
  End Sub

  Private Sub ObtainFilteredShips()

    Dim totalsToFilter = New List(Of ShipModel)(ShipLocations.SelectMany(Function(x) x.Ships).Where(Function(x) _acceptableShips.Contains(x.ShipType)))
    _totalFilteredCount = totalsToFilter.Count

    Dim pagingSize = CInt(MySettings.Default.Width / 250)

    ShipLocationsFiltered = New ObservableCollection(Of ShipModel)(totalsToFilter.Where(Function(x) Not _pagingMemoryOfFilteredShips.Contains(x.MMSI)).Take(pagingSize))
    ShipLocationsFiltered.ToList().ForEach(Sub(x) _pagingMemoryOfFilteredShips.Add(x.MMSI))
  End Sub


#Region "Disposing"
  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overridable Sub Dispose(disposing As Boolean)
    If disposing Then
      If Not IsNothing(_timerRefresh) Then _timerRefresh.Dispose()
      If Not IsNothing(_timerFilter) Then _timerFilter.Dispose()
      ErrorMessage = Nothing
      LocationRectangle = Nothing
      ShipLocations = Nothing
      ShipLocationsFiltered = Nothing
      OwnedShipsMarquee = Nothing
      ContractorShipsMarquee = Nothing
    End If
  End Sub
#End Region
End Class