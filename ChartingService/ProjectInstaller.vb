Imports System.ComponentModel
Imports System.Configuration.Install

Public Class ProjectInstaller

  Protected Overrides Sub OnBeforeInstall(ByVal savedState As IDictionary)
    Dim parameter As String = "MyDuration"" ""ChartingDataProviderId"
    Context.Parameters("assemblypath") = """" + Context.Parameters("assemblypath") + """ """ + parameter + """"
    MyBase.OnBeforeInstall(savedState)
  End Sub

  Public Sub New()
    MyBase.New()

    'This call is required by the Component Designer.
    InitializeComponent()

    'Add initialization code after the call to InitializeComponent

  End Sub

End Class
