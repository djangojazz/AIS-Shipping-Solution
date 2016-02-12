Imports System.Xml.Serialization

<Serializable>
Public Class ShipDb
  <XmlAttribute>
  Public Property ShipId As Integer
  <XmlAttribute>
  Public Property MMSI As Integer
  <XmlAttribute>
  Public Property ShipName As String
  <XmlAttribute>
  Public Property Latitude As Double
  <XmlAttribute>
  Public Property Longitude As Double
  <XmlAttribute>
  Public Property ShipTypeId As Integer
End Class

Public Class ShipVolumeDb
  Public Property ShipVolumeId As Integer
  Public Property ShipId As Integer
  Public Property BoatHale As Double
  Public Property ExpectedVolume As Double
  Public Property CatchTypeID As Integer
End Class