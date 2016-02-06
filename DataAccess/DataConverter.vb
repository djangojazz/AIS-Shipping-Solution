Imports System.ComponentModel
Imports System.Reflection

Public Class DataConverter

  Public Shared Function ConvertTo(Of T)(list As IList(Of T)) As DataTable
    Dim table As DataTable = CreateTable(Of T)()
    Dim entityType As Type = GetType(T)
    Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(entityType)

    For Each item As T In list
      Dim row As DataRow = table.NewRow()

      For Each prop As PropertyDescriptor In properties
        row(prop.Name) = prop.GetValue(item)
      Next

      table.Rows.Add(row)
    Next

    Return table
  End Function

  Public Shared Function ConvertTo(Of T)(rows As IList(Of DataRow)) As IList(Of T)
    Dim list As IList(Of T) = Nothing

    If rows IsNot Nothing Then
      list = New List(Of T)()

      For Each row As DataRow In rows
        Dim item As T = CreateItem(Of T)(row)
        list.Add(item)
      Next
    End If

    Return list
  End Function

  Public Shared Function ConvertTo(Of T)(table As DataTable) As IList(Of T)
    If table Is Nothing Then
      Return Nothing
    End If

    Dim rows As New List(Of DataRow)()

    For Each row As DataRow In table.Rows
      rows.Add(row)
    Next

    Return ConvertTo(Of T)(rows)
  End Function

  Public Shared Function CreateItem(Of T)(row As DataRow) As T
    Dim obj As T = Nothing
    If row IsNot Nothing Then
      obj = Activator.CreateInstance(Of T)()

      For Each column As DataColumn In row.Table.Columns
        Dim prop As PropertyInfo = obj.GetType().GetProperty(column.ColumnName)
        Try
          Dim value As Object = row(column.ColumnName)
          If (value.ToString().Length > 0) Then
            prop.SetValue(obj, value, Nothing)
          End If
        Catch
          ' You can log something here
          Throw
        End Try
      Next
    End If

    Return obj
  End Function

  Public Shared Function CreateTable(Of T)() As DataTable
    Dim entityType As Type = GetType(T)
    Dim table As New DataTable(entityType.Name)
    Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(entityType)

    For Each prop As PropertyDescriptor In properties
      table.Columns.Add(prop.Name, prop.PropertyType)
    Next

    Return table
  End Function
End Class
