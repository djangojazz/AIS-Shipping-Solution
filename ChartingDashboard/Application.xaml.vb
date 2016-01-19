Class Application
  Public Sub Application_Exit(sender As Object, e As ExitEventArgs)
    MySettings.Default.Save()
  End Sub
End Class
