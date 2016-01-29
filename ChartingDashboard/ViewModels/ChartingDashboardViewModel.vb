Imports System.Timers
Imports System.Collections.ObjectModel
Imports Microsoft.Maps.MapControl.WPF

<NotifyPropertyChanged>
Public Class ChartingDashboardViewModel
  Inherits BaseViewModel
  Implements IDisposable

  'VARIABLES
  Private _totalFilteredCount As Integer
  Private _pagingMemoryOfFilteredShips As List(Of Integer) = New List(Of Integer)
  Private _timers As New List(Of Timer)
  Private _acceptableShips As ShipType() = {ShipType.Owned, ShipType.Contractor}

  'CONSTRUCTOR
  Public Sub New()
    RefreshShipsAndResetMap().Wait()
    TimerHelper((MySettings.Default.MapRefreshFrequencyInSeconds * 1000), Sub() RefreshShipsAndResetMap())
    FilterRefreshShips()
    TimerHelper((MySettings.Default.DetailsRefreshFrequencyInSeconds * 1000), Sub() FilterRefreshShips())
  End Sub


  'PROPERTY
  Public Property LocationRectangle As LocationRect

  Public Property ShipLocations As ObservableCollection(Of ShipModel)

  Public Property ShipLocationsFiltered As ObservableCollection(Of ShipModel)

  Public Property Dimension As Double

  <SafeForDependencyAnalysis>
  Public ReadOnly Property Color As LinearGradientBrush
    Get
      Return CType(Application.Current.Resources("brush.Foreground.BoatGradientOther"), LinearGradientBrush)
    End Get

  End Property

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
  Private Function RefreshShipsAndResetMap() As Task
    Dim otask = Task.Factory.StartNew(Sub()
                                        ShipLocations = New ObservableCollection(Of ShipModel)(New ShipsService().TestLoadShipLocations())
                                        LocationRectangle = MapHelpers.GetRectangleOfLocation(ShipLocations, MySettings.Default.Padding)
                                      End Sub)
    Return otask
  End Function

  Private Sub FilterRefreshShips()
    If (ShipLocations?.Count > 0) Then
      If (_totalFilteredCount <> _pagingMemoryOfFilteredShips?.Count) Then
        ObtainFilteredShips()
      Else
        _pagingMemoryOfFilteredShips.Clear()
        ObtainFilteredShips()
      End If
    End If
  End Sub

  Private Sub ObtainFilteredShips()
    Dim totalsToFilter = New List(Of ShipModel)(ShipLocations.Where(Function(x) _acceptableShips.Contains(x.ShipType)))
    _totalFilteredCount = totalsToFilter.Count

    ShipLocationsFiltered = New ObservableCollection(Of ShipModel)(totalsToFilter.Where(Function(x) Not _pagingMemoryOfFilteredShips.Contains(x.MMSI)).Take(MySettings.Default.PagingSize))
    ShipLocationsFiltered.ToList().ForEach(Sub(x) _pagingMemoryOfFilteredShips.Add(x.MMSI))
  End Sub


#Region "Disposing"
  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overridable Sub Dispose(disposing As Boolean)
    If disposing Then
      _timers.ForEach(Sub(x As Timer) x.Dispose())
      ErrorMessage = Nothing
      LocationRectangle = Nothing
      ShipLocations = Nothing
      ShipLocationsFiltered = Nothing
    End If
  End Sub
#End Region
End Class