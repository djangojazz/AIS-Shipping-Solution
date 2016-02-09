Public Class TestData
  Public Function ReturnTestShips() As IList(Of ShipDb)
    Return {
      New ShipDb With {.MMSI = 111111111, .ShipName = "Anne Sleuth", .ShipTypeId = 1, .Latitude = 46.851859, .Longitude = -129.322418},
      New ShipDb With {.MMSI = 367197230, .ShipName = "Buck & Ann", .ShipTypeId = 1, .Latitude = 46.451859, .Longitude = -124.322418},
      New ShipDb With {.MMSI = 371321345, .ShipName = "Chrimey Jeeps", .ShipTypeId = 1, .Latitude = 51.421869, .Longitude = -135.322418},
      New ShipDb With {.MMSI = 316023833, .ShipName = "Raw Spirit", .ShipTypeId = 1, .Latitude = 50.769775, .Longitude = -127.362129},
      New ShipDb With {.MMSI = 111112211, .ShipName = "Bon Voyage", .ShipTypeId = 2, .Latitude = 47.871859, .Longitude = -130.322418},
      New ShipDb With {.MMSI = 876541242, .ShipName = "Hectors Habit", .ShipTypeId = 2, .Latitude = 40.114445, .Longitude = -127.774811},
      New ShipDb With {.MMSI = 471233478, .ShipName = "Freds Fleet", .ShipTypeId = 2, .Latitude = 45.114445, .Longitude = -127.654811},
      New ShipDb With {.MMSI = 888233478, .ShipName = "Kelly CanUStop", .ShipTypeId = 2, .Latitude = 41.112387, .Longitude = -128.457451},
      New ShipDb With {.MMSI = 587452151, .ShipName = "X The Racer", .ShipTypeId = 2, .Latitude = 45.518102, .Longitude = -124.344233},
      New ShipDb With {.MMSI = 876452151, .ShipName = "Zuess", .ShipTypeId = 2, .Latitude = 41.418202, .Longitude = -128.546233},
      New ShipDb With {.MMSI = 847251152, .ShipName = "Sarah Sleeting", .ShipTypeId = 2, .Latitude = 48.285102, .Longitude = -127.586333},
      New ShipDb With {.MMSI = 367002180, .ShipName = "David Brusco", .ShipTypeId = 2, .Latitude = 47.625912, .Longitude = -124.7612},
      New ShipDb With {.MMSI = 681452151, .ShipName = "Yes I can", .ShipTypeId = 3, .Latitude = 40.418202, .Longitude = -123.486233},
      New ShipDb With {.MMSI = 813213453, .ShipName = "Chappy Pappy", .ShipTypeId = 3, .Latitude = 51.456469, .Longitude = -131.135418},
      New ShipDb With {.MMSI = 713413454, .ShipName = "Charlies Cheepo", .ShipTypeId = 3, .Latitude = 40.456469, .Longitude = -126.155418},
      New ShipDb With {.MMSI = 848413458, .ShipName = "Dudes Dud   ", .ShipTypeId = 3, .Latitude = 43.756854, .Longitude = -124.456789},
      New ShipDb With {.MMSI = 975413459, .ShipName = "Earles Agony", .ShipTypeId = 3, .Latitude = 43.897445, .Longitude = -124.557781},
      New ShipDb With {.MMSI = 124835678, .ShipName = "Forget Flying", .ShipTypeId = 3, .Latitude = 40.458123, .Longitude = -123.551451},
      New ShipDb With {.MMSI = 813545664, .ShipName = "Genas Sinker", .ShipTypeId = 3, .Latitude = 46.124545, .Longitude = -127.654811},
      New ShipDb With {.MMSI = 991233478, .ShipName = "Icant Float", .ShipTypeId = 3, .Latitude = 45.548123, .Longitude = -129.811451},
      New ShipDb With {.MMSI = 871233478, .ShipName = "Jacks Rwild", .ShipTypeId = 3, .Latitude = 42.712387, .Longitude = -133.887451},
      New ShipDb With {.MMSI = 678433478, .ShipName = "Larry Leaper", .ShipTypeId = 3, .Latitude = 40.512387, .Longitude = -125.877451},
      New ShipDb With {.MMSI = 367150410, .ShipName = "Major Major", .ShipTypeId = 3, .Latitude = 46.333302, .Longitude = -124.116333},
      New ShipDb With {.MMSI = 387150410, .ShipName = "Neato Torpedo", .ShipTypeId = 3, .Latitude = 47.333302, .Longitude = -124.056333},
      New ShipDb With {.MMSI = 487150410, .ShipName = "October Ocean", .ShipTypeId = 3, .Latitude = 48.333302, .Longitude = -126.556333},
      New ShipDb With {.MMSI = 587230410, .ShipName = "Pauls PushingIt", .ShipTypeId = 3, .Latitude = 46.583302, .Longitude = -127.586333},
      New ShipDb With {.MMSI = 467230410, .ShipName = "Quest4 Johnny", .ShipTypeId = 3, .Latitude = 48.183302, .Longitude = -128.686333},
      New ShipDb With {.MMSI = 547230410, .ShipName = "Roger DodgerOver", .ShipTypeId = 3, .Latitude = 49.283302, .Longitude = -124.786333}
      }
  End Function

  Public Function CreateTestShipVolume(boatHale As Integer, expectedVolume As Integer, catchTypeId As Integer) As ShipVolumeDb
    Return New ShipVolumeDb With {.BoatHale = boatHale, .ExpectedVolume = expectedVolume, .CatchTypeID = catchTypeId}
  End Function
End Class
