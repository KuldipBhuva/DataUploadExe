using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;



/// <summary>
/// Summary description for connection
/// </summary>
/// 





public class connection
{
    SqlConnection conn;
    //string Dest = "Data Source=KETAN-PC\\KETAN;Initial Catalog=VPDPTAX;User ID=sa;Password=sa123";
    string Dest = "Data Source=182.74.222.163,8001;Initial Catalog=VPDBack14Feb;User ID=newtech;Password=Smile@12345";

    public connection()
    {
        //
        // TODO: Add constructor logic here
        //
        conn = new SqlConnection();

        conn.ConnectionString = Dest;


    }

    public SqlConnection OpenConnetion()
    {
        if (conn.State != ConnectionState.Open)
        {
            conn.Open();
        }
        return conn;
    }

    public void CloseConnection()
    {
        if (conn.State != ConnectionState.Closed)
        {
            conn.Close();
        }
    }
}
