using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataUploads
{
    public class pfledgerall
    {

        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public pfledgerall()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }

        public string COMPCODE { get; set; }

        public string YEARCODE { get; set; }

        public string USERNAME { get; set; }

        public string EMPNO { get; set; }

        public string MONTHNAME { get; set; }

        public DateTime UPTODATE { get; set; }

        public double Serial { get; set; }

        public decimal OwnCont { get; set; }

        public decimal CoCont { get; set; }

        public decimal VolCont { get; set; }

        public decimal OwnInt { get; set; }

        public decimal CoInt { get; set; }

        public decimal VolInt { get; set; }

        public decimal Total { get; set; }

        public decimal Col8 { get; set; }

        public decimal Col9 { get; set; }

        public decimal Col10 { get; set; }

        public decimal Ibbowtot { get; set; }

        public decimal Ibbcotot { get; set; }

        public decimal Ibbvoltot { get; set; }

        public string Tag { get; set; }

        public decimal NoOfMonth { get; set; }

        public decimal MonthA { get; set; }

        public double EmpSerial { get; set; }

        public string monthdesc { get; set; }

        public string EmpName { get; set; }

        public string TranCode { get; set; }

        public string PfCode { get; set; }

        public decimal CFPF { get; set; }

        public string VCHNO { get; set; }

        public double Autoserial { get; set; }
        bool _tDeleteRecords = false;
        public bool TDeleteRecords
        {
            get { return _tDeleteRecords; }
            set { _tDeleteRecords = value; }
        }

        public bool InsertPFLedgerall()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertPFLedgerall";
                cmd.Parameters.AddWithValue("@COMPCODE", this.COMPCODE);
                cmd.Parameters.AddWithValue("@YEARCODE", this.YEARCODE);
                cmd.Parameters.AddWithValue("@USERNAME", this.USERNAME);
                cmd.Parameters.AddWithValue("@EMPNO", this.EMPNO);
                cmd.Parameters.AddWithValue("@MONTHNAME", this.MONTHNAME);
                cmd.Parameters.AddWithValue("@UPTODATE", this.UPTODATE);
                cmd.Parameters.AddWithValue("@Serial", this.Serial);
                cmd.Parameters.AddWithValue("@OwnCont", this.OwnCont);
                cmd.Parameters.AddWithValue("@CoCont", this.CoCont);
                cmd.Parameters.AddWithValue("@VolCont", this.VolCont);
                cmd.Parameters.AddWithValue("@OwnInt", this.OwnInt);
                cmd.Parameters.AddWithValue("@CoInt", this.CoInt);
                cmd.Parameters.AddWithValue("@VolInt", this.VolInt);
                cmd.Parameters.AddWithValue("@Total", this.Total);
                cmd.Parameters.AddWithValue("@Col8", this.Col8);
                cmd.Parameters.AddWithValue("@Col9", this.Col9);
                cmd.Parameters.AddWithValue("@Col10", this.Col10);
                cmd.Parameters.AddWithValue("@Ibbowtot", this.Ibbowtot);
                cmd.Parameters.AddWithValue("@Ibbcotot", this.Ibbcotot);
                cmd.Parameters.AddWithValue("@Ibbvoltot", this.Ibbvoltot);
                cmd.Parameters.AddWithValue("@Tag", this.Tag);
                cmd.Parameters.AddWithValue("@NoOfMonth", this.NoOfMonth);
                cmd.Parameters.AddWithValue("@MonthA", this.MonthA);
                cmd.Parameters.AddWithValue("@EmpSerial", this.EmpSerial);
                cmd.Parameters.AddWithValue("@monthdesc", this.monthdesc);
                cmd.Parameters.AddWithValue("@EmpName", this.EmpName);
                cmd.Parameters.AddWithValue("@TranCode", this.TranCode);
                cmd.Parameters.AddWithValue("@PfCode", this.PfCode);
                cmd.Parameters.AddWithValue("@CFPF", this.CFPF);
                cmd.Parameters.AddWithValue("@VCHNO", this.VCHNO);
                cmd.Parameters.AddWithValue("@Autoserial", this.Autoserial);
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
