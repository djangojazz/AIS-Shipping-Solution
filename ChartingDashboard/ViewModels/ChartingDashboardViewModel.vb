Imports System.Timers
Imports System.Collections.ObjectModel
Imports Microsoft.Maps.MapControl.WPF

Public Class ChartingDashboardViewModel
  Inherits BaseViewModel

  Private _shipLocations As ObservableCollection(Of ShipModel)
  Private _locationRectangle As LocationRect

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

  Public Sub New()
    RefreshShipsAndResetMap()
    ShipRefreshFrequency(3000)
  End Sub

  Private Async Sub ShipRefreshFrequency(refreshDuration As Integer)
    Dim timer As New Timer(refreshDuration)
    AddHandler timer.Elapsed, AddressOf RefreshShipsAndResetMap
    timer.Enabled = True
  End Sub

  Private Async Function RefreshShipsAndResetMap() As Task
    ShipLocations = New ObservableCollection(Of ShipModel)(Await New ShipsService().LoadShipLocations())
    LocationRectangle = Await New ShipsService().GetRectangleOfLocation(ShipLocations, MySettings.Default.Padding)
    'GeocodeResult = Await New ShipsService().GetAverageLocation(ShipLocations)
  End Function

End Class