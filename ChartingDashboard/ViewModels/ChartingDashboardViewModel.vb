﻿Imports System.Timers
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
      Dimension = _zoomLevel * 15
    End Set
  End Property


  <SafeForDependencyAnalysis>
  Public ReadOnly Property MapContentHeight As Double
    Get
      Return (MySettings.Default.Height * MySettings.Default.MapContentHeightPercent)
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

    If (RefreshInstance < 1) Then
      ErrorMessage = "LOADING INITIAL MAP VALUES"
    End If

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
    If (_ships?.Count > 0) Then
      For i = 0 To _ships.Count - 1
        Dim shipToCompare = _ships(i)

        _ships.Where(Function(x) x IsNot shipToCompare).ToList() _
          .ForEach(Sub(x)
                     Dim locationsCollide = DetectCollision(shipToCompare.Location, x.Location)
                     If (locationsCollide) Then
                       shipToCompare.Collision = True
                       x.Collision = True
                     End If
                   End Sub)
      Next
    End If

    ErrorMessage = $"Ran UpdateShipsInformation {DateTime.Now.ToString} {_ships(0).Collision} {RefreshInstance.ToString}"
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