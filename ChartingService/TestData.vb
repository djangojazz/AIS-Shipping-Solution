Public Class TestDatas
  Public Function ReturnTestData() As IList(Of ShipDb)
    Return {
      New ShipDb With {.MMSI = 111111111, .ShipName = "Anne Sleuth", .ShipTypeId = 1, .Latitude = 46.851859, .Longitude = -129.322418, .BoatHale = 5000, .ExpectedVolume = 20000, .CatchTypeID = 3},
      New ShipDb With {.MMSI = 111111111, .ShipName = "Anne Sleuth", .ShipTypeId = 1, .Latitude = 46.851859, .Longitude = -129.322418, .BoatHale = 5000, .ExpectedVolume = 20000, .CatchTypeID = 3}
      }
    '          New List(Of ShipVolume)({
    '                                  New ShipVolume With {.BoatHale = 5000, .ExpectedVolume = 20000, .CatchType = CatchType.Salmon},
    '                                  New ShipVolume With {.BoatHale = 3500, .ExpectedVolume = 4000, .CatchType = CatchType.DoverSole},
    '                                  New ShipVolume With {.BoatHale = 7600, .ExpectedVolume = 8000, .CatchType = CatchType.Tuna}
    '                                  })),
    '111112211, "Bon Voyage", ShipType.Contractor, 47.871859 - increment, -130.322418 - increment),
    '367197230, "Buck & Ann", ShipType.Owned, 46.451859 - increment, -124.322418 - increment,
    '          New List(Of ShipVolume)({
    '                                  New ShipVolume With {.BoatHale = 500, .ExpectedVolume = 2000, .CatchType = CatchType.Shrimp}
    '                                  })),
    '371321345, "Chrimeney Jeeps", ShipType.Owned, 51.421869 - increment, -135.322418 - increment,
    '          New List(Of ShipVolume)({
    '                                  New ShipVolume With {.BoatHale = 4000, .ExpectedVolume = 20000, .CatchType = CatchType.Rockfish}
    '                                  })),
    '381321345, "Chappy Pappy", ShipType.Other, 51.456469, -131.135418 - increment),
    '371341345, "Charlies Cheepo", ShipType.Other, 40.456469, -126.155418 - increment),
    '384841345, "Dudes Dud", ShipType.Other, 43.756854, -124.456789 - increment),
    '397541345, "Earles Agony", ShipType.Other, 43.897445, -124.557781 - increment),
    '471233478, "Franks SingleFleet", ShipType.Contractor, 45.114445, -127.654811),
    '124835678, "Forget Flying", ShipType.Other, 40.458123, -123.551451 - increment),
    '813545664, "Genas Sinker", ShipType.Other, 46.124545, -127.654811 - increment),
    '876541242, "Hectors Habit", ShipType.Contractor, 40.114445, -127.774811 - increment),
    '991233478, "Icant Float", ShipType.Other, 45.548123 - increment, -129.811451 - increment),
    '871233478, "Jacks Rwild", ShipType.Other, 42.712387, -133.887451 - increment),
    '888233478, "Kelly CanUStop", ShipType.Contractor, 41.112387, -128.457451 - increment),
    '678433478, "Larry Leaper", ShipType.Other, 40.512387, -125.877451 - increment),
    '367150410, "Major Major", ShipType.Other, 46.333302, -124.116333 - increment),
    '387150410, "Neato Torpedo", ShipType.Other, 47.333302, -124.056333 - increment),
    '487150410, "October Ocean", ShipType.Other, 48.333302, -126.556333),
    '587230410, "Pauls PushingIt", ShipType.Other, 46.583302 - increment, -127.586333),
    '467230410, "Quest4 Johnny", ShipType.Other, 48.183302, -128.686333),
    '547230410, "Roger DodgerOver", ShipType.Other, 49.283302, -124.786333),
    '847251152, "Sarah Sleeting", ShipType.Contractor, 48.285102 - increment, -127.586333),
    '947252151, "Tom And Jerry", ShipType.Other, 39.285102, -128.456333),
    '101252151, "Ugotta B KiddingMe", ShipType.Other, 30.285102, -128.452233),
    '132452151, "Versions Unknown", ShipType.Other, 33.285102 - increment, -140.454233),
    '242452151, "Wild For The Sea", ShipType.Other, 40.518102, -145.334233),
    '587452151, "X The Racer", ShipType.Contractor, 45.518102, -124.344233),
    '681452151, "Yes I can", ShipType.Other, 40.418202 - increment, -123.486233)

  End Function
End Class
