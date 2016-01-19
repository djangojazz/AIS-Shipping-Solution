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
      OnPropertyChanged("ShipLocations")
    End Set
  End Property

  Private _locationAddress As String
  Public Property LocationAddress() As String
    Get
      Return _locationAddress
    End Get
    Set(ByVal value As String)
      _locationAddress = value
      OnPropertyChanged("LocationAddress")
    End Set
  End Property


  Public Property Center As Location

    Get
      Return _center
    End Get
    Set(ByVal value As Location)
      _center = value
      OnPropertyChanged("Center")
    End Set
  End Property

  Public Sub New()
    Center = New Location With {.Latitude = -128, .Longitude = 48}
    'New MapCore With {.Center = New Location With {.Latitude = -123, .Longitude = 48}}
    ShipRefreshFrequency(3000)
  End Sub

  Private Async Sub ShipRefreshFrequency(refreshDuration As Integer)
    Dim timer As New Timer(refreshDuration)
    LocationAddress = "Test" + DateTime.Now
    'ShipLocations = New ObservableCollection(Of ShipModel)(Await New ShipsService().LoadShipLocations())
    timer.Enabled = True

  End Sub

End Class