namespace ChartingCSharp.ViewModels
{
  using DataAccessCSharp;
  using System.Data.SqlClient;
  using System.Configuration;
  using System.Data;
  public class MainWindowViewModel
  {
    public string Text { get; set; }

    public MainWindowViewModel()
    {
      Text = "Hello There";
      GetData();
    }

    private void GetData()
    {
      var testerData = new TesterData();

      string query = "Select top 10 * from dbo.Person";
      string proc = "EXEC pGetOrdersByPersonId 1";
      var connection = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;

      var sqlTalker = new SQLTalker(".", "Tester");
        //.Procer(proc, true);
        //.Reader(query, ',', '"', true);

      var thing = "Hello";

      //using (SqlConnection sqlConn = new SqlConnection(connection))
      //using (SqlCommand cmd = new SqlCommand(query, sqlConn))
      //using (SqlDataAdapter adapter = new SqlDataAdapter())
      //using (DataTable table = new DataTable())
      //{
      //  sqlConn.Open();
      //  adapter.SelectCommand = cmd;
      //  adapter.Fill(table);
      //  sqlConn.Close();
      //  var thing = table;
      //  //int count = (int)cmd.ExecuteScalar();
      //  //Text = count.ToString();

        
      //}

      //var tables = testerData.Tables;
      //var view = testerData.vPersonOrders;

      //var rdr = testerData.vPersonOrders.CreateDataReader();
      //while(rdr.Read())
      //{
      //  Text = rdr["FirstName"].ToString();
      //}


    }
  }
}
