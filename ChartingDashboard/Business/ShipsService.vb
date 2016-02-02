Imports System.Linq
Imports Microsoft.Maps.MapControl.WPF

Public Class ShipsService
  Public Function TestLoadShipLocations() As IList(Of ShipModel)
    Dim dBships = DataAccess.DataConverter.ConvertTo(Of ShipDb)(New DataAccess.SQLTalker().GetData("EXEC Ships.pShipsMockService 's', 10000"))
    Return dBships.Select(Function(x) New ShipModel With
                          {
                          .MMSI = x.MMSI,
                          .ShipName = x.ShipName,
                          .ShipType = DirectCast(x.ShipTypeId, ShipType),
                          .Location = New Location() With {.Latitude = x.Latitude, .Longitude = x.Longitude}
                          }).ToList()

    'Return {
    '  MakeABoat(1, "Brett Home", ShipType.Owned, 45.457302, -122.754326),
    '  MakeABoat(2, "Thai Roses", ShipType.Other, 45.486155, -122.747739),
    '  MakeABoat(3, "Seattle", ShipType.Other, 47.6149942, -122.4759882)
    '}
    'MakeABoat(2, "Thai Roses", ShipType.Other, 45.486155, -122.747739)
    'MakeABoat(3, "Seattle", ShipType.Other, 47.6149942, -122.4759882)


  End Function

  Private Shared Function MakeABoat(mmsi As Integer, shipName As String, shipType As ShipType, latitude As Double, longitude As Double) As ShipModel
    Return New ShipModel With {
                              .MMSI = mmsi,
                              .ShipName = shipName,
                              .ShipType = shipType,
                              .Location = New Location() With {.Latitude = latitude, .Longitude = longitude}
                              }
  End Function
End Class