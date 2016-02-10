Imports System.Data.SqlClient
Imports System.IO
Imports System.Text

Public Class SQLTalker
  Private _cnx As String

  Public Sub New(connectionString As String)
    _cnx = connectionString
  End Sub

  Public Sub New(Optional server As String = "(local)", Optional database As String = "Tester", Optional user As String = Nothing, Optional password As String = Nothing)
    _cnx = If(user?.Length > 0 AndAlso password?.Length > 0, $"Server={server};Database={database};User Id={user};Password={password}", $"Server ={server};Database={database};Trusted_Connection=True;")
  End Sub

#Region "String Readers To populate a sql query from a sql file."
  Public Function SQLQueryFromFile(sqlloc As String) As String
    Try
      Using sr As New StreamReader(sqlloc)
        Return sr.ReadToEnd().ToString()
      End Using
    Catch
      Return "Does Not exist!"
    End Try
  End Function

  Public Function SQLQueryFromFile(sqlloc As String, rpl As String, rplw As String) As String
    Try
      Using sr As New StreamReader(sqlloc)
        Return sr.ReadToEnd().Replace(rpl, rplw)
      End Using
    Catch
      Return "Does Not exist!"
    End Try
  End Function

  Public Function SQLQueryFromFile(sqlloc As String, rpl As String, rplw As String, rpl2 As String, rplw2 As String) As String
    Try
      Using sr As New StreamReader(sqlloc)
        Return sr.ReadToEnd().Replace(rpl, rplw).Replace(rpl2, rplw2)
      End Using
    Catch
      Return "Does Not exist!"
    End Try

  End Function
#End Region

#Region "Related To populating data sets"
  Public Function Reader(sql As String, seperator As String, inclhdr As Boolean) As String
    Using cn As New SqlConnection(_cnx)
      Using cmd As New SqlCommand(sql, cn)
        cn.Open()
        cmd.CommandTimeout = 60
        Dim sb = New StringBuilder()

        Using rdr As SqlDataReader = cmd.ExecuteReader()
          If inclhdr = True Then
            Dim strhdr As String = String.Empty

            Using schema As DataTable = rdr.GetSchemaTable()
              For i As Integer = 0 To schema.Rows.Count - 1
                strhdr += schema.Rows(i)(0).ToString()
                If i < schema.Rows.Count - 1 Then
                  strhdr += seperator
                End If
              Next
              sb.AppendLine(strhdr)
            End Using
          End If

          While rdr.Read()

            Dim strRow As String = ""
            For i As Integer = 0 To rdr.FieldCount - 1
              strRow += rdr.GetValue(i).ToString()
              If i < rdr.FieldCount - 1 Then
                strRow += seperator
              End If
            Next
            sb.AppendLine(strRow)
          End While

          cn.Close()
          Return sb.ToString()
        End Using
      End Using
    End Using
  End Function

  Public Function Reader(sql As String, seperator As Char, delimeter As Char, inclhdr As Boolean) As String
    Using cn As New SqlConnection(_cnx)
      Using cmd As New SqlCommand(sql, cn)
        cn.Open()
        cmd.CommandTimeout = 60
        Dim sb = New StringBuilder()

        Using rdr As SqlDataReader = cmd.ExecuteReader()
          If inclhdr = True Then
            Dim strhdr As String = String.Empty

            Using schema As DataTable = rdr.GetSchemaTable()
              For i As Integer = 0 To schema.Rows.Count - 1
                strhdr += delimeter + schema.Rows(i)(0).ToString() + delimeter
                If i < schema.Rows.Count - 1 Then
                  strhdr += seperator
                End If
              Next
              sb.Append(strhdr)
            End Using
            sb.Append(Environment.NewLine)
          End If

          ' Read data and write rows out to string.
          While rdr.Read()
            Dim strRow As String = ""
            For i As Integer = 0 To rdr.FieldCount - 1
              strRow += delimeter + rdr.GetValue(i).ToString() + delimeter

              If i < rdr.FieldCount - 1 Then
                strRow += seperator
              End If
            Next

            sb.Append(strRow)

            If rdr.FieldCount > 1 Then
              sb.Append(Environment.NewLine)
            End If
          End While

          cn.Close()
          Return sb.ToString()
        End Using
      End Using
    End Using
  End Function

  Public Function Procer(sql As String) As String
    Using cn As New SqlConnection(_cnx)
      Using cmd As New SqlCommand(sql, cn)
        cmd.CommandTimeout = 60
        Dim sb = New StringBuilder()
        cn.Open()

        Dim rtn As SqlParameter = cmd.Parameters.Add("Return", SqlDbType.Int)
        rtn.Direction = ParameterDirection.ReturnValue
        cmd.ExecuteNonQuery()

        If CInt(rtn.Value) = 0 Then
          sb.Append("Results:" & vbTab & vbTab & "Success")
        Else
          sb.Append("Result:" & vbTab & vbTab & "Failure!")
        End If

        cn.Close()
        Return sb.ToString()
      End Using
    End Using
  End Function

  Public Function Procer(sql As String, counts As Boolean) As String
    Using cn As New SqlConnection(_cnx)
      Using cmd As New SqlCommand(sql, cn)
        cmd.CommandTimeout = 60
        Dim sb = New StringBuilder()
        cn.Open()

        Dim rtn As SqlParameter = cmd.Parameters.Add("return", SqlDbType.Int)
        rtn.Direction = ParameterDirection.ReturnValue

        If counts = True Then
          sb.Append("RowsAffected:" & vbTab + cmd.ExecuteNonQuery().ToString())
          sb.Append(vbLf)
          If CInt(rtn.Value) = 0 Then
            sb.Append("Results:" & vbTab & vbTab & "Success")
          Else
            sb.Append("Result:" & vbTab & vbTab & "Failure!")
          End If
        Else
          cmd.ExecuteNonQuery()

          If CInt(rtn.Value) = 0 Then
            sb.Append("Results:" & vbTab & vbTab & "Success")
          Else
            sb.Append("Result:" & vbTab & vbTab & "Failure!")
          End If
        End If

        cn.Close()
        Return sb.ToString()
      End Using
    End Using
  End Function

  Public Function GetData(sqlquery As String) As DataTable
    Using cn As New SqlConnection(_cnx)
      Using cmd As New SqlCommand(sqlquery, cn)
        Using adapter As New SqlDataAdapter()
          Using table As New DataTable()
            cn.Open()
            adapter.SelectCommand = cmd
            table.Locale = Globalization.CultureInfo.InvariantCulture
            adapter.Fill(table)
            cn.Close()

            Return table
          End Using
        End Using
      End Using
    End Using
  End Function
#End Region
End Class
