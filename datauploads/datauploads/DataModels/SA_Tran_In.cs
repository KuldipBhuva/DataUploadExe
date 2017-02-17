using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads.DataModels
{
    public class SA_Tran_In
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public SA_Tran_In()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }
        public decimal Srno { get; set; }

        public string Compcode { get; set; }

        public string Empno { get; set; }

        public string Yearcode { get; set; }

        public string Empname { get; set; }

        public decimal Month { get; set; }

        public decimal Year { get; set; }

        public decimal Amount { get; set; }


        public bool _tDeleteRecords { get; set; }

        internal bool InsertSATranIn()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertSATranIn";
                cmd.Parameters.AddWithValue("@srno", this.Srno);
                cmd.Parameters.AddWithValue("@compcode", this.Compcode);
                cmd.Parameters.AddWithValue("@Empno", this.Empno);
                cmd.Parameters.AddWithValue("@EmpName", this.Empname);
                cmd.Parameters.AddWithValue("@YearCode", this.Yearcode);
                cmd.Parameters.AddWithValue("@Month", this.Month);
                cmd.Parameters.AddWithValue("@Year", this.Year);
                cmd.Parameters.AddWithValue("@Amount", this.Amount);
                cmd.Parameters.AddWithValue("@tDeleteRecords", this._tDeleteRecords);
                cmd.CommandTimeout = 1000000000;
                int iResult = cmd.ExecuteNonQuery();
                if (iResult != -1)
                    return true;
                return false;
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
