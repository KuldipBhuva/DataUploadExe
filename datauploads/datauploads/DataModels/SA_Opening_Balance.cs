using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataUploads.DataModels
{
    public class SA_Opening_Balance
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public SA_Opening_Balance()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }

        public decimal SrNo { get; set; }

        public string Compcode { get; set; }

        public string Empno { get; set; }

        public string Yearcode { get; set; }

        public string Empname { get; set; }

        public decimal CEO_Bal { get; set; }

        public decimal XEO_Bal { get; set; }


        public bool InsertSAOpeningBalance()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertSAOpeningBalance";
                cmd.Parameters.AddWithValue("@srno", this.SrNo);
                cmd.Parameters.AddWithValue("@compcode", this.Compcode);
                cmd.Parameters.AddWithValue("@Empno", this.Empno);
                cmd.Parameters.AddWithValue("@EmpName", this.Empname);
                cmd.Parameters.AddWithValue("@YearCode", this.Yearcode);
                cmd.Parameters.AddWithValue("@CEO_Bal", this.CEO_Bal);
                cmd.Parameters.AddWithValue("@XEO_Bal", this.XEO_Bal);
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

        public object _tDeleteRecords { get; set; }
    }
}
