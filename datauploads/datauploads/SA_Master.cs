using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads
{
    public class SA_Master
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public SA_Master()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }
        private int _Srno;
        public int Srno
        {
            get { return _Srno; }
            set { _Srno = value; }
        }

        private string _SANo
      , _Compcode
      , _Empno
      , _Empname;
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

        public string SANo
        {
            get { return _SANo; }
            set { _SANo = value; }
        }

        private DateTime? _sa_dom;

        public DateTime? sa_dom
        {
            get { return _sa_dom; }
            set { _sa_dom = value; }
        }

        public bool InsertSAMaster()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertSAMaster";
                cmd.Parameters.AddWithValue("@Srno", this.Srno);
                cmd.Parameters.AddWithValue("@Compcode", this.Compcode);
                cmd.Parameters.AddWithValue("@Empno", this.Empno);
                cmd.Parameters.AddWithValue("@SANo", this.SANo);
                cmd.Parameters.AddWithValue("@Empname", this.Empname);
                cmd.Parameters.AddWithValue("@SaDom", this.sa_dom);
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
