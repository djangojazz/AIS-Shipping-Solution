Imports System.Linq
Imports Microsoft.Maps.MapControl.WPF

Public Class ShipsService
  Public Function TestLoadShipLocations() As IList(Of ShipModel)
    'Dim dBships = DataAccess.DataConverter.ConvertTo(Of ShipDb)(New DataAccess.SQLTalker().GetData("EXEC Ships.pShipsMockService 's', 10000"))
    'Return dBships.Select(Function(x) New ShipModel With
    '                      {
    '                      .MMSI = x.MMSI,
    '                      .ShipName = x.ShipName,
    '                      .ShipType = DirectCast(x.ShipTypeId, ShipType),
    '                      .Location = New Location() With {.Latitude = x.Latitude, .Longitude = x.Longitude}
    '                      }).ToList()

    Return {New ShipModel With
                          {
                          .MMSI = 123456789,
                          .ShipName = "Bretts Barge",
                          .ShipType = ShipType.Contractor,
                          .Location = New Location() With {.Latitude = 45.457302, .Longitude = -122.754326}
                          }}
  End Function
End Class