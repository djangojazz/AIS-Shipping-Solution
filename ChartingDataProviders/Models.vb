Public Class ShipDb
  Public Property ShipId As Integer
  Public Property MMSI As Integer
  Public Property ShipName As String
  Public Property Latitude As Double
  Public Property Longitude As Double
  Public Property ShipTypeId As Integer
End Class

Public Class ShipVolumeDb
  Public Property ShipVolumeId As Integer
  Public Property ShipId As Integer
  Public Property BoatHale As Double
  Public Property ExpectedVolume As Double
  Public Property CatchTypeID As Integer
End Class