Imports System.Timers
Imports System.Collections.ObjectModel
Imports Microsoft.Maps.MapControl.WPF
Imports System.Linq
Imports System.ComponentModel

<NotifyPropertyChanged>
Public Class ChartingDashboardViewModel
  Inherits BaseViewModel
  Implements IDisposable

  'VARIABLES
  Private _totalFilteredCount As Integer

  Private _pagingMemoryOfFilteredShips As List(Of Integer) = New List(Of Integer)
  Private _acceptableShips As ShipType() = {ShipType.Owned, ShipType.Contractor}
  Private _ships As List(Of ShipModel)
  Private _initialized As Boolean
  Private TimerRefresh As Timer
  Private TimerFilter As Timer

  'CONSTRUCTOR
  Public Sub New()
    TimerRefresh = TimerHelper(TimeSpan.FromSeconds(0.5).TotalMilliseconds, Sub() RefreshShipsAndResetMap())
    TimerFilter = TimerHelper(TimeSpan.FromSeconds(0.5).TotalMilliseconds, Sub() FilterRefreshShips())

    RefreshShipsAndResetMap()
    FilterRefreshShips()
    TimerRefresh.Interval = 500
    TimerFilter.Interval = 500
  End Sub

  'PROPERTY
  Public Property LocationRectangle As LocationRect

  Public Property ShipLocations As ObservableCollection(Of ShipModel)

  Public Property ShipLocationsFiltered As ObservableCollection(Of ShipModel)

  Public Property Dimension As Double

  Public Property RefreshInstance As Integer

  Public Property DistanceThreshold As Double

  Private _zoomLevel As Integer

  Public Property ZoomLevel As Integer
    Get
      Return _zoomLevel
    End Get
    Set(ByVal value As Integer)
      _zoomLevel = value
      'When zoom changes we need to realign all ships dynamically
      Dimension = _zoomLevel * 15
      'Threading.Thread.Sleep(500)
      'RetrieveShipsAndDetermineCollision()
    End Set
  End Property


  <SafeForDependencyAnalysis>
  Public ReadOnly Property MapContentHeight As Double
    Get
      Return MySettings.Default.Height * (1 - 0.18 - MySettings.Default.MarqueeContentHeightPercentage)
    End Get
  End Property

  <SafeForDependencyAnalysis>
  Public ReadOnly Property MarqueeContentHeight As Double
    Get
      Return (MySettings.Default.Height * MySettings.Default.MarqueeContentHeightPercentage)
    End Get
  End Property


  'METHODS
  Private Sub RefreshShipsAndResetMap()
    TimerRefresh.Stop()
    TimerRefresh.Interval = TimeSpan.FromSeconds(MySettings.Default.MapRefreshFrequencyInSeconds).TotalMilliseconds
    RetrieveShipsAndDetermineCollision()
    LocationRectangle = GetRectangleOfLocation(ShipLocations, MySettings.Default.Padding)

    'This is happening so fast it made not be necessary to show it.
    'If (RefreshInstance < 1) Then
    '  ErrorMessage = "LOADING INITIAL MAP VALUES"
    'Else
    '  If ErrorMessage = "LOADING INITIAL MAP VALUES" Then
    '    ErrorMessage = String.Empty
    '  End If
    'End If

    RefreshInstance += 1
    TimerRefresh.Start()
  End Sub

  Private Sub FilterRefreshShips()
    TimerFilter.Stop()
    TimerFilter.Interval = TimeSpan.FromSeconds(MySettings.Default.DetailsRefreshFrequencyInSeconds).TotalMilliseconds
    If (ShipLocations?.Count > 0) Then
      If (_totalFilteredCount <> _pagingMemoryOfFilteredShips?.Count) Then
        ObtainFilteredShips()
      Else
        _pagingMemoryOfFilteredShips.Clear()
        ObtainFilteredShips()
      End If
    End If
    TimerFilter.Start()
  End Sub

  Private Sub ObtainFilteredShips()

    Dim totalsToFilter = New List(Of ShipModel)(ShipLocations.Where(Function(x) _acceptableShips.Contains(x.ShipType)))
    _totalFilteredCount = totalsToFilter.Count

    ShipLocationsFiltered = New ObservableCollection(Of ShipModel)(totalsToFilter.Where(Function(x) Not _pagingMemoryOfFilteredShips.Contains(x.MMSI)).Take(MySettings.Default.PagingSize))
    ShipLocationsFiltered.ToList().ForEach(Sub(x) _pagingMemoryOfFilteredShips.Add(x.MMSI))
  End Sub

  Private Sub RetrieveShipsAndDetermineCollision()
    _ships = New ShipsService().TestLoadShipLocations().ToList()
    UpdateShipsInformation()

    ShipLocations = New ObservableCollection(Of ShipModel)(_ships)
  End Sub

  Private Sub UpdateShipsInformation()
    If (_ships?.Count > 0 AndAlso DistanceThreshold > 0) Then
      'Dim shipGroupingModels = New List(Of ShipGroupingModel)
      'Dim maxGroupFromShips As Func(Of Integer) = Function() _ships.ToList().Select(Function(X) X.Group).ToList().OrderByDescending(Function(x) x).FirstOrDefault()
      'Dim shipGroupAlreadyExists As Func(Of ShipModel, Boolean) = Function(x) shipGroupingModels.Select(Function(y) y.Ships).ToList().Exists(Function(x) x.)

      'Dim CollectionToEmpty As New Collection(Of ShipModel)(_ships)
      ''Dim iCurrentIteration As Integer = 1
      'Do While CollectionToEmpty.Count > 0
      '  Dim currentGroup As New ShipGroupingModel With {.Ships = New List(Of ShipModel)} 'Just do the first Lat Long instead of a Key
      '  'With { .Group = iCurrentIteration}
      '  currentGroup.Ships.Add(CollectionToEmpty(0))
      '  CollectionToEmpty.RemoveAt(0)

      '  For i As Integer = CollectionToEmpty.Count - 1 To 0 Step -1
      '    If DetectCollision(CollectionToEmpty(0).Location, CollectionToEmpty(i).Location) Then
      '      currentGroup.Ships.Add(CollectionToEmpty(i))
      '      CollectionToEmpty.RemoveAt(i)
      '    End If
      '  Next
      'Loop

      For Each ship In _ships

        _ships.Where(Function(x) x IsNot ship).ToList() _
          .ForEach(Sub(x)
                     Dim locationsCollide = DetectCollision(ship.Location, x.Location)
                     If (locationsCollide) Then
                       ship.Collision = True
                       x.Collision = True
                     End If
                   End Sub)
      Next
    End If



    'ErrorMessage = $"Ran UpdateShipsInformation {DateTime.Now.ToString} {_ships(0).Collision} {RefreshInstance.ToString}"
  End Sub

  Private Function DetectCollision(loc1 As Location, loc2 As Location) As Boolean
    Dim milesDistanceBetweenPoints = loc1.DistanceTo(loc2, DistanceUnit.Miles)

    Return ((DistanceThreshold * 2) > milesDistanceBetweenPoints)
  End Function

#Region "Disposing"
  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overridable Sub Dispose(disposing As Boolean)
    If disposing Then
      If Not IsNothing(TimerRefresh) Then TimerRefresh.Dispose()
      If Not IsNothing(TimerFilter) Then TimerFilter.Dispose()
      ErrorMessage = Nothing
      LocationRectangle = Nothing
      ShipLocations = Nothing
      ShipLocationsFiltered = Nothing
    End If
  End Sub
#End Region
End Class