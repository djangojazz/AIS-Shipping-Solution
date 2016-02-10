Imports System.ServiceProcess

<CompilerServices.DesignerGenerated()>
Partial Class ChartingService
  Inherits ServiceBase

  ' The main entry point for the process
  <MTAThread()>
  <DebuggerNonUserCode()>
  Shared Sub Main(ByVal cmdArgs() As String)
    Dim ServicesToRun() As ServiceBase
    ServicesToRun = New ServiceBase() {New ChartingService(cmdArgs)}
    ServiceBase.Run(ServicesToRun)
  End Sub

  'Required by the Component Designer
  Private components As ComponentModel.IContainer

  ' NOTE: The following procedure is required by the Component Designer
  ' It can be modified using the Component Designer.  
  ' Do not modify it using the code editor.
  <DebuggerStepThrough()>
  Private Sub InitializeComponent()
    components = New ComponentModel.Container()
    Me.ServiceName = "ChartingService"
  End Sub

End Class
