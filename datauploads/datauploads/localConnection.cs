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





public class localConnection
{
    SqlConnection conn;
    //string Src = "Data Source=IBMSRV2;Initial Catalog=pfund;User ID=sa;Password=Server123";
    string Src = "Data Source=192.168.100.212,8001;Initial Catalog=pfund;User ID=newtech;Password=Smile@12345";
    //string Src = System.Configuration.ConfigurationManager.AppSettings.Get("Src");
    //string Src = "Data Source=180.149.246.103,8001;Initial Catalog=VPDPTAX;User ID=sa;Password=DFjkh*#bfu63";
    public localConnection()
    {
        //
        // TODO: Add constructor logic here
        //
        conn = new SqlConnection();

        conn.ConnectionString = Src;


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
