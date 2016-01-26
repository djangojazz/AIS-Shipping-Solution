Imports System.Timers
Imports System.Collections.ObjectModel
Imports Microsoft.Maps.MapControl.WPF

<NotifyPropertyChanged>
Public Class ChartingDashboardViewModel
  Inherits BaseViewModel
  Implements IDisposable

  'VARIABLES
  Private _oldFiltered As List(Of Integer) = New List(Of Integer)
  Private _timers As New List(Of Timer)


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

  <SafeForDependencyAnalysis>
  Public ReadOnly Property ContentHeight As Double
    Get
      Return MySettings.Default.Height * MySettings.Default.ContentHeightPercent
    End Get
  End Property

  <SafeForDependencyAnalysis>
  Public ReadOnly Property DatagridWidth() As Double
    Get
      Return MySettings.Default.Width * MySettings.Default.GridWidthPercent
    End Get
  End Property


  'METHODS
  Private Function RefreshShipsAndResetMap() As Task
    Dim otask = Task.Factory.StartNew(Sub()
                                        ShipLocations = New ObservableCollection(Of ShipModel)(New ShipsService().TestLoadShipLocations())
                                        LocationRectangle = New ShipsService().GetRectangleOfLocation(ShipLocations, MySettings.Default.Padding)
                                      End Sub)
    Return otask
  End Function

  Private Sub FilterRefreshShips()
    If ShipLocations.Count >= MySettings.Default.PagingSize Then
      If (ShipLocations.Count <> _oldFiltered.Count) Then
        ObtainFilteredShips()
      Else
        _oldFiltered.Clear()
        ObtainFilteredShips()
      End If
    End If
  End Sub

  Private Sub ObtainFilteredShips()
    ShipLocationsFiltered = New ObservableCollection(Of ShipModel)(_ShipLocations.Where(Function(x) Not _oldFiltered.Contains(x.MMSI)).Take(MySettings.Default.PagingSize))
    ShipLocationsFiltered.ToList().ForEach(Sub(x) _oldFiltered.Add(x.MMSI))
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