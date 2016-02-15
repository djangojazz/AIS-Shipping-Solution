Public Module DataAccessForProviders

  Public Function ReturnShipsFromProvider(providerType As ChartingProviderAPIType, Optional incrementPositionChange As Double = 0) As IList(Of ShipDb)
    Select Case providerType
      Case ChartingProviderAPIType.Test : Return ReturnShipsFromTest(incrementPositionChange)
      Case ChartingProviderAPIType.Fleetmon : Return ReturnShipsFromFleetMon()
      Case Else : Return ReturnShipsFromTest(incrementPositionChange)
    End Select
  End Function

  Private Function ReturnShipsFromTest(incrementPositionChange As Double) As IList(Of ShipDb)
    Return New TestDataProvider().ReturnTestShips(incrementPositionChange)
  End Function

  Private Function ReturnShipsFromFleetMon() As IList(Of ShipDb)
    Return New FleetMonProvider().ReturnMyFleetShipsFromFleetmon(FleetMonAPICall.myfleet, FleetMonAPIReturnType.xml)
  End Function

End Module
