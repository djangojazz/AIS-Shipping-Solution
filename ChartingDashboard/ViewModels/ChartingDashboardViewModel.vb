Imports System.Timers
Imports System.Collections.ObjectModel
Imports Microsoft.Maps.MapControl.WPF

Public Class ChartingDashboardViewModel
  Inherits BaseViewModel

  Private _shipLocations As ObservableCollection(Of ShipModel)
  Private _center As Location
  Private _locationAddress As String
  Private _geocodeResult As GeocodeService.GeocodeResult
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

  Public Property GeocodeResult() As GeocodeService.GeocodeResult
    Get
      Return _geocodeResult
    End Get
    Set
      _geocodeResult = Value
      OnPropertyChanged(NameOf(GeocodeResult))
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

  Public Property LocationAddress() As String
    Get
      Return _locationAddress
    End Get
    Set
      _locationAddress = Value
      OnPropertyChanged(NameOf(LocationAddress))
    End Set
  End Property

  Public Sub New()
    RefreshShips()
    ShipRefreshFrequency(3000)
  End Sub

  Private Async Sub ShipRefreshFrequency(refreshDuration As Integer)
    Dim timer As New Timer(refreshDuration)
    AddHandler timer.Elapsed, AddressOf RefreshShipsAndResetMap
    timer.Enabled = True
  End Sub

  Private Async Function RefreshShips() As Task(Of IList(Of ShipModel))
    ShipLocations = New ObservableCollection(Of ShipModel)(Await New ShipsService().LoadShipLocations())
  End Function

  Private Async Function RefreshShipsAndResetMap() As Task
    ShipLocations = New ObservableCollection(Of ShipModel)(Await New ShipsService().LoadShipLocations())
    LocationRectangle = Await New ShipsService().GetRectangleOfLocation(ShipLocations)
    'GeocodeResult = Await New ShipsService().GetAverageLocation(ShipLocations)
  End Function

End Class