using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads
{
    public class GrNomimast
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public GrNomimast()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }
        public int id { get; set; }

        public string compcode { get; set; }

        public string empno { get; set; }

        public string NomiName { get; set; }

        public int? rel { get; set; }

        public DateTime? dob { get; set; }

        public decimal? share { get; set; }

        bool _tDeleteRecords = false;
        public bool TDeleteRecords
        {
            get { return _tDeleteRecords; }
            set { _tDeleteRecords = value; }
        }

        public bool InsertGrNomimast()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertGrNomimast";
                //cmd.Parameters.AddWithValue("@id", this.id);
                cmd.Parameters.AddWithValue("@compcode", this.compcode);
                cmd.Parameters.AddWithValue("@empno", this.empno);
                cmd.Parameters.AddWithValue("@NomiName", this.NomiName);
                cmd.Parameters.AddWithValue("@rel", this.rel);
                cmd.Parameters.AddWithValue("@dob", this.dob);
                cmd.Parameters.AddWithValue("@share", this.share);
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
