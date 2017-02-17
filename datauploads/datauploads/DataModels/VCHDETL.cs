using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataUploads.DataModels
{
    public class VCHDETL
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;

        public VCHDETL()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }
        public string COMPCODE { get; set; }

        public string YEARCODE { get; set; }

        public string VCHNO { get; set; }

        public DateTime VCHDATE { get; set; }

        public string YEARMONTH { get; set; }

        public string EMPNO { get; set; }

        public string LOCCD { get; set; }

        public decimal? BASIC { get; set; }

        public decimal? GROSS { get; set; }

        public decimal? EPF { get; set; }

        public decimal? EFPF { get; set; }

        public decimal? CPF { get; set; }

        public decimal? CFPF { get; set; }

        public decimal? VPF { get; set; }

        public decimal? PFLOANRECO { get; set; }

        public string LOTNO { get; set; }

        public string CFILLER01 { get; set; }

        public decimal? NFILLER01 { get; set; }

        public decimal? NFILLER02 { get; set; }

        public DateTime? DFILLER01 { get; set; }

        public decimal TAG { get; set; }

        public string User01 { get; set; }

        public DateTime LogDt01 { get; set; }

        public string ORGFILENAME { get; set; }

        public string USER02 { get; set; }

        public DateTime? LOGDT02 { get; set; }

        public string UPDATEFILENAME { get; set; }

        public decimal? Imported { get; set; }

        public string ImportUser { get; set; }

        public DateTime? ImportDate { get; set; }

        public long autoserial { get; set; }

        public decimal? SA_Contribution { get; set; }

        public decimal? noofmon { get; set; }

        public decimal? PFLOANINT { get; set; }

        public decimal? SplPay { get; set; }

        public decimal? PenSalary { get; set; }

        public decimal? EDLISalary { get; set; }

        public int? Prnt { get; set; }

        public decimal? lossofpay { get; set; }

        public decimal? Sa_Vpf { get; set; }

        public DateTime? DepositDate { get; set; }

        public bool TDeleteRecords { get; set; }

        public bool Insert()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "InsertVCHDETL";
                cmd.Parameters.AddWithValue("@COMPCODE", this.COMPCODE);
                cmd.Parameters.AddWithValue("@YEARCODE", this.YEARCODE);
                cmd.Parameters.AddWithValue("@VCHNO", this.VCHNO);
                cmd.Parameters.AddWithValue("@VCHDATE", this.VCHDATE);
                cmd.Parameters.AddWithValue("@YEARMONTH", this.YEARMONTH);
                cmd.Parameters.AddWithValue("@EMPNO", this.EMPNO);
                cmd.Parameters.AddWithValue("@LOCCD", this.LOCCD);
                cmd.Parameters.AddWithValue("@BASIC", this.BASIC);
                cmd.Parameters.AddWithValue("@GROSS", this.GROSS);
                cmd.Parameters.AddWithValue("@EPF", this.EPF);
                cmd.Parameters.AddWithValue("@EFPF", this.EFPF);
                cmd.Parameters.AddWithValue("@CPF", this.CPF);
                cmd.Parameters.AddWithValue("@CFPF", this.CFPF);
                cmd.Parameters.AddWithValue("@VPF", this.VPF);
                cmd.Parameters.AddWithValue("@PFLOANRECO", this.PFLOANRECO);
                cmd.Parameters.AddWithValue("@LOTNO", this.LOTNO);
                cmd.Parameters.AddWithValue("@CFILLER01", this.CFILLER01);
                cmd.Parameters.AddWithValue("@NFILLER01", this.NFILLER01);
                cmd.Parameters.AddWithValue("@NFILLER02", this.NFILLER02);
                cmd.Parameters.AddWithValue("@DFILLER01", this.DFILLER01);
                cmd.Parameters.AddWithValue("@TAG", this.TAG);
                cmd.Parameters.AddWithValue("@User01", this.User01);
                cmd.Parameters.AddWithValue("@LogDt01", this.LogDt01);
                cmd.Parameters.AddWithValue("@ORGFILENAME", this.ORGFILENAME);
                cmd.Parameters.AddWithValue("@USER02", this.USER02);
                cmd.Parameters.AddWithValue("@LOGDT02", this.LOGDT02);
                cmd.Parameters.AddWithValue("@UPDATEFILENAME", this.UPDATEFILENAME);
                cmd.Parameters.AddWithValue("@Imported", this.Imported);
                cmd.Parameters.AddWithValue("@ImportUser", this.ImportUser);
                cmd.Parameters.AddWithValue("@ImportDate", this.ImportDate);
                cmd.Parameters.AddWithValue("@autoserial", this.autoserial);
                cmd.Parameters.AddWithValue("@SA_Contribution", this.SA_Contribution);
                cmd.Parameters.AddWithValue("@noofmon", this.noofmon);
                cmd.Parameters.AddWithValue("@PFLOANINT", this.PFLOANINT);
                cmd.Parameters.AddWithValue("@SplPay", this.SplPay);
                cmd.Parameters.AddWithValue("@PenSalary", this.PenSalary);
                cmd.Parameters.AddWithValue("@EDLISalary", this.EDLISalary);
                cmd.Parameters.AddWithValue("@Prnt", this.Prnt);
                cmd.Parameters.AddWithValue("@lossofpay", this.lossofpay);
                cmd.Parameters.AddWithValue("@Sa_Vpf", this.Sa_Vpf);
                cmd.Parameters.AddWithValue("@DepositDate", this.DepositDate);
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
