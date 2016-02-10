Imports System.Runtime.InteropServices

Public Class ChartingService

  Private _chartingEventLog As EventLog
  Private _databaseName As String
  Private _serverName As String
  Private _sqlTalker As DataAccess.SQLTalker
  Private _eventId As Integer

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

  Protected Overrides Sub OnStart(ByVal args() As String)
    _sqlTalker = New DataAccess.SQLTalker(Configuration.ConfigurationManager.ConnectionStrings("Charting").ToString())
    SetUpLoggingEvent()

    ' Update the service state to Start Pending.
    Dim serviceStatus As ServiceStatus = New ServiceStatus()
    serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING
    serviceStatus.dwWaitHint = 100000
    SetServiceStatus(Me.ServiceHandle, serviceStatus)

    _chartingEventLog.WriteEntry("In OnStart")

    Dim timer As System.Timers.Timer = New System.Timers.Timer()
    timer.Interval = 10000 '10 seconds
    AddHandler timer.Elapsed, AddressOf Me.OnTimer
    timer.Start()

    ' Update the service state to Running.
    serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
    SetServiceStatus(Me.ServiceHandle, serviceStatus)
  End Sub

  Protected Overrides Sub OnStop()
    ' Update the service state to End Pending.
    Dim serviceStatus As ServiceStatus = New ServiceStatus()
    serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING
    SetServiceStatus(Me.ServiceHandle, serviceStatus)

    _chartingEventLog.WriteEntry("In OnStop.")

    ' Update the service state to Ending.
    serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
    SetServiceStatus(Me.ServiceHandle, serviceStatus)
  End Sub

  Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)
    Const sqlCommand As String = "Select Count(*) From Ships.teShipDetail"
    Dim countOfShips = _sqlTalker.GetData(sqlCommand)(0)(0)
    Dim outputMessage = $"You have {countOfShips} ships"

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
  <System.Diagnostics.DebuggerNonUserCode()>
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
        _sqlTalker = Nothing
        _chartingEventLog = Nothing
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

End Class
