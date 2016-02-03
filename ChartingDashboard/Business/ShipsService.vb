Imports System.Linq
Imports Microsoft.Maps.MapControl.WPF

Public Class ShipsService
  Public Function TestLoadShipLocations() As IList(Of ShipModel)
    'Return {
    'MakeABoat(111111111, "Anne Sleuth", ShipType.Owned, 46.851859, -129.322418),
    'MakeABoat(111112211, "Bon Voyage", ShipType.Contractor, 47.871859, -130.322418),
    'MakeABoat(367197230, "Buck & Ann", ShipType.Owned, 46.451859, -124.322418),
    'MakeABoat(371321345, "Chrimeney Jeeps", ShipType.Owned, 51.421869, -135.322418),
    'MakeABoat(381321345, "Chappy Pappy", ShipType.Other, 51.456469, -131.135418),
    'MakeABoat(371341345, "Charlies Cheepo", ShipType.Other, 40.456469, -126.155418),
    'MakeABoat(384841345, "Dudes Dud", ShipType.Other, 43.756854, -124.456789),
    'MakeABoat(397541345, "Earles Agony", ShipType.Other, 43.897445, -124.557781),
    'MakeABoat(471233478, "Franks SingleFleet", ShipType.Contractor, 45.114445, -127.654811),
    'MakeABoat(124835678, "Forget Flying", ShipType.Other, 40.458123, -123.551451),
    'MakeABoat(813545664, "Genas Sinker", ShipType.Other, 46.124545, -127.654811),
    'MakeABoat(876541242, "Hectors Habit", ShipType.Contractor, 40.114445, -127.774811),
    'MakeABoat(991233478, "Icant Float", ShipType.Other, 45.548123, -129.811451),
    'MakeABoat(871233478, "Jacks Rwild", ShipType.Other, 42.712387, -133.887451),
    'MakeABoat(888233478, "Kelly CanUStop", ShipType.Contractor, 41.112387, -128.457451),
    'MakeABoat(678433478, "Larry Leaper", ShipType.Other, 40.512387, -125.877451),
    'MakeABoat(367150410, "Major Major", ShipType.Other, 46.333302, -124.116333),
    'MakeABoat(387150410, "Neato Torpedo", ShipType.Other, 47.333302, -124.056333),
    'MakeABoat(487150410, "October Ocean", ShipType.Other, 48.333302, -126.556333),
    'MakeABoat(587230410, "Pauls PushingIt", ShipType.Other, 46.583302, -127.586333),
    'MakeABoat(467230410, "Quest4 Johnny", ShipType.Other, 48.183302, -128.686333),
    'MakeABoat(547230410, "Roger DodgerOver", ShipType.Other, 49.283302, -124.786333),
    'MakeABoat(847251152, "Sarah Sleeting", ShipType.Contractor, 48.285102, -127.586333),
    'MakeABoat(947252151, "Tom And Jerry", ShipType.Other, 39.285102, -128.456333),
    'MakeABoat(101252151, "Ugotta B KiddingMe", ShipType.Other, 30.285102, -128.452233),
    'MakeABoat(132452151, "Versions Unknown", ShipType.Other, 33.285102, -140.454233),
    'MakeABoat(242452151, "Wild For The Sea", ShipType.Other, 40.518102, -145.334233),
    'MakeABoat(587452151, "X The Racer", ShipType.Contractor, 45.518102, -124.344233),
    'MakeABoat(681452151, "Yes I can", ShipType.Other, 40.418202, -123.486233)
    '}

    Dim dBships = DataAccess.DataConverter.ConvertTo(Of ShipDb)(New DataAccess.SQLTalker().GetData("EXEC Ships.pShipsMockService 's', 100000"))
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