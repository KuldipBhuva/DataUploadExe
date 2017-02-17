using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads
{
    public class SA_Contribution
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;
         
        public SA_Contribution()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }
        private int _Srno, _April
, _May
, _June
, _July
, _August
, _September
, _October
, _November
, _December
, _January
, _February
, _March;

        public int March
        {
            get { return _March; }
            set { _March = value; }
        }

        public int February
        {
            get { return _February; }
            set { _February = value; }
        }

        public int January
        {
            get { return _January; }
            set { _January = value; }
        }

        public int December
        {
            get { return _December; }
            set { _December = value; }
        }

        public int November
        {
            get { return _November; }
            set { _November = value; }
        }

        public int October
        {
            get { return _October; }
            set { _October = value; }
        }

        public int September
        {
            get { return _September; }
            set { _September = value; }
        }

        public int August
        {
            get { return _August; }
            set { _August = value; }
        }

        public int July
        {
            get { return _July; }
            set { _July = value; }
        }

        public int June
        {
            get { return _June; }
            set { _June = value; }
        }

        public int May
        {
            get { return _May; }
            set { _May = value; }
        }

        public int April
        {
            get { return _April; }
            set { _April = value; }
        }


        public int Srno
        {
            get { return _Srno; }
            set { _Srno = value; }
        }

        private string _Yearcode
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



        public bool InsertSAContribution()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertSAContribution";
                cmd.Parameters.AddWithValue("@Srno", this.Srno);
                cmd.Parameters.AddWithValue("@Compcode", this.Compcode);
                cmd.Parameters.AddWithValue("@Empno", this.Empno);
                cmd.Parameters.AddWithValue("@Empname", this.Empname);
                cmd.Parameters.AddWithValue("@April", this.April);
                cmd.Parameters.AddWithValue("@May", this.May);
                cmd.Parameters.AddWithValue("@June", this.June);
                cmd.Parameters.AddWithValue("@July", this.July);
                cmd.Parameters.AddWithValue("@August", this.August);
                cmd.Parameters.AddWithValue("@September", this.September);
                cmd.Parameters.AddWithValue("@October", this.October);
                cmd.Parameters.AddWithValue("@November", this.November);
                cmd.Parameters.AddWithValue("@December", this.December);
                cmd.Parameters.AddWithValue("@January", this.January);
                cmd.Parameters.AddWithValue("@February", this.February);
                cmd.Parameters.AddWithValue("@March", this.March);
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
