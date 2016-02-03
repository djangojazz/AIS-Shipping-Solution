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
  Private _refreshInstance As Integer

  'CONSTRUCTOR
  Public Sub New()
    TimerRefresh = TimerHelper(TimeSpan.FromSeconds(0.5).TotalMilliseconds, Sub() RefreshShipsAndResetMap())
    TimerFilter = TimerHelper(TimeSpan.FromSeconds(0.5).TotalMilliseconds, Sub() FilterRefreshShips())
  End Sub

  'PROPERTY
  Public Property LocationRectangle As LocationRect

  Public Property ShipLocations As ObservableCollection(Of ShipGroupingModel)

  Public Property ShipLocationsFiltered As ObservableCollection(Of ShipModel)

  Public Property Dimension As Double

  Public Property DistanceThreshold As Double

  Private _zoomLevel As Integer

  Public Property ZoomLevel As Integer
    Get
      Return _zoomLevel
    End Get
    Set(ByVal value As Integer)
      If (_zoomLevel <> value) Then
        _zoomLevel = value
        Dimension = _zoomLevel * 15
        RetrieveShipsAndDetermineCollision()
      End If

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
    If (_refreshInstance > 1) Then
      TimerRefresh.Interval = TimeSpan.FromSeconds(MySettings.Default.MapRefreshFrequencyInSeconds).TotalMilliseconds
    End If

    RetrieveShipsAndDetermineCollision()
    LocationRectangle = GetRectangleOfLocation(ShipLocations.SelectMany(Function(x) x.Ships).ToList(), MySettings.Default.Padding)

    'This is happening so fast it made not be necessary to show it.
    'If (RefreshInstance < 1) Then
    '  ErrorMessage = "LOADING INITIAL MAP VALUES"
    'Else
    '  If ErrorMessage = "LOADING INITIAL MAP VALUES" Then
    '    ErrorMessage = String.Empty
    '  End If
    'End If

    _refreshInstance += 1
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

    Dim totalsToFilter = New List(Of ShipModel)(ShipLocations.SelectMany(Function(x) x.Ships).Where(Function(x) _acceptableShips.Contains(x.ShipType)))
    _totalFilteredCount = totalsToFilter.Count

    ShipLocationsFiltered = New ObservableCollection(Of ShipModel)(totalsToFilter.Where(Function(x) Not _pagingMemoryOfFilteredShips.Contains(x.MMSI)).Take(MySettings.Default.PagingSize))
    ShipLocationsFiltered.ToList().ForEach(Sub(x) _pagingMemoryOfFilteredShips.Add(x.MMSI))
  End Sub

  Private Sub RetrieveShipsAndDetermineCollision()
    _ships = New ShipsService().TestLoadShipLocations().ToList()


    If (_ships?.Count > 0) Then
      Dim ReturnPriorityBoat As Func(Of ShipModel, ShipModel, ShipModel) = Function(x, y) If(x.ShipType <= y.ShipType, x, y)
      Dim groupings = New List(Of ShipGroupingModel)

      Dim CollectionToEmpty = _ships.ToList()
      Do While CollectionToEmpty.Count > 0
        Dim currentShip = CollectionToEmpty(0)
        Dim currentGroup As New ShipGroupingModel With {.Location = currentShip.Location, .ShipType = currentShip.ShipType, .Ships = New List(Of ShipModel)({currentShip})}
        CollectionToEmpty.RemoveAt(0)

        For i As Integer = CollectionToEmpty.Count - 1 To 0 Step -1
          Dim shipToCompare = CollectionToEmpty(i)
          If DetectCollision(currentShip.Location, shipToCompare.Location) Then
            If (currentGroup.ShipType > shipToCompare.ShipType) Then
              Dim priorityShip = ReturnPriorityBoat(currentShip, shipToCompare)
              currentGroup.Location = priorityShip.Location
              currentGroup.ShipType = priorityShip.ShipType
            End If

            currentGroup.Ships.Add(shipToCompare)
            CollectionToEmpty.RemoveAt(i)
          End If
        Next

        groupings.Add(currentGroup)
      Loop

      ShipLocations = New ObservableCollection(Of ShipGroupingModel)(groupings)
      'For Each ship In _ships

      '  _ships.Where(Function(x) x IsNot ship).ToList() _
      '    .ForEach(Sub(x)
      '               Dim locationsCollide = DetectCollision(ship.Location, x.Location)
      '               If (locationsCollide) Then
      '                 ship.Collision = True
      '                 x.Collision = True
      '               End If
      '             End Sub)
      'Next
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