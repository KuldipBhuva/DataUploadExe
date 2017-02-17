using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads.DataModels
{
    public class ExeUser
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public ExeUser()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAuthenticated { get; set; }

        public bool ExeUserLogin()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ExeUserLogin";
                cmd.Parameters.AddWithValue("@userName", this.Username);
                cmd.Parameters.AddWithValue("@password", this.Password);
                SqlParameter IsAuthenticated = new SqlParameter();
                IsAuthenticated.ParameterName = "@isAuthenticated";
                IsAuthenticated.Direction = ParameterDirection.Output;
                IsAuthenticated.SqlDbType = SqlDbType.Bit;
                cmd.Parameters.Add(IsAuthenticated);
                

                cmd.CommandTimeout = 1000000000;
                int iResult = cmd.ExecuteNonQuery();
                return Convert.ToBoolean(cmd.Parameters["@isAuthenticated"].Value);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                cn.CloseConnection();
            }

        }
    }
}
