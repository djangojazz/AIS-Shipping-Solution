Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports ChartingDataProviders
Imports ChartingService
Imports DataAccess
Imports Microsoft.Maps.MapControl.WPF

Public Module ShipsCommunications
  Public Function TestLoadShipLocations(increment As Double) As IList(Of ShipModel)
    Return {
    MakeABoat(111111111, "Anne Sleuth", ShipType.PacificSeafood, 46.851859 - increment, -129.322418 - increment,
              New List(Of ShipVolume)({
                                      New ShipVolume With {.BoatHale = 5000, .ExpectedVolume = 20000, .CatchType = CatchType.Salmon},
                                      New ShipVolume With {.BoatHale = 3500, .ExpectedVolume = 4000, .CatchType = CatchType.DoverSole},
                                      New ShipVolume With {.BoatHale = 7600, .ExpectedVolume = 8000, .CatchType = CatchType.Tuna}
                                      })),
    MakeABoat(111112211, "Bon Voyage", ShipType.Contractor, 47.871859 - increment, -130.322418 - increment),
    MakeABoat(367197230, "Buck & Ann", ShipType.PacificSeafood, 46.451859 - increment, -124.322418 - increment,
              New List(Of ShipVolume)({
                                      New ShipVolume With {.BoatHale = 500, .ExpectedVolume = 2000, .CatchType = CatchType.Shrimp}
                                      })),
    MakeABoat(371321345, "Chrimeney Jeeps", ShipType.PacificSeafood, 51.421869 - increment, -135.322418 - increment,
              New List(Of ShipVolume)({
                                      New ShipVolume With {.BoatHale = 4000, .ExpectedVolume = 20000, .CatchType = CatchType.Rockfish}
                                      })),
    MakeABoat(381321345, "Chappy Pappy", ShipType.Other, 51.456469, -131.135418 - increment),
    MakeABoat(371341345, "Charlies Cheepo", ShipType.Other, 40.456469, -126.155418 - increment),
    MakeABoat(384841345, "Dudes Dud", ShipType.Other, 43.756854, -124.456789 - increment),
    MakeABoat(397541345, "Earles Agony", ShipType.Other, 43.897445, -124.557781 - increment),
    MakeABoat(471233478, "Franks SingleFleet", ShipType.Contractor, 45.114445, -127.654811),
    MakeABoat(124835678, "Forget Flying", ShipType.Other, 40.458123, -123.551451 - increment),
    MakeABoat(813545664, "Genas Sinker", ShipType.Other, 46.124545, -127.654811 - increment),
    MakeABoat(876541242, "Hectors Habit", ShipType.Contractor, 40.114445, -127.774811 - increment),
    MakeABoat(991233478, "Icant Float", ShipType.Other, 45.548123 - increment, -129.811451 - increment),
    MakeABoat(871233478, "Jacks Rwild", ShipType.Other, 42.712387, -133.887451 - increment),
    MakeABoat(888233478, "Kelly CanUStop", ShipType.Contractor, 41.112387, -128.457451 - increment),
    MakeABoat(678433478, "Larry Leaper", ShipType.Other, 40.512387, -125.877451 - increment),
    MakeABoat(367150410, "Major Major", ShipType.Other, 46.333302, -124.116333 - increment),
    MakeABoat(387150410, "Neato Torpedo", ShipType.Other, 47.333302, -124.056333 - increment),
    MakeABoat(487150410, "October Ocean", ShipType.Other, 48.333302, -126.556333),
    MakeABoat(587230410, "Pauls PushingIt", ShipType.Other, 46.583302 - increment, -127.586333),
    MakeABoat(467230410, "Quest4 Johnny", ShipType.Other, 48.183302, -128.686333),
    MakeABoat(547230410, "Roger DodgerOver", ShipType.Other, 49.283302, -124.786333),
    MakeABoat(847251152, "Sarah Sleeting", ShipType.Contractor, 48.285102 - increment, -127.586333),
    MakeABoat(947252151, "Tom And Jerry", ShipType.Other, 39.285102, -128.456333),
    MakeABoat(101252151, "Ugotta B KiddingMe", ShipType.Other, 30.285102, -128.452233),
    MakeABoat(132452151, "Versions Unknown", ShipType.Other, 33.285102 - increment, -140.454233),
    MakeABoat(242452151, "Wild For The Sea", ShipType.Other, 40.518102, -145.334233),
    MakeABoat(587452151, "X The Racer", ShipType.Contractor, 45.518102, -124.344233),
    MakeABoat(681452151, "Yes I can", ShipType.Other, 40.418202 - increment, -123.486233)
    }
    'Dim dBships = DataAccess.DataConverter.ConvertTo(Of ShipDb)(New DataAccess.SQLTalker(Configuration.ConfigurationManager.ConnectionStrings("Ships").ToString()).GetData("EXEC Ships.pGetAllShips"))
    'Return dBships.GroupBy(Function(x) New With {Key x.ShipId, Key x.MMSI, Key x.ShipName, Key x.ShipTypeId, Key x.Latitude, Key x.Longitude}).Select(Function(x) New ShipModel With
    '                      {
    '                      .MMSI = x.Key.MMSI,
    '                      .ShipName = x.Key.ShipName,
    '                      .ShipType = DirectCast(x.Key.ShipTypeId, ShipType),
    '                      .Location = New Location() With {.Latitude = x.Key.Latitude, .Longitude = x.Key.Longitude},
    '                      .Volumes = x.Select(Function(y) New ShipVolume With
    '                          {
    '                          .BoatHale = y.BoatHale,
    '                          .ExpectedVolume = y.ExpectedVolume,
    '                          .CatchType = DirectCast(y.CatchTypeID, CatchType)}
    '                          ).OrderByDescending(Function(z) z.Threshold).ToList()
    '                      }).ToList()

    'Return {
    '  MakeABoat(1, "Brett Home", ShipType.Owned, 45.457302, -122.754326),
    '  MakeABoat(2, "Thai Roses", ShipType.Other, 45.486155, -122.747739),
    '  MakeABoat(3, "Seattle", ShipType.Other, 47.6149942, -122.4759882)
    '}
    'MakeABoat(2, "Thai Roses", ShipType.Other, 45.486155, -122.747739)

  End Function


  'MakeABoat(3, "Seattle", ShipType.Other, 47.6149942, -122.4759882)



  Private Function MakeABoat(mmsi As Integer, shipName As String, shipType As ShipType, latitude As Double, longitude As Double, Optional shipVolumes As List(Of ShipVolume) = Nothing) As ShipModel
    Return New ShipModel With {
                              .MMSI = mmsi,
                              .ShipName = shipName,
                              .ShipType = shipType,
                              .Location = New Location() With {.Latitude = latitude, .Longitude = longitude},
                              .Volumes = If(shipVolumes IsNot Nothing, New List(Of ShipVolume)(shipVolumes), Nothing)
                              }
  End Function

  Public Function RetrieveShipsAndDetermineCollision(ships As List(Of ShipModel), distanceThreshold As Double) As IList(Of ShipGroupingModel)
    If (ships?.Count > 0) Then
      Dim ReturnPriorityBoat As Func(Of ShipModel, ShipModel, ShipModel) = Function(x, y) If(x.ShipType <= y.ShipType, x, y)
      Dim groupings = New List(Of ShipGroupingModel)

      Dim CollectionToEmpty = ships.ToList()
      Do While CollectionToEmpty.Count > 0
        Dim currentShip = CollectionToEmpty(0)
        Dim currentGroup As New ShipGroupingModel With {.Location = currentShip.Location, .ShipType = currentShip.ShipType, .Ships = New List(Of ShipModel)({currentShip})}
        CollectionToEmpty.RemoveAt(0)

        For i As Integer = CollectionToEmpty.Count - 1 To 0 Step -1
          Dim shipToCompare = CollectionToEmpty(i)
          If DetectCollision(currentShip.Location, shipToCompare.Location, distanceThreshold) Then
            If (currentGroup.ShipType > shipToCompare.ShipType) Then
              Dim priorityShip = ReturnPriorityBoat(currentShip, shipToCompare)
              currentGroup.Location = priorityShip.Location
              currentGroup.ShipType = priorityShip.ShipType
            End If

            currentGroup.Ships.Add(shipToCompare)
            CollectionToEmpty.RemoveAt(i)
          End If
        Next

        groupings.Add(currentGroup)
      Loop

      Return groupings
    End If
  End Function

  Private Function DetectCollision(loc1 As Location, loc2 As Location, distanceThreshold As Double) As Boolean
    Dim milesDistanceBetweenPoints = loc1.DistanceTo(loc2, DistanceUnit.Miles)

    Return ((distanceThreshold * 2) > milesDistanceBetweenPoints)
  End Function

  Public Function TransformShipsIntoString(ships As List(Of ShipModel)) As String
    Dim FindPropertyInfo As Func(Of Object, PropertyInfo, String) = Function(x, y) $"({y.Name} {y.GetValue(x)}) "
    Dim props = New ShipModel().GetType().GetProperties().ToList()

    Dim sb = New StringBuilder()

    ships.ForEach(Sub(x)
                    props.ForEach(Sub(y) sb.Append(FindPropertyInfo(x, y)))
                    sb.Append("; ")
                  End Sub)

    Return sb.ToString()
  End Function

End Module