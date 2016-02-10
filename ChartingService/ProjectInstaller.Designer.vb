<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
  Inherits System.Configuration.Install.Installer

  'Installer overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Component Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Component Designer
  'It can be modified using the Component Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()>
  Private Sub InitializeComponent()
    Me.ChartingServiceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller()
    Me.ChartingServiceInstaller = New System.ServiceProcess.ServiceInstaller()
    '
    'ChartingServiceProcessInstaller
    '
    Me.ChartingServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
    Me.ChartingServiceProcessInstaller.Password = Nothing
    Me.ChartingServiceProcessInstaller.Username = Nothing
    '
    'ChartingServiceInstaller
    '
    Me.ChartingServiceInstaller.Description = "Charting Service For Periodic Database Updating for Charting Locations"
    Me.ChartingServiceInstaller.DisplayName = "Charting Service"
    Me.ChartingServiceInstaller.ServiceName = "ChartingService"
    Me.ChartingServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic
    '
    'ProjectInstaller
    '
    Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.ChartingServiceProcessInstaller, Me.ChartingServiceInstaller})

  End Sub

  Friend WithEvents ChartingServiceProcessInstaller As ServiceProcess.ServiceProcessInstaller
  Friend WithEvents ChartingServiceInstaller As ServiceProcess.ServiceInstaller
End Class
