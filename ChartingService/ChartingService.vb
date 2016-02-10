Imports System.Runtime.InteropServices

Public Class ChartingService

  Private Property eventId As Integer
  Declare Auto Function SetServiceStatus Lib "advapi32.dll" (ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean

  Public Sub New(ByVal cmdArgs() As String)
    InitializeComponent()
    AssignCommandArgsForService(cmdArgs)
  End Sub

  Private Sub AssignCommandArgsForService(cmdArgs() As String)
    Dim eventSourceName As String = "TestSource"
    Dim logName As String = "TestLog"
    If (cmdArgs.Count() > 0) Then
      eventSourceName = cmdArgs(0)
    End If
    If (cmdArgs.Count() > 1) Then
      logName = cmdArgs(1)
    End If

    EventLog1 = New EventLog()
    If (Not EventLog.SourceExists(eventSourceName)) Then EventLog.CreateEventSource(eventSourceName, logName)

    EventLog1.Source = eventSourceName
    EventLog1.Log = logName
  End Sub

  Protected Overrides Sub OnStart(ByVal args() As String)
    AssignCommandArgsForService(args)

    ' Update the service state to Start Pending.
    Dim serviceStatus As ServiceStatus = New ServiceStatus()
    serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING
    serviceStatus.dwWaitHint = 100000
    SetServiceStatus(Me.ServiceHandle, serviceStatus)

    EventLog1.WriteEntry("In OnStart")

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

    EventLog1.WriteEntry("In OnStop.")

    ' Update the service state to Ending.
    serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
    SetServiceStatus(Me.ServiceHandle, serviceStatus)
  End Sub

  Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)
    ' TODO: Insert monitoring activities here.
    EventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId)
    eventId = eventId + 1
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

End Class
