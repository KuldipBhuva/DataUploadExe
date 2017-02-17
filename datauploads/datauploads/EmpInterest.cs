using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads
{
    public class EmpInterest
    {
        #region Member variables
        string _compcode,
        _Empno,
        _EmpName,
        
        _uptodate,
        _YearCode;
       
        decimal _OWNINTT,
        _VOLINTT,
        _COMPINTT;
        bool _tDeleteRecords = false;
        public bool TDeleteRecords
        {
            get { return _tDeleteRecords; }
            set { _tDeleteRecords = value; }
        }

        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public string YearCode
        {
            get { return _YearCode; }
            set { _YearCode = value; }
        }

        public string Uptodate
        {
            get { return _uptodate; }
            set { _uptodate = value; }
        }

        public decimal COMPINTT
        {
            get { return _COMPINTT; }
            set { _COMPINTT = value; }
        }

        public decimal VOLINTT
        {
            get { return _VOLINTT; }
            set { _VOLINTT = value; }
        }

        public decimal OWNINTT
        {
            get { return _OWNINTT; }
            set { _OWNINTT = value; }
        }

        public string EmpName
        {
            get { return _EmpName; }
            set { _EmpName = value; }
        }

        public string Empno
        {
            get { return _Empno; }
            set { _Empno = value; }
        }

        public string Compcode
        {
            get { return _compcode; }
            set { _compcode = value; }
        }
        #endregion
        public EmpInterest()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }

        public bool InsertEmpInterest()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertEmpInterest";
                cmd.Parameters.AddWithValue("@compcode", this.Compcode);
                cmd.Parameters.AddWithValue("@Empno", this.Empno);
                cmd.Parameters.AddWithValue("@EmpName", this.EmpName);
                cmd.Parameters.AddWithValue("@OWNINTT", this.OWNINTT);
                cmd.Parameters.AddWithValue("@VOLINTT", this.VOLINTT);
                cmd.Parameters.AddWithValue("@COMPINTT", this.COMPINTT);
                cmd.Parameters.AddWithValue("@uptodate", Convert.ToDateTime(this.Uptodate));
                cmd.Parameters.AddWithValue("@YearCode", this.YearCode);
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
