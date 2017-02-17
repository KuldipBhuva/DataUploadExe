using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads.DataModels
{
    public class SA_Interest
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public SA_Interest()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }

        private int SA_INT, EX_EMPLOYER_INT;
        public int SAInT
        {
            get { return SA_INT; }
            set { SA_INT = value; }
        }
        public int Ex_Employer
        {
            get { return EX_EMPLOYER_INT; }
            set { EX_EMPLOYER_INT = value; }
        }

        private string _Yearcode, _Compcode, _Empno, _Empname;
        bool _tDeleteRecords = false;
        public bool TDeleteRecords
        {
            get { return _tDeleteRecords; }
            set { _tDeleteRecords = value; }
        }
        public string Empname
        {
            get { return _Empname; }
            set { _Empname = value; }
        }

        public string Empno
        {
            get { return _Empno; }
            set { _Empno = value; }
        }

        public string Compcode
        {
            get { return _Compcode; }
            set { _Compcode = value; }
        }

        public string Yearcode
        {
            get { return _Yearcode; }
            set { _Yearcode = value; }
        }

        private DateTime _Uptodate;

        public DateTime Uptodate
        {
            get { return _Uptodate; }
            set { _Uptodate = value; }
        }

        public bool InsertSAInterest()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertSAInterest";
                cmd.Parameters.AddWithValue("@Compcode", this.Compcode);
                cmd.Parameters.AddWithValue("@Empno", this.Empno);
                cmd.Parameters.AddWithValue("@Empname", this.Empname);
                cmd.Parameters.AddWithValue("@Sa_Int", this.SAInT);
                cmd.Parameters.AddWithValue("@Ex_Employer_Int", this.Ex_Employer);
                cmd.Parameters.AddWithValue("@Uptodate", this.Uptodate);
                cmd.Parameters.AddWithValue("@Yearcode", this.Yearcode);
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
