Imports System.Timers
Imports System.Collections.ObjectModel
Imports Microsoft.Maps.MapControl.WPF

Public Class ChartingDashboardViewModel
  Inherits BaseViewModel

  Private _shipLocations As ObservableCollection(Of ShipModel)
  Private _center As Location

  Public Property ShipLocations() As ObservableCollection(Of ShipModel)
    Get
      Return _shipLocations
    End Get
    Set
      _shipLocations = Value
      OnPropertyChanged(NameOf(ShipLocations))
    End Set
  End Property

  Private _locationAddress As String
  Public Property LocationAddress() As String
    Get
      Return _locationAddress
    End Get
    Set(ByVal value As String)
      _locationAddress = value
      OnPropertyChanged(NameOf(LocationAddress))
    End Set
  End Property

  Public Sub New()
    RefreshShips()
    ShipRefreshFrequency(3000)
  End Sub

  Private Async Sub ShipRefreshFrequency(refreshDuration As Integer)
    Dim timer As New Timer(refreshDuration)
    AddHandler timer.Elapsed, AddressOf RefreshShips
    timer.Enabled = True
  End Sub

  Private Async Function RefreshShips() As Task
    ShipLocations = New ObservableCollection(Of ShipModel)(Await New ShipsService().LoadShipLocations())
  End Function

End Class