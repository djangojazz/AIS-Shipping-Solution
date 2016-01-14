using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace DataAccessCSharp
{
  public class SQLTalker
  {
    private string Server { get; set; }
    private string Database { get; set; }

    // Only Cnx string to database will be available to see to the caller of the class.
    public string Cnx { get; set; }

    // default values of the library is set to local database that has a test database.
    public SQLTalker(string server = "(local)", string database = "Tester")
    {
      Server = server;
      Database = database;
      Cnx = "Integrated Security=SSPI;Persist Security Info=False;Data Source =" + server + ";Initial Catalog=" + database;
    }
    
    #region String Readers to populate a sql query from a sql file.
    public string SQLQueryFromFile(string sqlloc)
    {
      try
      {
        using (StreamReader sr = new StreamReader(sqlloc))
        {
          return sr.ReadToEnd().ToString();
        }
      }
      catch
      {
        return "Does not exist!";
      }
    }

    public string SQLQueryFromFile(string sqlloc, string rpl, string rplw)
    {
      try
      {
        using (StreamReader sr = new StreamReader(sqlloc))
        {
          return sr.ReadToEnd().Replace(rpl, rplw);
        }
      }
      catch
      {
        return "Does not exist!";
      }
    }

    public string SQLQueryFromFile(string sqlloc, string rpl, string rplw, string rpl2, string rplw2)
    {
      try
      {
        using (StreamReader sr = new StreamReader(sqlloc))
        {
          return sr.ReadToEnd().Replace(rpl, rplw).Replace(rpl2, rplw2);
        }
      }
      catch
      {
        return "Does not exist!";
      }

    }
    #endregion

    #region Related to populating data sets
    public string Reader(string sql, string seperator, bool inclhdr)
    {
      using (SqlConnection cn = new SqlConnection(Cnx))
      using (SqlCommand cmd = new SqlCommand(sql, cn))
      {
        cn.Open();
        cmd.CommandTimeout = 60;
        var sb = new StringBuilder();

        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          if (inclhdr == true)
          {
            string strhdr = string.Empty;
            
            using (DataTable schema = rdr.GetSchemaTable())
            {
              for (int i = 0; i < schema.Rows.Count; i++)
              {
                strhdr += schema.Rows[i][0].ToString();
                if (i < schema.Rows.Count - 1)
                {
                  strhdr += seperator;
                }
              }
              sb.AppendLine(strhdr);
            }
          }
          
          while (rdr.Read())
          {

            string strRow = "";
            for (int i = 0; i < rdr.FieldCount; i++)
            {
              strRow += rdr.GetValue(i).ToString();
              if (i < rdr.FieldCount - 1)
              {
                strRow += seperator;
              }
            }
            sb.AppendLine(strRow);
          }

          cn.Close();
          return sb.ToString();
        }
      }
    }

    public string Reader(string sql, char seperator, char delimeter, bool inclhdr)
    {
      using (SqlConnection cn = new SqlConnection(Cnx))
      using (SqlCommand cmd = new SqlCommand(sql, cn))
      {
        cn.Open();
        cmd.CommandTimeout = 60;
        var sb = new StringBuilder();

        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
          if (inclhdr == true)
          {
            string strhdr = string.Empty;

            using (DataTable schema = rdr.GetSchemaTable())
            {
              for (int i = 0; i < schema.Rows.Count; i++)
              {
                strhdr += delimeter + schema.Rows[i][0].ToString() + delimeter;
                if (i < schema.Rows.Count - 1)
                {
                  strhdr += seperator;
                }
              }
              sb.Append(strhdr);
            }
            sb.Append(Environment.NewLine);
          }

          // Read data and write rows out to string.
          while (rdr.Read())
          {
            string strRow = "";
            for (int i = 0; i < rdr.FieldCount; i++)
            {
              strRow += delimeter + rdr.GetValue(i).ToString() + delimeter;

              if (i < rdr.FieldCount - 1)
              {
                strRow += seperator;
              }
            }

            sb.Append(strRow);

            if (rdr.FieldCount > 1)
              sb.Append(Environment.NewLine);
          }

          cn.Close();
          return sb.ToString();
        }
      }
    }

    public string Procer(string sql)
    {
      using (SqlConnection cn = new SqlConnection(Cnx))
      using (SqlCommand cmd = new SqlCommand(sql, cn))
      {
        cmd.CommandTimeout = 60;
        var sb = new StringBuilder();
        cn.Open();

        SqlParameter rtn = cmd.Parameters.Add("return", SqlDbType.Int);
        rtn.Direction = ParameterDirection.ReturnValue;
        cmd.ExecuteNonQuery();

        if ((int)rtn.Value == 0)
          sb.Append("Results:\t\tSuccess");
        else
          sb.Append("Result:\t\tFailure!");

        cn.Close();
        return sb.ToString();
      }
    }

    public string Procer(string sql, bool counts)
    {
      using (SqlConnection cn = new SqlConnection(Cnx))
      using (SqlCommand cmd = new SqlCommand(sql, cn))
      {
        cmd.CommandTimeout = 60;
        var sb = new StringBuilder();
        cn.Open();

        SqlParameter rtn = cmd.Parameters.Add("return", SqlDbType.Int);
        rtn.Direction = ParameterDirection.ReturnValue;
        
        if (counts == true)
        {
          sb.Append("RowsAffected:\t" + cmd.ExecuteNonQuery().ToString());
          sb.Append("\n");
          if ((int)rtn.Value == 0)
            sb.Append("Results:\t\tSuccess");
          else
            sb.Append("Result:\t\tFailure!");
        }
        else
        {
          cmd.ExecuteNonQuery();

          if ((int)rtn.Value == 0)
            sb.Append("Results:\t\tSuccess");
          else
            sb.Append("Result:\t\tFailure!");
        }

        cn.Close();
        return sb.ToString();
      }
    }

    public DataTable GetData(string sqlquery)
    {
      using (SqlConnection cn = new SqlConnection(Cnx))
      using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
      using (SqlDataAdapter adapter = new SqlDataAdapter())
      using (DataTable table = new DataTable())
      {
        cn.Open();
        adapter.SelectCommand = cmd;
        table.Locale = System.Globalization.CultureInfo.InvariantCulture;
        adapter.Fill(table);
        cn.Close();

        return table;
      }
    }
    #endregion

    public static string Help()
    {
      StringBuilder sb = new StringBuilder();

      sb.Append(
          "1. To use this DLL you need to declare a new instance first of the class\n" +
          "SQLTalker with ( <server>, <db>)\n\n" +
          "2. The <server> if NOT listed will default to (local) and the\n" +
          "<db> to 'Test'.  You may replace these values with ones you desire.\n" +
          "EG: SQLTalker s = new SQLTalker(\"PCTRSQL7\", \"Case3315\")\n\n" +
          "3. These properties are then made into a connection string public property\n" +
          " used for connections. EG: s.Cnx should yield(with default): \n" +
          "Integrated Security=SSPI;Persist Security Info=False;Data Source=.\n" +
          ";Initial Catalog=Test\n\n" +
          "4. There is a method, SQLQueryFromFile, with two overloads of getting data\n" +
          "from a file.  Merely populate the single parameter with the location of a\n" +
          "SQL file (*.sql) and it will read the file to end and give you a string with\n" +
          "what it found.  The overloads accept a replace pattern to find, and then\n" +
          "replace with, the first overload takes a single replace and the second two.\n\n" +
          "5. There are methods of obtaining data from a database for output.\n" +
          "These methods are:\n\n" +
          "\tA.  Reader = will execute a Data Reader to generate results back\n" +
          "\tas a string value. Parameters are:\n" +
          "\t\ti. sql = string of sql statement\n" +
          "\t\tii. seperator = string used to seperate values in grid.\n" +
          "\t\tI suggest ',' is used for standard CSV.\n" +
          "\t\tiii. inclhdr = Set 'true' for headers 'false' for no headers.\n\n" +
          "\tB.  Procer = will execute a procedure that DOES NOT RETURN results.\n" +
          "\tThe return will be a string return of \"success\" or \"failure\"\n" +
          "\tUse another method if results are desired and also declare the \n" +
          "\tvariables in the execution.  EG: exec proc @a = (a), @b = (b)\n" +
          "\tThe parameters are: \n" +
          "\t\ti. sql = string of sql statement\n" +
          "\t\tii. counts = rows affected.  To be used if rows are changed.\n\n" +
          "\tC. GetData = will return a datatable to examine grid results.\n" +
          "\tThe only parameter is sql which like above will be the string \n" +
          "\tof a SQL statement."
           );

      return sb.ToString();
    }
  }

}
