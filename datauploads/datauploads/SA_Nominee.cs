using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads
{
   public class SA_Nominee
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;
         
        public SA_Nominee()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }
        private decimal _Share;
        public decimal Share
        {
            get { return _Share; }
            set { _Share = value; }
        }
        private string _Rel
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

        public string REL
        {
            get { return _Rel; }
            set { _Rel = value; }
        }

        private DateTime? _Dob;

        public DateTime? DOB
        {
            get { return _Dob; }
            set { _Dob = value; }
        }
        public bool InsertSANominee()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertSANominee";
                cmd.Parameters.AddWithValue("@Compcode", this.Compcode);
                cmd.Parameters.AddWithValue("@Empno", this.Empno);
                cmd.Parameters.AddWithValue("@Empname", this.Empname);
                cmd.Parameters.AddWithValue("@Rel", this.REL);
                cmd.Parameters.AddWithValue("@DOB", this.DOB);
                cmd.Parameters.AddWithValue("@Share", this.Share);
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
