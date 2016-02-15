Imports System.Net
Imports DataAccess

Public Class FleetMonProvider
  Private _apiBaseCallAddress As String

  Public Sub New()
    _apiBaseCallAddress = System.Configuration.ConfigurationManager.AppSettings("FleetMonAPIBaseAddress") + "/{0}/?username=" + System.Configuration.ConfigurationManager.AppSettings("FleetMonAPIUser") _
    + "&api_key=" + System.Configuration.ConfigurationManager.AppSettings("FleetMonAPIKey") + "&format={1}"
  End Sub

  Public Function ReturnMyFleetShipsFromFleetmon(fleetMonAPICalltoMake As FleetMonAPICall, fleetMonAPIformatToReturn As FleetMonAPIReturnType) As IList(Of ShipDb)
    Try
      Dim specificAPICall = String.Format(_apiBaseCallAddress, fleetMonAPICalltoMake.ToString(), fleetMonAPIformatToReturn.ToString())

      Using client = New WebClient()
        Dim xdoc As XDocument = XDocument.Load(specificAPICall)
        Dim vessels = xdoc.Descendants("vessel")

        Return vessels.ToList().Select(Function(x) New ShipDb With
                                             {
                                              .MMSI = x.Element(NameOf(FleetMonVesselModel.mmsinumber)).Value,
                                              .ShipName = x.Element(NameOf(FleetMonVesselModel.name)).Value,
                                              .Latitude = x.Element(NameOf(FleetMonVesselModel.latitude)).Value,
                                              .Longitude = x.Element(NameOf(FleetMonVesselModel.longitude)).Value,
                                              .ShipTypeId = [Enum].Parse(GetType(ShipType), x.Parent.Element("tags").Value)
                                              }).ToList()
      End Using
    Catch ex As Exception
      Throw ex
    End Try
  End Function
End Class
