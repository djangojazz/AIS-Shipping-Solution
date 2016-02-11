Imports System.Runtime.InteropServices
Imports ChartingDataProviders

Public Class ChartingService

  Private _chartingEventLog As EventLog
  Private _timer As Timers.Timer = New Timers.Timer()
  Private _sqlTalker As DataAccess.SQLTalker
  Private _eventId As Integer
  Private _pollingDurationInMinutes As Integer
  Private _chartingAPIProviderType As ChartingProviderAPIType
  Private _serviceStatus As ServiceStatus = New ServiceStatus()

  Declare Auto Function SetServiceStatus Lib "advapi32.dll" (ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean

  Public Sub New(ByVal cmdArgs() As String)
    InitializeComponent()
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

    _sqlTalker = New DataAccess.SQLTalker(Configuration.ConfigurationManager.ConnectionStrings("Charting").ToString())
    SetUpLoggingEvent()


    _timer.Interval = _pollingDurationInMinutes * 10000 'CHANGE THIS LATER 60000
    AddHandler _timer.Elapsed, AddressOf OnTimer
    _timer.Start()

    ' Update the service state to Running.
    SetStatus(ServiceState.SERVICE_RUNNING)
    _chartingEventLog.WriteEntry("Service Running")
  End Sub

  Protected Overrides Sub OnStop()
    SetStatus(ServiceState.SERVICE_STOP_PENDING)
    _chartingEventLog.WriteEntry("Service Stopped.")
    SetStatus(ServiceState.SERVICE_STOPPED)
  End Sub

  Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)
    Const sqlCommand As String = "EXEC Ships.pInsertOrUpdateShipPosition 1, 111111111, 'Anne Sleuth', 46.851859, -129.322418, 1"
    Dim countOfShips = _sqlTalker.Procer(sqlCommand)
    Dim outputMessage = $"Provider Type: {_chartingAPIProviderType} with {countOfShips}"

    _chartingEventLog.WriteEntry(outputMessage, EventLogEntryType.Information, _eventId)
    _eventId += 1
  End Sub

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
