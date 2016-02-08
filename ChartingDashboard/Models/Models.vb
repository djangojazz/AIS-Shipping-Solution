Imports Microsoft.Maps.MapControl.WPF

Public Class ShipDb
  Public Property ShipId As Integer
  Public Property MMSI As Integer
  Public Property ShipName As String
  Public Property Latitude As Double
  Public Property Longitude As Double
  Public Property ShipTypeId As Integer
  Public Property ShipVolumeId As Integer
  Public Property BoatHale As Double
  Public Property ExpectedVolume As Double
  Public Property CatchTypeID As Integer
End Class

Public Class ShipGroupingModel
  Public Property Location As Location
  Public Property ShipType As ShipType
  Public Property Ships As IList(Of ShipModel)
End Class

Public Class ShipModel
  Public Property MMSI As Integer
  Public Property ShipName As String
  Public Property ShipType As ShipType
  Public Property Location As Location
  Public Property Volumes As IList(Of ShipVolume)
End Class

Public Class ShipVolume
  Public Property BoatHale As Double
  Public Property ExpectedVolume As Double
  Public Property CatchType As CatchType
End Class