Imports System.Runtime.InteropServices
Imports ChartingDataProviders
Imports DataAccess

Public Class ChartingService

  Private _chartingEventLog As EventLog
  Private _timer As Timers.Timer = New Timers.Timer()
  Private _sqlTalker As SQLTalker
  Private _eventId As Integer = 1
  Private _pollingDurationInMinutes As Integer = 15
  Private _chartingAPIProviderType As ChartingProviderAPIType
  Private _serviceStatus As ServiceStatus = New ServiceStatus()

  Public Enum ServiceState
    SERVICE_STOPPED = 1
    SERVICE_START_PENDING = 2
    SERVICE_STOP_PENDING = 3
    SERVICE_RUNNING = 4
    SERVICE_CONTINUE_PENDING = 5
    SERVICE_PAUSE_PENDING = 6
    SERVICE_PAUSED = 7
  End Enum

  <StructLayout(LayoutKind.Sequential)>
  Public Structure ServiceStatus
    Public dwServiceType As Long
    Public dwCurrentState As ServiceState
    Public dwControlsAccepted As Long
    Public dwWin32ExitCode As Long
    Public dwServiceSpecificExitCode As Long
    Public dwCheckPoint As Long
    Public dwWaitHint As Long
  End Structure

  Declare Auto Function SetServiceStatus Lib "advapi32.dll" (ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean

  Public Sub New(ByVal cmdArgs() As String)
    InitializeComponent()
  End Sub

  Protected Overrides Sub OnStart(ByVal args() As String)
    SetStatus(ServiceState.SERVICE_START_PENDING, 100000)

    If (args.Count() > 0) Then
      Try
        _pollingDurationInMinutes = CInt(args(0))
      Catch ex As Exception
        SetStatus(ServiceState.SERVICE_CONTINUE_PENDING)
        _pollingDurationInMinutes = 1
        SetStatus(ServiceState.SERVICE_START_PENDING)
      End Try
    End If

    If (args.Count() > 1) Then
      Try
        _chartingAPIProviderType = DirectCast(CInt(args(1)), ChartingProviderAPIType)
      Catch ex As Exception
        SetStatus(ServiceState.SERVICE_CONTINUE_PENDING)
        _chartingAPIProviderType = DirectCast(1, ChartingProviderAPIType)
        SetStatus(ServiceState.SERVICE_START_PENDING)
      End Try
    End If

    _sqlTalker = New SQLTalker(Configuration.ConfigurationManager.ConnectionStrings("Ships").ToString())
    SetUpLoggingEvent()

    _timer.Interval = _pollingDurationInMinutes * 60000
    AddHandler _timer.Elapsed, AddressOf UpdateDatabaseWithProviderValues
    _timer.Start()

    ' Update the service state to Running.
    SetStatus(ServiceState.SERVICE_RUNNING)
    _chartingEventLog.WriteEntry("Service Running")
  End Sub

  Protected Overrides Sub OnStop()
    SetStatus(ServiceState.SERVICE_STOP_PENDING)
    TruncateTestRecords()
    _chartingEventLog.WriteEntry("Service Stopped.")
    SetStatus(ServiceState.SERVICE_STOPPED)
  End Sub

  Private Sub TruncateTestRecords()
    Dim outputMessage = String.Empty
    Dim truncateShipsHistory = "Truncate Table Ships.teShipPastLocation"
    Dim deleteShips = "Delete Ships.teShipDetail; DBCC CHECKIDENT ('Ships.teShipDetail', RESEED, 0);"

    Dim historyResults = _sqlTalker.Procer(truncateShipsHistory)
    outputMessage += $"ShipsHistory {historyResults} {Environment.NewLine}"
    Dim results = _sqlTalker.Procer(deleteShips)
    outputMessage += $"Ships {results} {Environment.NewLine}"
    _chartingEventLog.WriteEntry(outputMessage, EventLogEntryType.Information, _eventId)
  End Sub

  Private Sub UpdateDatabaseWithProviderValues(sender As Object, e As Timers.ElapsedEventArgs)
    Dim outputMessage = String.Empty
    Dim data As IList(Of ShipDb) = New List(Of ShipDb)
    Dim serializedXml As String

    Try
      data = ReturnShipsFromProvider(_chartingAPIProviderType, (_eventId * 0.00001))
      outputMessage += $"Count of Ships to insert {data.Count} {Environment.NewLine}"
    Catch ex As Exception
      'TODO: email out someone that we could not get data
      outputMessage += "Could not get a list of the ships!"
    End Try

    Try
      serializedXml = data.SerializeToXml()
    Catch ex As Exception
      outputMessage += "Could not serialize data returned from API!"
    End Try

    Try
      outputMessage += _sqlTalker.BlockLoadXMLShipData(_pollingDurationInMinutes, serializedXml)
    Catch ex As Exception
      outputMessage += "Could not load the data into the database!"
    End Try

    _chartingEventLog.WriteEntry(outputMessage, EventLogEntryType.Information, _eventId)
    _eventId += 1
  End Sub

  Private Sub SetUpLoggingEvent()
    Dim eventSourceName As String = "ChartingSource"
    Dim logName As String = "ChartingLog"

    _chartingEventLog = New EventLog()
    If (Not EventLog.SourceExists(eventSourceName)) Then EventLog.CreateEventSource(eventSourceName, logName)

    _chartingEventLog.Source = eventSourceName
    _chartingEventLog.Log = logName
  End Sub

  Private Sub SetStatus(status As ServiceState, Optional waitHint As Integer = 0)
    _serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING

    If (waitHint > 0) Then
      _serviceStatus.dwWaitHint = waitHint
    End If
    SetServiceStatus(ServiceHandle, _serviceStatus)
  End Sub

  'UserService overrides dispose to clean up the component list.
  <DebuggerNonUserCode()>
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
        _chartingEventLog = Nothing
        _timer = Nothing
        _sqlTalker = Nothing
        _eventId = 0
        _pollingDurationInMinutes = 0
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

End Class
