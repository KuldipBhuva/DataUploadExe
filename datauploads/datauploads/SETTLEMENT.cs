using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataUploads
{
    public class SETTLEMENT
    {

        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public SETTLEMENT()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }
        public string CompCode { get; set; }

        public string YearCode { get; set; }

        public string EmpNo { get; set; }

        public string BKCODE { get; set; }

        public string Settle_Trans { get; set; }

        public string ChequePrint { get; set; }

        public string PayTo { get; set; }

        public string PBankAccNo { get; set; }

        public DateTime? SettleDt { get; set; }

        public string Chqno { get; set; }

        public DateTime? ChqDt { get; set; }

        public string Remarks { get; set; }

        public string Vchno { get; set; }

        public DateTime? VchDt { get; set; }

        public decimal? OWNPF { get; set; }

        public decimal? VOLPF { get; set; }

        public decimal? COMPPF { get; set; }

        public decimal? OWNINT { get; set; }

        public decimal? VOLINT { get; set; }

        public decimal? COMPINT { get; set; }

        public decimal? NONREFLOAN { get; set; }

        public decimal? TDSAMT { get; set; }

        public decimal? SETTLEAMT { get; set; }

        public decimal? REFLOAN { get; set; }

        public decimal? LOANBAL { get; set; }

        public decimal? NEGSAL { get; set; }

        public string TRANSTOOTHFUNDS { get; set; }

        public string RTGS { get; set; }

        public string SET_PRINT { get; set; }

        public DateTime? PRINTDT { get; set; }

        public string MAKER { get; set; }

        public DateTime? MAKEDT { get; set; }

        public string CHECKED { get; set; }

        public string CHECKER { get; set; }

        public DateTime? CHECKDT { get; set; }

        public string APPROVED { get; set; }

        public string APPROVEUSER { get; set; }

        public DateTime? APPROVEDT { get; set; }

        public DateTime? CONFIRMEMAILDT { get; set; }

        public string INT_TRF_EMPNO { get; set; }

        public string cFiller01 { get; set; }

        public string cFiller02 { get; set; }

        public decimal? NFiller01 { get; set; }

        public decimal? NFiller02 { get; set; }

        public decimal? Tag { get; set; }

        public long autoserial { get; set; }

        public int? NoofServYear { get; set; }

        public decimal? CompContDed { get; set; }

        public decimal? CompIntDed { get; set; }

        public decimal? RateofDed { get; set; }

        public string DEATHNAME { get; set; }

        public string DEATHNAME1 { get; set; }

        public string DEATHNAME2 { get; set; }

        public string DEATHNAME3 { get; set; }

        public string DEATHNAME4 { get; set; }

        public decimal? DEATH { get; set; }

        public decimal? DisallowAmount { get; set; }

        public decimal? itax { get; set; }

        public decimal? itaxcess { get; set; }

        public decimal? itaxSurcharge { get; set; }

        public int? PFTPrint { get; set; }

        public decimal? TDSPERC { get; set; }

        public int? DED24GENERATED { get; set; }

        public string TDS_USERNAME { get; set; }

        public DateTime? TDS_LOGDATE { get; set; }

        public decimal? TDS_AMOUNT { get; set; }

        public bool TDeleteRecords { get; set; }
        public bool Insert()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertSETTLEMENT";
                cmd.Parameters.AddWithValue("@CompCode", this.CompCode);
                cmd.Parameters.AddWithValue("@YearCode", this.YearCode);
                cmd.Parameters.AddWithValue("@EmpNo", this.EmpNo);
                cmd.Parameters.AddWithValue("@BKCODE", this.BKCODE);
                cmd.Parameters.AddWithValue("@Settle_Trans", this.Settle_Trans);
                cmd.Parameters.AddWithValue("@ChequePrint", this.ChequePrint);
                cmd.Parameters.AddWithValue("@PayTo", this.PayTo);
                cmd.Parameters.AddWithValue("@PBankAccNo", this.PBankAccNo);
                cmd.Parameters.AddWithValue("@SettleDt", this.SettleDt);
                cmd.Parameters.AddWithValue("@Chqno", this.Chqno);
                cmd.Parameters.AddWithValue("@ChqDt", this.ChqDt);
                cmd.Parameters.AddWithValue("@Remarks", this.Remarks);
                cmd.Parameters.AddWithValue("@Vchno", this.Vchno);
                cmd.Parameters.AddWithValue("@VchDt", this.VchDt);
                cmd.Parameters.AddWithValue("@OWNPF", this.OWNPF);
                cmd.Parameters.AddWithValue("@VOLPF", this.VOLPF);
                cmd.Parameters.AddWithValue("@COMPPF", this.COMPPF);
                cmd.Parameters.AddWithValue("@OWNINT", this.OWNINT);
                cmd.Parameters.AddWithValue("@VOLINT", this.VOLINT);
                cmd.Parameters.AddWithValue("@COMPINT", this.COMPINT);
                cmd.Parameters.AddWithValue("@NONREFLOAN", this.NONREFLOAN);
                cmd.Parameters.AddWithValue("@TDSAMT", this.TDSAMT);
                cmd.Parameters.AddWithValue("@SETTLEAMT", this.SETTLEAMT);
                cmd.Parameters.AddWithValue("@REFLOAN", this.REFLOAN);
                cmd.Parameters.AddWithValue("@LOANBAL", this.LOANBAL);
                cmd.Parameters.AddWithValue("@NEGSAL", this.NEGSAL);
                cmd.Parameters.AddWithValue("@TRANSTOOTHFUNDS", this.TRANSTOOTHFUNDS);
                cmd.Parameters.AddWithValue("@RTGS", this.RTGS);
                cmd.Parameters.AddWithValue("@SET_PRINT", this.SET_PRINT);
                cmd.Parameters.AddWithValue("@PRINTDT", this.PRINTDT);
                cmd.Parameters.AddWithValue("@MAKER", this.MAKER);
                cmd.Parameters.AddWithValue("@MAKEDT", this.MAKEDT);
                cmd.Parameters.AddWithValue("@CHECKED", this.CHECKED);
                cmd.Parameters.AddWithValue("@CHECKER", this.CHECKER);
                cmd.Parameters.AddWithValue("@CHECKDT", this.CHECKDT);
                cmd.Parameters.AddWithValue("@APPROVED", this.APPROVED);
                cmd.Parameters.AddWithValue("@APPROVEUSER", this.APPROVEUSER);
                cmd.Parameters.AddWithValue("@APPROVEDT", this.APPROVEDT);
                cmd.Parameters.AddWithValue("@CONFIRMEMAILDT", this.CONFIRMEMAILDT);
                cmd.Parameters.AddWithValue("@INT_TRF_EMPNO", this.INT_TRF_EMPNO);
                cmd.Parameters.AddWithValue("@cFiller01", this.cFiller01);
                cmd.Parameters.AddWithValue("@cFiller02", this.cFiller02);
                cmd.Parameters.AddWithValue("@NFiller01", this.NFiller01);
                cmd.Parameters.AddWithValue("@NFiller02", this.NFiller02);
                cmd.Parameters.AddWithValue("@Tag", this.Tag);
                cmd.Parameters.AddWithValue("@autoserial", this.autoserial);
                cmd.Parameters.AddWithValue("@NoofServYear", this.NoofServYear);
                cmd.Parameters.AddWithValue("@CompContDed", this.CompContDed);
                cmd.Parameters.AddWithValue("@CompIntDed", this.CompIntDed);
                cmd.Parameters.AddWithValue("@RateofDed", this.RateofDed);
                cmd.Parameters.AddWithValue("@DEATHNAME", this.DEATHNAME);
                cmd.Parameters.AddWithValue("@DEATHNAME1", this.DEATHNAME1);
                cmd.Parameters.AddWithValue("@DEATHNAME2", this.DEATHNAME2);
                cmd.Parameters.AddWithValue("@DEATHNAME3", this.DEATHNAME3);
                cmd.Parameters.AddWithValue("@DEATHNAME4", this.DEATHNAME4);
                cmd.Parameters.AddWithValue("@DEATH", this.DEATH);
                cmd.Parameters.AddWithValue("@DisallowAmount", this.DisallowAmount);
                cmd.Parameters.AddWithValue("@itax", this.itax);
                cmd.Parameters.AddWithValue("@itaxcess", this.itaxcess);
                cmd.Parameters.AddWithValue("@itaxSurcharge", this.itaxSurcharge);
                cmd.Parameters.AddWithValue("@PFTPrint", this.PFTPrint);
                cmd.Parameters.AddWithValue("@TDSPERC", this.TDSPERC);
                cmd.Parameters.AddWithValue("@DED24GENERATED", this.DED24GENERATED);
                cmd.Parameters.AddWithValue("@TDS_USERNAME", this.TDS_USERNAME);
                cmd.Parameters.AddWithValue("@TDS_LOGDATE", this.TDS_LOGDATE);
                cmd.Parameters.AddWithValue("@TDS_AMOUNT", this.TDS_AMOUNT);
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
