Imports System.Timers
Imports System.Collections.ObjectModel
Imports Microsoft.Maps.MapControl.WPF

Public Class ChartingDashboardViewModel
  Inherits BaseViewModel
  Implements IDisposable

  Private _shipLocations As ObservableCollection(Of ShipModel)
  Private _shipLocationsfiltered As ObservableCollection(Of ShipModel)
  Private _oldFiltered As List(Of Integer) = New List(Of Integer)
  Private _locationRectangle As LocationRect
  Private _timers As New List(Of Timer)

  Public Property LocationRectangle() As LocationRect
    Get
      Return _locationRectangle
    End Get
    Set
      _locationRectangle = Value
      OnPropertyChanged(NameOf(LocationRectangle))
    End Set
  End Property

  Public Property ShipLocations() As ObservableCollection(Of ShipModel)
    Get
      Return _shipLocations
    End Get
    Set
      _shipLocations = Value
      OnPropertyChanged(NameOf(ShipLocations))
    End Set
  End Property

  Public Property ShipLocationsFiltered() As ObservableCollection(Of ShipModel)
    Get
      Return _shipLocationsfiltered
    End Get
    Set
      _shipLocationsfiltered = Value
      OnPropertyChanged(NameOf(ShipLocationsFiltered))
    End Set
  End Property

  Public Sub New()
    RefreshShipsAndResetMap()
    TimerHelper(6000, Function() RefreshShipsAndResetMap())
    FilterRefreshShips()
    TimerHelper(2000, Function() FilterRefreshShips())
  End Sub

  Private Async Function RefreshShipsAndResetMap() As Task
    ShipLocations = New ObservableCollection(Of ShipModel)(Await New ShipsService().LoadShipLocations())
    LocationRectangle = Await New ShipsService().GetRectangleOfLocation(ShipLocations, MySettings.Default.Padding)
  End Function

  Private Async Function FilterRefreshShips() As Task
    If ShipLocations.Count >= MySettings.Default.PagingSize Then
      ShipLocationsFiltered = New ObservableCollection(Of ShipModel)(_shipLocations.Where(Function(x) Not _oldFiltered.Contains(x.MMSI)).Take(MySettings.Default.PagingSize))
      If (ShipLocations.Count <> _oldFiltered.Count) Then
        ShipLocationsFiltered.ToList().ForEach(Sub(x) _oldFiltered.Add(x.MMSI))
      Else
        _oldFiltered.Clear()
      End If
    End If
  End Function

#Region "Disposing"
  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overridable Sub Dispose(disposing As Boolean)
    If disposing Then
      _timers.ForEach(Sub(x As Timer) x.Dispose())
    End If
  End Sub
#End Region
End Class