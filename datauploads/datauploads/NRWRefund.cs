using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads
{
    public class NRWRefund
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public NRWRefund()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }
        public string COMPCODE { get; set; }

        public string YEARCODE { get; set; }

        public string EMPNO { get; set; }

        public string NRWTYPE { get; set; }

        public DateTime? AVAILDT { get; set; }

        public decimal? AVAILNRW { get; set; }

        public string VCHNO { get; set; }

        public DateTime? REFUNDDT { get; set; }

        public DateTime? EFFDATE { get; set; }

        public string CHQNO { get; set; }

        public decimal? NRWREFAMT { get; set; }

        public string BKCODE { get; set; }

        public decimal? COMPPF { get; set; }

        public decimal? OWNPF { get; set; }

        public decimal? VOLPF { get; set; }

        public decimal? TOTALREFAMT { get; set; }

        public DateTime? CHQDT { get; set; }

        public string REMARKS { get; set; }

        public string APPROVED { get; set; }

        public string APPROVUSER { get; set; }

        public DateTime? APPROVDT { get; set; }

        public decimal? TAG { get; set; }

        public string USER01 { get; set; }

        public DateTime LOGDT01 { get; set; }

        public string USER02 { get; set; }

        public DateTime? LOGDT02 { get; set; }

        public long AUTOSERIAL { get; set; }

        public string loantype { get; set; }

        public decimal? intcomppf { get; set; }

        public decimal? intownpf { get; set; }

        public decimal? intvolpf { get; set; }

        public decimal? RefundIntt { get; set; }

        public string RefLoanVch { get; set; }

        public bool TDeleteRecords { get; set; }

        public bool Insert()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertNRWRefund";
                cmd.Parameters.AddWithValue("@COMPCODE", this.COMPCODE);
                cmd.Parameters.AddWithValue("@YEARCODE", this.YEARCODE);
                cmd.Parameters.AddWithValue("@EMPNO", this.EMPNO);
                cmd.Parameters.AddWithValue("@NRWTYPE", this.NRWTYPE);
                cmd.Parameters.AddWithValue("@AVAILDT", this.AVAILDT);
                cmd.Parameters.AddWithValue("@AVAILNRW", this.AVAILNRW);
                cmd.Parameters.AddWithValue("@VCHNO", this.VCHNO);
                cmd.Parameters.AddWithValue("@REFUNDDT", this.REFUNDDT);
                cmd.Parameters.AddWithValue("@EFFDATE", this.EFFDATE);
                cmd.Parameters.AddWithValue("@CHQNO", this.CHQNO);
                cmd.Parameters.AddWithValue("@NRWREFAMT", this.NRWREFAMT);
                cmd.Parameters.AddWithValue("@BKCODE", this.BKCODE);
                cmd.Parameters.AddWithValue("@COMPPF", this.COMPPF);
                cmd.Parameters.AddWithValue("@OWNPF", this.OWNPF);
                cmd.Parameters.AddWithValue("@VOLPF", this.VOLPF);
                cmd.Parameters.AddWithValue("@TOTALREFAMT", this.TOTALREFAMT);
                cmd.Parameters.AddWithValue("@CHQDT", this.CHQDT);
                cmd.Parameters.AddWithValue("@REMARKS", this.REMARKS);
                cmd.Parameters.AddWithValue("@APPROVED", this.APPROVED);
                cmd.Parameters.AddWithValue("@APPROVUSER", this.APPROVUSER);
                cmd.Parameters.AddWithValue("@APPROVDT", this.APPROVDT);
                cmd.Parameters.AddWithValue("@TAG", this.TAG);
                cmd.Parameters.AddWithValue("@USER01", this.USER01);
                cmd.Parameters.AddWithValue("@LOGDT01", this.LOGDT01);
                cmd.Parameters.AddWithValue("@USER02", this.USER02);
                cmd.Parameters.AddWithValue("@LOGDT02", this.LOGDT02);
                cmd.Parameters.AddWithValue("@AUTOSERIAL", this.AUTOSERIAL);
                cmd.Parameters.AddWithValue("@loantype", this.loantype);
                cmd.Parameters.AddWithValue("@intcomppf", this.intcomppf);
                cmd.Parameters.AddWithValue("@intownpf", this.intownpf);
                cmd.Parameters.AddWithValue("@intvolpf", this.intvolpf);
                cmd.Parameters.AddWithValue("@RefundIntt", this.RefundIntt);
                cmd.Parameters.AddWithValue("@RefLoanVch", this.RefLoanVch);
                cmd.Parameters.AddWithValue("@tDeleteRecords", this.TDeleteRecords);

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
