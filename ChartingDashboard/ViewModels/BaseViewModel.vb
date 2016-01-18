Imports System.ComponentModel

Public Class BaseViewModel
  Implements INotifyPropertyChanged
  Public Event PropertyChanged As PropertyChangedEventHandler
  Private Event INotifyPropertyChanged_PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

  Protected Overridable Sub OnPropertyChanged(propertyName As String)
    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
  End Sub
End Class
