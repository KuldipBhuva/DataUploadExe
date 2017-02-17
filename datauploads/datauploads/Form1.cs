using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DataAccessLayer;
using System.IO;
using DataUploads.DataModels;
namespace datauploads
{
    public partial class Form1 : Form
    {
        DataAccess dataAccess;
        DataTable dataTableYears;
        public Form1()
        {
            try
            {
                InitializeComponent();
                dataAccess = new DataAccess();
                dataTableYears = dataAccess.GetAllYears();

                BindDropDownList(dataTableYears);
            }
            catch (Exception ex)
            {
                dataAccess = new DataAccess();
                dataAccess.LogException(ex);
                MessageBox.Show(ex.Message);
            }





            //dataAccess = new DataAccess();
            //dataTableAuditYears = dataAccess.GetAuditedYear();
        }

        void BindDropDownList(DataTable dataTableSource)
        {
            try
            {

                DelYearCode.DisplayMember = "YearCode";
                DelYearCode.ValueMember = "YearCode";
                DelYearCode.DataSource = dataTableSource;



                cmbYear.DisplayMember = "YearCode";
                cmbYear.ValueMember = "YearCode";
                cmbYear.DataSource = dataTableSource;


                IBMYearcode.DisplayMember = "YearCode";
                IBMYearcode.ValueMember = "YearCode";
                IBMYearcode.DataSource = dataTableSource;



                WebYear.DisplayMember = "YearCode";
                WebYear.ValueMember = "YearCode";
                WebYear.DataSource = dataTableSource;
                DataRow[] drs = dataTableSource.Select(" iscurryear =" + 1 + "");
                if (drs.Length > 0)
                {
                    DelYearCode.SelectedValue = drs[0]["YearCode"];
                }
            }
            catch (Exception ex)
            {
                dataAccess = new DataAccess();
                dataAccess.LogException(ex);
                MessageBox.Show(ex.Message);
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void uploaddatatblwise(string tblname, string whrcondition)
        {
            try
            {

                DataAccess da = new DataAccess();
                da.TableName = tblname;
                da.WhereCondition = whrcondition;
                bool result = da.CheckDataExists();

                if (result)
                {
                    tabControlGenerateCSV.SelectedIndex = 1;
                    MessageBox.Show("Data already exist in this table for the given condition, please perform step 1 to delete existing data.");
                }
                else
                {
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = 100;
                    progressBar1.Value = 0;
                    progressBar1.Visible = true;

                    #region Please use below code to get connection string from app config.
                    //string Src = "Data Source=PERFECT1;Initial Catalog=VPDPTAX;Integrated Security=True";
                    //string Src = "Data Source=IBMSRV2;Initial Catalog=pfund;User ID=sa;Password=Server123";

                    //string Src = "Data Source=192.168.100.212,8001;Initial Catalog=pfund;User ID=newtech;Password=HfIO@5up_(Gbu2jhfYR";
                    string Src = "Data Source=192.168.100.212,8001;Initial Catalog=pfund;User ID=newtech;Password=Smile@12345";
                    string Dest = "Data Source=182.74.222.163,8001;Initial Catalog=VPDBack14Feb;User ID=newtech;Password=Smile@12345";

                    //string Dest = "Data Source=180.149.242.111,8001;Initial Catalog=VPDBack14Feb;User ID=sa;Password=HfIO@#54p_)gbu2jhfYR"; 
                    #endregion

                    //string Src = System.Configuration.ConfigurationManager.AppSettings.Get("Src");
                    //string Dest = System.Configuration.ConfigurationManager.AppSettings.Get("Dest");

                    int BatchSize = 500;
                    int NotifyAfter = 500;

                    SqlBulkCopy c = new SqlBulkCopy(Dest, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.TableLock);
                    dataAccess = new DataAccess();
                    string columns = dataAccess.GetTableColumns(tblname, "");

                    SqlConnection srcConn = new SqlConnection(Src);
                    srcConn.Open();

                    SqlCommand cm = new SqlCommand("select * from sysobjects where type='u' and name in ('" + tblname + "')");
                    cm.Connection = srcConn;
                    cm.CommandType = CommandType.Text;
                    cm.CommandTimeout = 0;
                    DataTable AllTables = new DataTable();
                    SqlDataAdapter a = new SqlDataAdapter(cm);
                    a.Fill(AllTables);
                    SqlDataReader dr;



                    int tot = AllTables.Rows.Count;
                    step2msg.Text = "Found " + tot + " Tables to copy.";
                    //Console.WriteLine("Found {0} Tables to copy.", tot);
                    //Console.WriteLine("");
                    step2msg.Text = step2msg.Text + System.Environment.NewLine;
                    int cnt = 1;
                    //progressBar1.PerformStep();
                    foreach (DataRow r in AllTables.Rows)
                    {

                        progressBar1.Value = cnt * (100 / tot);
                        Application.DoEvents();
                        progressBar1.PerformStep();
                        step2msg.Text = step2msg.Text + System.Environment.NewLine + "Copying table " + r["name"].ToString();

                        cm.CommandText = String.Format("select * from [{0}] " + whrcondition, r["name"]); // where code > 20
                        cm.CommandTimeout = 0;
                        dr = cm.ExecuteReader();
                        c.BatchSize = BatchSize;
                        c.BulkCopyTimeout = 0;
                        c.DestinationTableName = r["name"].ToString();
                        c.NotifyAfter = NotifyAfter;
                        c.WriteToServer(dr);
                        dr.Close();

                        step2msg.Text = step2msg.Text + " Finish Coping table " + r["name"].ToString();
                        cnt = cnt + 1;
                    }

                    srcConn.Close();
                    c.Close();
                    MessageBox.Show("Sucessfully Uploaded");
                    progressBar1.Visible = false;
                }



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                dataAccess = new DataAccess();
                dataAccess.LogException(ex);
                MessageBox.Show(ex.Message);
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {

                if (comboBoxUploadCondition.Text.ToString() == "Compcode")
                {
                    if (comboBoxTabletoUploadData.Text.ToString() == "form13")
                    {
                        uploaddatatblwise("form13", " where compcode='" + cmbCompany.Text.ToString() + "'  ");
                    }
                    else if (comboBoxTabletoUploadData.Text.ToString() == "empmast")
                    {
                        uploaddatatblwise("empmast", " where compcode='" + cmbCompany.Text.ToString() + "'  ");
                    }
                    else if (comboBoxTabletoUploadData.Text.ToString() == "empmast1")
                    {
                        uploaddatatblwise("empmast1", " where compcode='" + cmbCompany.Text.ToString() + "'  ");
                    }
                    else if (comboBoxTabletoUploadData.Text.ToString() == "emptransferin")
                    {
                        uploaddatatblwise("emptransferin", " where compcode='" + cmbCompany.Text.ToString() + "'  ");
                    }
                    else if (comboBoxTabletoUploadData.Text.ToString() == "unclaimedtransferin")
                    {
                        uploaddatatblwise("unclaimedtransferin", " where compcode='" + cmbCompany.Text.ToString() + "'  ");
                    }
                    else if (comboBoxTabletoUploadData.Text.ToString() == "loanmast")
                    {
                        uploaddatatblwise("loanmast", " where compcode='" + cmbCompany.Text.ToString() + "'  ");
                    }
                    else if (comboBoxTabletoUploadData.Text.ToString() == "nomimast")
                    {
                        uploaddatatblwise("nomimast", " where compcode='" + cmbCompany.Text.ToString() + "'  ");
                    }
                    else if (comboBoxTabletoUploadData.Text.ToString() == "pfledgerall")
                    {
                        uploaddatatblwise("pfledgerall", " where compcode='" + cmbCompany.Text.ToString() + "'  ");
                    }
                    //else if (comboBoxTabletoUploadData.Text.ToString() == "pfledgerall-Monthly")
                    //{
                    //    uploaddatatblwise("pfledgerall", " where compcode='" + cmbCompany.Text.ToString() + "'  and MONTHNAME= '" + comboBoxMonth.Text + "'");
                    //}
                }
                else if (comboBoxUploadCondition.Text.ToString() == "Compcode and year code")
                {
                    if (comboBoxTabletoUploadData.Text.ToString() == "form13")
                    {
                        string yearCode = cmbYear.Text;
                        string[] years = yearCode.Split('-');
                        uploaddatatblwise("form13", " where compcode='" + cmbCompany.Text.ToString() + "' and yearcode='" + DelYearCode.Text.ToString() + "' and vchdt >'" + years[0] + "-03-31'");
                    }
                    if (comboBoxTabletoUploadData.Text.ToString() == "empmast")
                    {
                        uploaddatatblwise("empmast", " where compcode='" + cmbCompany.Text.ToString() + "' and yearcode='" + cmbYear.Text + "' ");
                    }

                    else if (comboBoxTabletoUploadData.Text.ToString() == "empmast1")
                    {
                        uploaddatatblwise("empmast1", " where compcode='" + cmbCompany.Text.ToString() + "' and yearcode='" + cmbYear.Text + "' ");
                    }

                    else if (comboBoxTabletoUploadData.Text.ToString() == "emptransferin")
                    {
                        uploaddatatblwise("emptransferin", " where compcode='" + cmbCompany.Text.ToString() + "' and yearcode='" + cmbYear.Text + "' ");
                    }

                    else if (comboBoxTabletoUploadData.Text.ToString() == "unclaimedtransferin")
                    {
                        uploaddatatblwise("unclaimedtransferin", " where compcode='" + cmbCompany.Text.ToString() + "' and yearcode='" + cmbYear.Text + "' ");
                    }

                    else if (comboBoxTabletoUploadData.Text.ToString() == "loanmast")
                    {
                        uploaddatatblwise("loanmast", " where compcode='" + cmbCompany.Text.ToString() + "' and yearcode='" + cmbYear.Text + "' ");
                    }

                    else if (comboBoxTabletoUploadData.Text.ToString() == "nomimast")
                    {
                        uploaddatatblwise("nomimast", " where compcode='" + cmbCompany.Text.ToString() + "' and yearcode='" + cmbYear.Text + "' ");
                    }

                    else if (comboBoxTabletoUploadData.Text.ToString() == "pfledgerall")
                    {
                        uploaddatatblwise("pfledgerall", " where compcode='" + cmbCompany.Text.ToString() + "' and yearcode='" + cmbYear.Text + "' ");
                    }
                    else if (comboBoxTabletoUploadData.Text.ToString() == "pfledgerall-Monthly")
                    {
                        uploaddatatblwise("pfledgerall", " where compcode='" + cmbCompany.Text.ToString() + "'  and MONTHNAME= '" + comboBoxMonth.Text + "' and yearcode='" + cmbYear.Text + "' ");
                    }
                }
            }
            catch (Exception ex)
            {
                dataAccess = new DataAccess();
                dataAccess.LogException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DataAccess dataAccess = new DataAccess();
                dataAccess.YearCode = DelYearCode.SelectedText;
                dataAccess.CompCode = DelCompany.SelectedText;
                bool IsAuditYear = dataAccess.CheckAuditYear();
                if (IsAuditYear)
                {
                    if (MessageBox.Show("Selected year is audited, are you sure you want to delele audited year related table data? Click Yes to continue and No to cancel.", "Test", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                //if (MessageBox.Show("Selected year is audited, are you sure you want to delele audited year related table data? Click Yes to continue and No to cancel.", "Test", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //{
                ////string Dest = "Data Source=75.125.101.82;Initial Catalog=VPDPTAX;User ID=sausr;Password=newtech";

                //string Dest = "Data Source=180.149.242.111,8001;Initial Catalog=VPDBack14Feb;User ID=sa;Password=HfIO@#54p_)gbu2jhfYR";
                string Dest = "Data Source=182.74.222.163,8001;Initial Catalog=VPDBack14Feb;User ID=newtech;Password=Smile@12345";
                //string Dest = System.Configuration.ConfigurationManager.AppSettings.Get("Dest");
                SqlConnection cn = new SqlConnection(Dest);
                SqlCommand cmd = new SqlCommand();
                string str = "";
                if (Delcondition.Text.ToString() == "Delete Compcode")
                {
                    if (delTable.Text.ToString().Contains("pfledgerall-Monthly"))
                    {

                    }
                    else
                    {
                        str = "delete from " + delTable.Text.ToString() + " where compcode='" + DelCompany.Text.ToString() + "'";
                    }



                }
                else if (Delcondition.SelectedItem.ToString() == "Delete Compcode and year code")
                {
                    if (delTable.Text.ToString().Contains("Form13"))
                    {
                        string yearCode = delTable.Text.ToString();
                        string[] years = yearCode.Split('-');
                        str = "delete from " + delTable.Text.ToString() + " where compcode='" + DelCompany.Text.ToString() + "' and yearcode='" + DelYearCode.Text.ToString() + "' and vchdt >'" + years[0] + "-03-31'";
                    }
                    else if (delTable.Text.ToString().Contains("pfledgerall-Monthly"))
                    {
                        string yearCode = delTable.Text.ToString();
                        str = "delete from pfledgerall where compcode='" + DelCompany.Text.ToString() + "' and yearcode='" + DelYearCode.Text.ToString() + "' and monthname='" + comboBoxMonth.Text.ToString() + "'";
                    }
                    else
                    {
                        string yearCode = delTable.Text.ToString();
                        string[] years = yearCode.Split('-');
                        str = "delete from " + delTable.Text.ToString() + " where compcode='" + DelCompany.Text.ToString() + "' and yearcode='" + DelYearCode.Text.ToString() + "'";
                    }
                }
                try
                {
                    cmd = new SqlCommand(str, cn);
                    cn.Open();
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    if (Delcondition.Text.ToString() == "Delete Compcode")
                    {
                        DelMsg.Text = "Data deleted for company " + DelCompany.Text.ToString() + " for table " + delTable.Text.ToString();
                    }
                    else if (Delcondition.Text.ToString() == "Delete Compcode and year code")
                    {
                        if (delTable.Text.ToString().Contains("pfledgerall-Monthly"))
                        {
                            DelMsg.Text = "Data deleted for company " + DelCompany.Text.ToString() + "  for yearcode = " + DelYearCode.Text.ToString() + " table " + delTable.Text.ToString() + " With month name " + comboBoxMonth.Text.ToString();
                        }
                        else
                        {
                            DelMsg.Text = "Data deleted for company " + DelCompany.Text.ToString() + "  for yearcode = " + DelYearCode.Text.ToString() + " table " + delTable.Text.ToString();
                        }

                    }
                    buttonNext.Enabled = true;
                }
                catch (Exception ex)
                {
                    dataAccess = new DataAccess();
                    dataAccess.LogException(ex);
                    DelMsg.Text = ex.Message;
                    //MessageBox.Show(ex.Message);
                }
                buttonNext.Enabled = true;
                //}
                // else
                //{
                /// Code for ‘No’
                //}
            }
            catch (Exception ex)
            {
                dataAccess = new DataAccess();
                dataAccess.LogException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private bool TValidate()
        {
            //if()
            return true;
        }

        private void IbmGo_Click(object sender, EventArgs e)
        {
            try
            {
                //string Src = "Data Source=IBMSRV2;Initial Catalog=pfund;User ID=sa;Password=Server123";
                string Src = "Data Source=192.168.100.212,8001;Initial Catalog=pfund;User ID=newtech;Password=Smile@12345";
                //string Src = System.Configuration.ConfigurationManager.AppSettings.Get("Src");
                //string Src = "Data Source=180.149.242.111,8001;Initial Catalog=VPDBack14Feb;User ID=sa;Password=HfIO@#54p_)gbu2jhfYR";

                SqlConnection cn = new SqlConnection(Src);
                SqlCommand cmd = new SqlCommand();

                string str = "";
                if (IBMCondition.SelectedItem.ToString() == "Select Compcode")
                    str = "select count(1) from " + IBMTable.SelectedItem.ToString() + " where compcode='" + IBMComp.SelectedItem.ToString() + "'";
                else if (IBMCondition.SelectedItem.ToString() == "Select Compcode and year code")
                {
                    if (IBMTable.SelectedItem.ToString() == "form13")
                    {
                        string yearCode = WebYear.Text.ToString();
                        string[] years = yearCode.Split('-');
                        str = "select count(1) from " + IBMTable.SelectedItem.ToString() + " where compcode='" + IBMComp.SelectedItem.ToString() + "' and yearcode='" + IBMYearcode.Text.ToString() + "' and vchdt>'" + years[0] + "-03-31'";
                    }
                    else if (IBMTable.SelectedItem.ToString() == "pfledgerall-Monthly")
                    {
                        str = "select count(1) from " + IBMTable.SelectedItem.ToString() + " where compcode='" + IBMComp.SelectedItem.ToString() + "' and yearcode='" + IBMYearcode.Text.ToString().ToString() + "' and MonthName ='" + comboBoxIBMMonth.Text + "'";
                    }
                    else
                    {
                        str = "select count(1) from " + IBMTable.SelectedItem.ToString() + " where compcode='" + IBMComp.SelectedItem.ToString() + "' and yearcode='" + IBMYearcode.Text.ToString().ToString() + "'";
                    }
                }

                lblstring.Text = str;
                SqlDataAdapter adp;
                DataSet ds;
                cmd = new SqlCommand(str, cn);
                adp = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 1000;
                ds = new DataSet();
                adp.Fill(ds);




                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (IBMCondition.SelectedItem.ToString() == "Select Compcode")
                        IBMMsg.Text = ds.Tables[0].Rows[0][0].ToString() + " rows available in table " + IBMTable.SelectedItem.ToString() + " for company " + IBMComp.SelectedItem.ToString();
                    else
                        IBMMsg.Text = ds.Tables[0].Rows[0][0].ToString() + " rows available in table " + IBMTable.SelectedItem.ToString() + " for company " + IBMComp.SelectedItem.ToString() + " in year code " + IBMYearcode.SelectedItem.ToString();

                }
            }
            catch (Exception ex)
            {
                dataAccess = new DataAccess();
                dataAccess.LogException(ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void WebGo_Click(object sender, EventArgs e)
        {
            try
            {


                //string Dest = "Data Source=75.125.101.82;Initial Catalog=VPDPTAX;User ID=sausr;Password=newtech";
                //string Dest = "Data Source=180.149.242.111,8001;Initial Catalog=VPDBack14Feb;User ID=sa;Password=HfIO@#54p_)gbu2jhfYR";
                string Dest = "Data Source=182.74.222.163,8001;Initial Catalog=VPDBack14Feb;User ID=newtech;Password=Smile@12345";
                //string Dest = System.Configuration.ConfigurationManager.AppSettings.Get("Dest");
                SqlConnection cn = new SqlConnection(Dest);
                SqlCommand cmd = new SqlCommand();



                string str = "";
                string COndition = string.Empty;
                if (WebCondition.SelectedItem.ToString() == "Select Compcode" || WebTable.Text.ToString().Contains("SA_TRANSFER")
          || WebTable.Text.ToString().Contains("SA_Master") || WebTable.Text.ToString().Contains("SA_Nominee"))
                {
                    str = "select count(*) from " + WebTable.Text.ToString() + " where compcode='" + WebComapny.Text.ToString() + "'";
                    COndition = " compcode='" + WebComapny.Text.ToString() + "'";
                }
                else if (WebCondition.SelectedItem.ToString() == "Select Compcode and year code")
                {
                    if (WebTable.Text.ToString() == "form13")
                    {
                        string yearCode = WebYear.Text.ToString();
                        string[] years = yearCode.Split('-');
                        str = "select count(*) from " + WebTable.Text.ToString() + " where compcode='" + WebComapny.Text.ToString() + "' and yearcode='" + WebYear.Text.ToString() + "' and vchdt>'" + years[0] + "-03-31'";
                        COndition = " compcode='" + WebComapny.Text.ToString() + "' and yearcode='" + WebYear.Text.ToString() + "' and vchdt>'" + years[0] + "-03-31'";
                    }
                    else if (WebTable.Text.ToString() == "pfledgerall-Monthly")
                    {
                        str = "select count(*) from pfledgerall where compcode='" + WebComapny.Text.ToString() + "' and yearcode='" + WebYear.Text.ToString() + "' and MonthName= '" + comboBoxWebMonth.Text + "'";
                        COndition = " compcode='" + WebComapny.Text.ToString() + "' and yearcode='" + WebYear.Text.ToString() + "' and MonthName= '" + comboBoxWebMonth.Text + "'";
                    }
                    else
                    {
                        str = "select count(*) from " + WebTable.Text.ToString() + " where compcode='" + WebComapny.Text.ToString() + "' and yearcode='" + WebYear.Text.ToString() + "'";
                        //COndition = " compcode='" + WebComapny.Text.ToString() + "' and yearcode='" + WebYear.Text.ToString() + "'";  Commented By Keval Trivedi on 07-04-2014 to solve issue of last row of 'zzzzz' 
                        COndition = " compcode='" + WebComapny.Text.ToString() + "' and yearcode='" + WebYear.Text.ToString() + "' and UPPER(empno) <> 'ZZZZZ'";
                    }
                }

                if (!string.IsNullOrEmpty(COndition))
                {
                    if (WebTable.SelectedItem.ToString() == "pfledgerall-Monthly")
                    {
                        dataAccess = new DataAccess();
                        DataTable dataTableSumData = dataAccess.GetSumDataTable("pfledgerall", COndition);
                        dataGridView1.DataSource = dataTableSumData;
                    }
                    else
                    {
                        dataAccess = new DataAccess();
                        DataTable dataTableSumData = dataAccess.GetSumDataTable(WebTable.SelectedItem.ToString(), COndition);
                        dataGridView1.DataSource = dataTableSumData;
                    }

                }

                SqlDataAdapter adp;
                DataSet ds;
                cmd = new SqlCommand(str, cn);
                adp = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 1000;
                ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (WebCondition.SelectedItem.ToString() == "Select Compcode")
                        WebMsg.Text = ds.Tables[0].Rows[0][0].ToString() + " rows available in table " + WebTable.SelectedItem.ToString() + " for company " + WebComapny.SelectedItem.ToString();
                    else
                        WebMsg.Text = ds.Tables[0].Rows[0][0].ToString() + " rows available in table " + WebTable.SelectedItem.ToString() + " for company " + WebComapny.SelectedItem.ToString() + " in year code " + WebYear.SelectedItem.ToString();
                }
            }
            catch (Exception ex)
            {
                dataAccess = new DataAccess();
                dataAccess.LogException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void DelCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataAccess = new DataAccess();
            string strAuditYear = string.Empty;
            dataAccess.CompCode = Convert.ToString(DelCompany.SelectedItem);
            dataAccess.GetAuditedYear(out strAuditYear);
            DataTable dataTableAuditYears;
            if (!string.IsNullOrEmpty(strAuditYear))
            {
                DataRow[] drs = dataTableYears.Select(" YearCode <> '" + strAuditYear + "'");
                dataTableAuditYears = drs.CopyToDataTable();
                //BindDropDownList(dataTableAuditYears);

            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlGenerateCSV.SelectedIndex == 1)
            {
                if (!buttonNext.Enabled)
                {
                    MessageBox.Show("Step 2 is performed only after step 1's completion.");
                    tabControlGenerateCSV.SelectedIndex = 0;
                }
                else
                {
                    cmbCompany.SelectedIndex = DelCompany.SelectedIndex;
                    cmbYear.SelectedIndex = DelYearCode.SelectedIndex;
                    tabControlGenerateCSV.SelectedIndex = 1;
                }
            }

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCompany.SelectedIndex = DelCompany.SelectedIndex;
                cmbYear.SelectedIndex = DelYearCode.SelectedIndex;
                comboBoxMonthUpload.SelectedIndex = comboBoxMonth.SelectedIndex;
                tabControlGenerateCSV.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                dataAccess = new DataAccess();
                dataAccess.LogException(ex);
                MessageBox.Show(ex.Message);
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Delcondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxUploadCondition.SelectedIndex = Delcondition.SelectedIndex;
        }

        private void delTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxTabletoUploadData.SelectedIndex = delTable.SelectedIndex;
            if (delTable.Text == "pfledgerall-Monthly")
            {
                comboBoxMonth.Visible = true;
                //buttonMonthlyPfledger.Visible = true;
            }
            else
            {
                comboBoxMonth.Visible = false;
                //buttonMonthlyPfledger.Visible = false;
            }
        }

        private void buttonMonthlyPfledger_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxMonth.SelectedIndex = comboBoxMonth.SelectedIndex;
            //Delete.Enabled = false;
            //buttonNext.Enabled = true;
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void IBMTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IBMTable.Text == "pfledgerall-Monthly")
            {
                comboBoxIBMMonth.Visible = true;
                //buttonMonthlyPfledger.Visible = true;
            }
            else
            {
                comboBoxIBMMonth.Visible = false;
                //buttonMonthlyPfledger.Visible = false;
            }
        }

        private void WebTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WebTable.Text == "pfledgerall-Monthly")
            {
                comboBoxWebMonth.Visible = true;
                //buttonMonthlyPfledger.Visible = true;
            }
            else
            {
                comboBoxWebMonth.Visible = false;
                //buttonMonthlyPfledger.Visible = false;
            }
        }

        private void buttonBrowseFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialogInterest.ShowDialog();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(folderBrowserDialogInterest.SelectedPath))
            {
                MessageBox.Show("Please select folder path.");
                return;
            }
            string strDirectoryPath = folderBrowserDialogInterest.SelectedPath;
            SyncData(strDirectoryPath);

        }

        void SyncData(string strDirectoryPath)
        {
            string[] fileInfo = Directory.GetFiles(strDirectoryPath);

            string[] directoryInfo = Directory.GetDirectories(strDirectoryPath);
            EmpInterest empInterest;

            foreach (string directory in directoryInfo)
            {
                fileInfo = Directory.GetFiles(directory);
                if (fileInfo.Length > 0)
                {
                    foreach (string csvFile in fileInfo)
                    {
                        FileInfo xlsfileInfo = new FileInfo(csvFile);

                        var reader = new StreamReader(File.OpenRead(xlsfileInfo.FullName));
                        switch (Path.GetFileNameWithoutExtension(xlsfileInfo.Name).ToUpper())
                        {
                            case "SA_CONTRIBUTION":
                                #region SA_Contribution
                                SA_Contribution sa_Contribution;
                                int iCount = 0;
                                string strCompany = "", strYear = "";
                                string FileName = "sa_contribution";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCount != 0)
                                    {
                                        var values = line.Split(',');
                                        sa_Contribution = new SA_Contribution();
                                        sa_Contribution.Compcode = values[1];
                                        strCompany = sa_Contribution.Compcode;
                                        if (!string.IsNullOrEmpty(values[0]))
                                        sa_Contribution.Srno = Convert.ToInt32(values[0]);
                                        sa_Contribution.Compcode = values[1];
                                        sa_Contribution.Empno = values[2];
                                        sa_Contribution.Empname = values[3];
                                        if (values[4] != "" && values[4] != null)
                                        {
                                            sa_Contribution.Uptodate = Convert.ToDateTime(values[4]);
                                        }
                                        if (!string.IsNullOrEmpty(values[5]))
                                        sa_Contribution.April = Convert.ToInt32(values[5]);
                                        if (!string.IsNullOrEmpty(values[6]))
                                        sa_Contribution.May = Convert.ToInt32(values[6]);
                                        if (!string.IsNullOrEmpty(values[7]))
                                        sa_Contribution.June = Convert.ToInt32(values[7]);
                                        if (!string.IsNullOrEmpty(values[8]))
                                        sa_Contribution.July = Convert.ToInt32(values[8]);
                                        if (!string.IsNullOrEmpty(values[9]))
                                        sa_Contribution.August = Convert.ToInt32(values[9]);
                                        if (!string.IsNullOrEmpty(values[10]))
                                        sa_Contribution.September = Convert.ToInt32(values[10]);
                                        if (!string.IsNullOrEmpty(values[11]))
                                        sa_Contribution.October = Convert.ToInt32(values[11]);
                                        if (!string.IsNullOrEmpty(values[12]))
                                        sa_Contribution.November = Convert.ToInt32(values[12]);
                                        if (!string.IsNullOrEmpty(values[13]))
                                        sa_Contribution.December = Convert.ToInt32(values[13]);
                                        if (!string.IsNullOrEmpty(values[14]))
                                        sa_Contribution.January = Convert.ToInt32(values[14]);
                                        if (!string.IsNullOrEmpty(values[15]))
                                        sa_Contribution.February = Convert.ToInt32(values[15]);
                                        if (!string.IsNullOrEmpty(values[16]))
                                        sa_Contribution.March = Convert.ToInt32(values[16]);
                                        sa_Contribution.Yearcode = values[17];

                                        sa_Contribution.Empname = values[3];

                                        strYear = sa_Contribution.Yearcode;
                                        if (iCount == 1)
                                            sa_Contribution.TDeleteRecords = true;

                                        if (sa_Contribution.InsertSAContribution())
                                            labelStatus.Text = string.Format("Info : File Name : {3},Company : {1} - Year : {2} , {0} row(s) inserted.", iCount, strCompany, strYear, FileName);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }

                                    }
                                    iCount++;
                                }

                                lblResult.Text += string.Format("File Name : {3},Company : {0}, Year : {1}, Count : {2}\n", strCompany, strYear, iCount - 1, FileName);
                                #endregion
                                break;
                            case "VCHDETL":
                                #region VCHDETL
                                iCount = 0;
                                strCompany = ""; strYear = "";
                                FileName = "VCHDETL";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCount != 0)
                                    {
                                        var values = line.Split(',');
                                        VCHDETL vchdetl = new VCHDETL();
                                        vchdetl.COMPCODE = values[0];

                                        vchdetl.YEARCODE = values[1];

                                        vchdetl.VCHNO = values[2];
                                        if (values[3] != "" && values[3] != null && values[3]!="0.0")
                                        {
                                            vchdetl.VCHDATE = Convert.ToDateTime(values[3]);
                                        }
                                        vchdetl.YEARMONTH = values[4];

                                        vchdetl.EMPNO = values[5];

                                        vchdetl.LOCCD = values[6];
                                        if (!string.IsNullOrEmpty(values[7]))
                                        vchdetl.BASIC = Convert.ToDecimal(values[7]);
                                        if (!string.IsNullOrEmpty(values[8]))
                                        vchdetl.GROSS = Convert.ToDecimal(values[8]);
                                        if (!string.IsNullOrEmpty(values[9]))
                                        vchdetl.EPF = Convert.ToDecimal(values[9]);
                                        if (!string.IsNullOrEmpty(values[10]))
                                        vchdetl.EFPF = Convert.ToDecimal(values[10]);
                                        if (!string.IsNullOrEmpty(values[11]))
                                        vchdetl.CPF = Convert.ToDecimal(values[11]);
                                        if (!string.IsNullOrEmpty(values[12]))
                                        vchdetl.CFPF = Convert.ToDecimal(values[12]);
                                        if (!string.IsNullOrEmpty(values[13]))
                                        vchdetl.VPF = Convert.ToDecimal(values[13]);
                                        if (!string.IsNullOrEmpty(values[14]))
                                        vchdetl.PFLOANRECO = Convert.ToDecimal(values[14]);

                                        vchdetl.LOTNO = values[15];

                                        vchdetl.CFILLER01 = values[16];
                                        if (!string.IsNullOrEmpty(values[17]))
                                        vchdetl.NFILLER01 = Convert.ToDecimal(values[17]);
                                        if (!string.IsNullOrEmpty(values[18]))
                                        vchdetl.NFILLER02 = Convert.ToDecimal(values[18]);
                                        if (values[19] != "" && values[19] != null)
                                        {
                                            vchdetl.DFILLER01 = Convert.ToDateTime(values[19]);
                                        }
                                        if (!string.IsNullOrEmpty(values[20]))
                                        vchdetl.TAG = Convert.ToDecimal(values[20]);

                                        vchdetl.User01 = values[21];

                                        if (values[22] != "" && values[22] != null)
                                        {
                                            vchdetl.LogDt01 = Convert.ToDateTime(values[22]);
                                        }

                                        vchdetl.ORGFILENAME = values[23];

                                        vchdetl.USER02 = values[24];
                                        if (values[25] != "" && values[25] != null)
                                        {
                                            vchdetl.LOGDT02 = Convert.ToDateTime(values[25]);
                                        }

                                        vchdetl.UPDATEFILENAME = values[26];
                                        if (!string.IsNullOrEmpty(values[27]))
                                        vchdetl.Imported = Convert.ToDecimal(values[27]);

                                        vchdetl.ImportUser = values[28];
                                        if (values[29] != "" && values[29] != null)
                                        {
                                            vchdetl.ImportDate = Convert.ToDateTime(values[29]);
                                        }
                                        if (!string.IsNullOrEmpty(values[31]))
                                        vchdetl.SA_Contribution = Convert.ToDecimal(values[31]);
                                        if (!string.IsNullOrEmpty(values[32]))
                                        vchdetl.noofmon = Convert.ToDecimal(values[32]);
                                        if (!string.IsNullOrEmpty(values[33]))
                                        vchdetl.PFLOANINT = Convert.ToDecimal(values[33]);
                                        if (!string.IsNullOrEmpty(values[34]))
                                        vchdetl.SplPay = Convert.ToDecimal(values[34]);
                                        if (!string.IsNullOrEmpty(values[35]))
                                        vchdetl.PenSalary = Convert.ToDecimal(values[35]);
                                        if (!string.IsNullOrEmpty(values[36]))
                                        vchdetl.EDLISalary = Convert.ToDecimal(values[36]);
                                        if (!string.IsNullOrEmpty(values[37]))
                                        vchdetl.Prnt = Convert.ToInt32(values[37]);
                                        if (!string.IsNullOrEmpty(values[38]))
                                        vchdetl.lossofpay = Convert.ToDecimal(values[38]);
                                        if (!string.IsNullOrEmpty(values[39]))
                                        vchdetl.Sa_Vpf = Convert.ToDecimal(values[39]);
                                        if (values[40] != "" && values[40] != null)
                                        {
                                            vchdetl.DepositDate = Convert.ToDateTime(values[40]);
                                        }
                                        strYear = vchdetl.YEARCODE;
                                        if (iCount == 1)
                                            vchdetl.TDeleteRecords = true;

                                        if (vchdetl.Insert())
                                            labelStatus.Text = string.Format("Info : File Name : {3},Company : {1} - Year : {2} , {0} row(s) inserted.", iCount, strCompany, strYear, FileName);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }
                                    }
                                    iCount++;
                                }
                                #endregion
                                break;
                            case "SETTLEMENT":
                                #region SETTLEMENT
                                iCount = 0;
                                strCompany = ""; strYear = "";
                                FileName = "SETTLEMENT";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCount != 0)
                                    {
                                        var values = line.Split(',');
                                        SETTLEMENT settlement = new SETTLEMENT();
                                        settlement.CompCode = values[0];

                                        settlement.YearCode = values[1];

                                        settlement.EmpNo = values[2];

                                        settlement.BKCODE = values[3];

                                        settlement.Settle_Trans = values[4];

                                        settlement.ChequePrint = values[5];

                                        settlement.PayTo = values[6];

                                        settlement.PBankAccNo = values[7];
                                        if (values[8] != "" && values[8] != null)
                                        {
                                            settlement.SettleDt = Convert.ToDateTime(values[8]);
                                        }
                                        settlement.Chqno = values[9];
                                        if (values[10] != "" && values[10] != null)
                                        {
                                            settlement.ChqDt = Convert.ToDateTime(values[10]);
                                        }
                                        settlement.Remarks = values[11];

                                        settlement.Vchno = values[12];
                                        if (values[13] != "" && values[13] != null)
                                        {
                                            settlement.VchDt = Convert.ToDateTime(values[13]);
                                        }
                                        if (!string.IsNullOrEmpty(values[14]))
                                        settlement.OWNPF = Convert.ToDecimal(values[14]);
                                        if (!string.IsNullOrEmpty(values[15]))
                                        settlement.VOLPF = Convert.ToDecimal(values[15]);
                                        if (!string.IsNullOrEmpty(values[16]))
                                        settlement.COMPPF = Convert.ToDecimal(values[16]);
                                        if (!string.IsNullOrEmpty(values[17]))
                                        settlement.OWNINT = Convert.ToDecimal(values[17]);
                                        if (!string.IsNullOrEmpty(values[18]))
                                        settlement.VOLINT = Convert.ToDecimal(values[18]);
                                        if (!string.IsNullOrEmpty(values[19]))
                                        settlement.COMPINT = Convert.ToDecimal(values[19]);
                                        if (!string.IsNullOrEmpty(values[20]))
                                        settlement.NONREFLOAN = Convert.ToDecimal(values[20]);
                                        if (!string.IsNullOrEmpty(values[21]))
                                        settlement.TDSAMT = Convert.ToDecimal(values[21]);
                                        if (!string.IsNullOrEmpty(values[22]))
                                        settlement.SETTLEAMT = Convert.ToDecimal(values[22]);
                                        if (!string.IsNullOrEmpty(values[23]))
                                        settlement.REFLOAN = Convert.ToDecimal(values[23]);
                                        if (!string.IsNullOrEmpty(values[24]))
                                        settlement.LOANBAL = Convert.ToDecimal(values[24]);
                                        if (!string.IsNullOrEmpty(values[25]))
                                        settlement.NEGSAL = Convert.ToDecimal(values[25]);
                                        
                                        settlement.TRANSTOOTHFUNDS = values[26];

                                        settlement.RTGS = values[27];

                                        settlement.SET_PRINT = values[28];
                                        if (values[29] != "" && values[29] != null)
                                        {
                                            settlement.PRINTDT = Convert.ToDateTime(values[29]);
                                        }
                                        settlement.MAKER = values[30];
                                        if (values[31] != "" && values[31] != null)
                                        {
                                            settlement.MAKEDT = Convert.ToDateTime(values[31]);
                                        }
                                        settlement.CHECKED = values[32];

                                        settlement.CHECKER = values[33];
                                        if (values[34] != "" && values[34] != null)
                                        {
                                            settlement.CHECKDT = Convert.ToDateTime(values[34]);
                                        }
                                        settlement.APPROVED = values[35];

                                        settlement.APPROVEUSER = values[36];
                                        if (values[37] != "" && values[37] != null)
                                        {
                                            settlement.APPROVEDT = Convert.ToDateTime(values[37]);
                                        }
                                        if (values[38] != "" && values[38] != null)
                                        {
                                            settlement.CONFIRMEMAILDT = Convert.ToDateTime(values[38]);
                                        }
                                        settlement.INT_TRF_EMPNO = values[39];

                                        settlement.cFiller01 = values[40];

                                        settlement.cFiller02 = values[41];
                                        if (!string.IsNullOrEmpty(values[42]))
                                        settlement.NFiller01 = Convert.ToDecimal(values[42]);
                                        if (!string.IsNullOrEmpty(values[43]))
                                        settlement.NFiller02 = Convert.ToDecimal(values[43]);

                                        settlement.Tag = Convert.ToDecimal(values[44]);
                                        if (!string.IsNullOrEmpty(values[46]))
                                        settlement.NoofServYear = Convert.ToInt32(values[46]);
                                        if (!string.IsNullOrEmpty(values[47]))
                                        settlement.CompContDed = Convert.ToDecimal(values[47]);
                                        if (!string.IsNullOrEmpty(values[48]))
                                        settlement.CompIntDed = Convert.ToDecimal(values[48]);
                                        if (!string.IsNullOrEmpty(values[49]))
                                        settlement.RateofDed = Convert.ToDecimal(values[49]);

                                        settlement.DEATHNAME = values[50];

                                        settlement.DEATHNAME1 = values[51];

                                        settlement.DEATHNAME2 = values[52];

                                        settlement.DEATHNAME3 = values[53];

                                        settlement.DEATHNAME4 = values[54];
                                        if (!string.IsNullOrEmpty(values[55]))
                                        settlement.DEATH = Convert.ToDecimal(values[55]);
                                        if (!string.IsNullOrEmpty(values[56]))
                                        settlement.DisallowAmount = Convert.ToDecimal(values[56]);
                                        if (!string.IsNullOrEmpty(values[57]))
                                        settlement.itax = Convert.ToDecimal(values[57]);
                                        if (!string.IsNullOrEmpty(values[58]))
                                        settlement.itaxcess = Convert.ToDecimal(values[58]);
                                        if (!string.IsNullOrEmpty(values[59]))
                                        settlement.itaxSurcharge = Convert.ToDecimal(values[59]);
                                        if (!string.IsNullOrEmpty(values[60]))
                                        settlement.PFTPrint = Convert.ToInt32(values[60]);
                                        if (!string.IsNullOrEmpty(values[61]))
                                        settlement.TDSPERC = Convert.ToDecimal(values[61]);
                                        if (!string.IsNullOrEmpty(values[62]))
                                        settlement.DED24GENERATED = Convert.ToInt32(values[62]);

                                        settlement.TDS_USERNAME = values[63];
                                        if (values[64] != "" && values[64] != null)
                                        {
                                            settlement.TDS_LOGDATE = Convert.ToDateTime(values[64]);
                                        }
                                        if (!string.IsNullOrEmpty(values[65]))
                                        settlement.TDS_AMOUNT = Convert.ToDecimal(values[65]);

                                        strYear = settlement.YearCode;
                                        if (iCount == 1)
                                            settlement.TDeleteRecords = true;

                                        if (settlement.Insert())
                                            labelStatus.Text = string.Format("Info : File Name : {3},Company : {1} - Year : {2} , {0} row(s) inserted.", iCount, strCompany, strYear, FileName);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }
                                    }
                                    iCount++;
                                }
                                #endregion
                                break;
                            case "NRWREFUND":
                                #region VCHDETL
                                iCount = 0;
                                strCompany = ""; strYear = "";
                                FileName = "NRWRefund";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCount != 0)
                                    {
                                        var values = line.Split(',');
                                        NRWRefund nrwRefund = new NRWRefund();
                                        nrwRefund.COMPCODE = values[0];

                                        nrwRefund.YEARCODE = values[1];

                                        nrwRefund.EMPNO = values[2];

                                        nrwRefund.NRWTYPE = values[3];
                                        if (values[4] != "" && values[4] != null)
                                        {
                                            nrwRefund.AVAILDT = Convert.ToDateTime(values[4]);
                                        }
                                        if (!string.IsNullOrEmpty(values[5]))
                                        nrwRefund.AVAILNRW = Convert.ToDecimal(values[5]);

                                        nrwRefund.VCHNO = values[6];
                                        if (values[7] != "" && values[7] != null)
                                        {
                                            nrwRefund.REFUNDDT = Convert.ToDateTime(values[7]);
                                        }
                                        if (values[8] != "" && values[8]!=null)
                                        {
                                            nrwRefund.EFFDATE = Convert.ToDateTime(values[8]);
                                        }
                                        nrwRefund.CHQNO = values[9];
                                        if (values[10] != "" && values[10] != null)
                                        {
                                            nrwRefund.NRWREFAMT = Convert.ToDecimal(values[10]);
                                        }
                                        nrwRefund.BKCODE = values[11];
                                        if (!string.IsNullOrEmpty(values[12]))
                                        nrwRefund.COMPPF = Convert.ToDecimal(values[12]);
                                        if (!string.IsNullOrEmpty(values[13]))
                                        nrwRefund.OWNPF = Convert.ToDecimal(values[13]);
                                        if (!string.IsNullOrEmpty(values[14]))
                                        nrwRefund.VOLPF = Convert.ToDecimal(values[14]);
                                        if (!string.IsNullOrEmpty(values[15]))
                                        nrwRefund.TOTALREFAMT = Convert.ToDecimal(values[15]);
                                        if (values[16] != "" && values[16] != null)
                                        {
                                            nrwRefund.CHQDT = Convert.ToDateTime(values[16]);
                                        }
                                        nrwRefund.REMARKS = values[17];

                                        nrwRefund.APPROVED = values[18];

                                        nrwRefund.APPROVUSER = values[19];
                                        if (values[20] != "" && values[20] != null)
                                        {
                                            nrwRefund.APPROVDT = Convert.ToDateTime(values[20]);
                                        }
                                        if (!string.IsNullOrEmpty(values[21]))
                                        nrwRefund.TAG = Convert.ToDecimal(values[21]);

                                        nrwRefund.USER01 = values[22];
                                        if (values[23] != "" && values[23] != null)
                                        {
                                            nrwRefund.LOGDT01 = Convert.ToDateTime(values[23]);
                                        }
                                        nrwRefund.USER02 = values[24];
                                        if (values[25] != "" && values[25] != null)
                                        {
                                            nrwRefund.LOGDT02 = Convert.ToDateTime(values[25]);
                                        }
                                        nrwRefund.loantype = values[27];
                                        if (!string.IsNullOrEmpty(values[28]))
                                        nrwRefund.intcomppf = Convert.ToDecimal(values[28]);
                                        if (!string.IsNullOrEmpty(values[29]))
                                        nrwRefund.intownpf = Convert.ToDecimal(values[29]);
                                        if (!string.IsNullOrEmpty(values[30]))
                                        nrwRefund.intvolpf = Convert.ToDecimal(values[30]);
                                        if (!string.IsNullOrEmpty(values[31]))
                                        nrwRefund.RefundIntt = Convert.ToDecimal(values[31]);

                                        nrwRefund.RefLoanVch = values[32];

                                        strYear = nrwRefund.YEARCODE;
                                        if (iCount == 1)
                                            nrwRefund.TDeleteRecords = true;

                                        if (nrwRefund.Insert())
                                            labelStatus.Text = string.Format("Info : File Name : {3},Company : {1} - Year : {2} , {0} row(s) inserted.", iCount, strCompany, strYear, FileName);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }
                                    }
                                    iCount++;
                                }
                                #endregion
                                break;
                            case "GRNOMIMAST":
                                #region VCHDETL
                                iCount = 0;
                                strCompany = ""; strYear = "";
                                FileName = "GrNomimast";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCount != 0)
                                    {
                                        var values = line.Split(',');
                                        GrNomimast grNomimast = new GrNomimast();

                                        grNomimast.compcode = values[0];

                                        grNomimast.empno = values[1];

                                        grNomimast.NomiName = values[2];
                                        if (!string.IsNullOrEmpty(values[3]))
                                            grNomimast.rel = Convert.ToInt32(values[3]);

                                        if (!string.IsNullOrEmpty(values[4]))
                                            grNomimast.dob = Convert.ToDateTime(values[4]);
                                        if (!string.IsNullOrEmpty(values[5]))
                                            grNomimast.share = Convert.ToDecimal(values[5]);
                                        if (iCount == 1)
                                            grNomimast.TDeleteRecords = true;

                                        if (grNomimast.InsertGrNomimast())
                                            labelStatus.Text = string.Format("Info : File Name : {3},Company : {1} - Year : {2} , {0} row(s) inserted.", iCount, strCompany, strYear, FileName);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }
                                    }
                                    iCount++;
                                }
                                #endregion
                                break;
                            case "SA_INTEREST":
                                #region SA_Interest
                                SA_Interest sa_interests;
                                int iCountinterest = 0;
                                string strComPany = "", strYeaR = "";
                                string FiLeNaMe = "sa_interest";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCountinterest != 0)
                                    {
                                        var values = line.Split(',');
                                        sa_interests = new SA_Interest();
                                        sa_interests.Compcode = values[0];
                                        strComPany = sa_interests.Compcode;
                                        sa_interests.Compcode = values[0];
                                        sa_interests.Empno = values[1];
                                        sa_interests.Empname = values[2];
                                        if (!string.IsNullOrEmpty(values[3]))
                                        sa_interests.SAInT = Convert.ToInt32(values[3]);
                                        if (!string.IsNullOrEmpty(values[4]))
                                        sa_interests.Ex_Employer = Convert.ToInt32(values[4]);
                                        if (!string.IsNullOrEmpty(values[5]))
                                        {
                                            sa_interests.Uptodate = Convert.ToDateTime(values[5]);
                                        }
                                        sa_interests.Yearcode = values[6];

                                        strYeaR = sa_interests.Yearcode;
                                        if (iCountinterest == 1)
                                            sa_interests.TDeleteRecords = true;

                                        if (sa_interests.InsertSAInterest())
                                            labelStatus.Text = string.Format("Info : File Name : {3},Company : {1} - Year : {2} , {0} row(s) inserted.", iCountinterest, strComPany, strYeaR, FiLeNaMe);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }

                                    }
                                    iCountinterest++;
                                }

                                lblResult.Text += string.Format("File Name : {3},Company : {0}, Year : {1}, Count : {2}\n", strComPany, strYeaR, iCountinterest - 1, FiLeNaMe);
                                #endregion
                                break;

                            case "SA_MASTER":
                                #region SA_Master
                                SA_Master sa_Master;
                                int iCountMaster = 0;
                                string strCompanyMaster = "";
                                string filename = "sa_master";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCountMaster != 0)
                                    {
                                        var values = line.Split(',');
                                        sa_Master = new SA_Master();
                                        sa_Master.Compcode = values[1];
                                        strCompanyMaster = sa_Master.Compcode;
                                        if (!string.IsNullOrEmpty(values[0]))
                                        sa_Master.Srno = Convert.ToInt32(values[0]);
                                        sa_Master.Compcode = values[1];
                                        sa_Master.Empno = values[2];
                                        sa_Master.SANo = values[3];
                                        sa_Master.Empname = values[4];
                                        if (!string.IsNullOrEmpty(values[5].Trim()))
                                            sa_Master.sa_dom = Convert.ToDateTime(values[5]);
                                        else
                                            sa_Master.sa_dom = (DateTime?)null;

                                        if (iCountMaster == 1)
                                            sa_Master.TDeleteRecords = true;

                                        if (sa_Master.InsertSAMaster())
                                            labelStatus.Text = string.Format("Info : File Name : {2},Company : {1} -, {0} row(s) inserted.", iCountMaster, strCompanyMaster, filename);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }

                                    }
                                    iCountMaster++;
                                }

                                lblResult.Text += string.Format("File Name : {2},Company : {0}, Count : {1}\n", strCompanyMaster, iCountMaster - 1, filename);
                                #endregion
                                break;

                            case "SA_NOMINEE":
                                #region SA_Nominee
                                SA_Nominee sa_Nominee;
                                int iCountNom = 0;
                                string strComPanyNom = "";
                                string fileName = "sa_nominee";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCountNom != 0)
                                    {
                                        var values = line.Split(',');
                                        sa_Nominee = new SA_Nominee();
                                        sa_Nominee.Compcode = values[0];
                                        strComPanyNom = sa_Nominee.Compcode;
                                        sa_Nominee.Compcode = values[0];
                                        sa_Nominee.Empno = values[1];
                                        sa_Nominee.Empname = values[2];
                                        sa_Nominee.REL = values[3];
                                        if (!string.IsNullOrEmpty(values[4].Trim()))
                                        {
                                            sa_Nominee.DOB = Convert.ToDateTime(values[4]);
                                        }
                                        else
                                        {
                                            sa_Nominee.DOB = (DateTime?)null;
                                        }
                                        if (!string.IsNullOrEmpty(values[5].Trim()))
                                            sa_Nominee.Share = Convert.ToDecimal(values[5]);
                                        if (iCountNom == 1)
                                            sa_Nominee.TDeleteRecords = true;

                                        if (sa_Nominee.InsertSANominee())
                                            labelStatus.Text = string.Format("Info : File Name : {2},Company : {1} - , {0} row(s) inserted.", iCountNom, strComPanyNom, fileName);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }

                                    }
                                    iCountNom++;
                                }

                                lblResult.Text += string.Format("File Name : {2},Company : {0}, Count : {1}\n", strComPanyNom, iCountNom - 1, fileName);
                                #endregion
                                break;

                            case "EMP_INTEREST":
                                #region Emp_Interest
                                int iCountInterest = 0;
                                string strCompanyInterest = "", strYearInterest = "";
                                string fileNaME = "emp_interest";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCountInterest != 0)
                                    {
                                        var values = line.Split(',');
                                        empInterest = new EmpInterest();
                                        empInterest.Compcode = values[0];
                                        strCompanyInterest = empInterest.Compcode;
                                        empInterest.Empno = values[1];
                                        empInterest.EmpName = values[2];
                                        if (!string.IsNullOrEmpty(values[3]))
                                        empInterest.OWNINTT = Convert.ToDecimal(values[3]);
                                        if (!string.IsNullOrEmpty(values[4]))
                                        empInterest.VOLINTT = Convert.ToDecimal(values[4]);
                                        if (!string.IsNullOrEmpty(values[5]))
                                        empInterest.COMPINTT = Convert.ToDecimal(values[5]);
                                        empInterest.Uptodate = values[6];
                                        empInterest.YearCode = values[7];
                                        strYearInterest = empInterest.YearCode;
                                        if (iCountInterest == 1)
                                            empInterest.TDeleteRecords = true;

                                        if (empInterest.InsertEmpInterest())
                                            labelStatus.Text = string.Format("Info : File Name : {3} ,Company : {1} - Year : {2} , {0} row(s) inserted.", iCountInterest, strCompanyInterest, strYearInterest, fileNaME);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }

                                    }
                                    iCountInterest++;
                                }

                                lblResult.Text += string.Format("File Name : {3},Company : {0}, Year : {1}, Count : {2}\n", strCompanyInterest, strYearInterest, iCountInterest - 1, fileNaME);
                                #endregion
                                break;
                            case "SA_OPENING_BALANCE":
                                #region sa_opening_balance
                                int iCountOpeningBalance = 0;
                                string strCompanyopBalance = "", strYearOpBalance = "";
                                string fileNaMEOpBalance = "SA_Opening_Balance";
                                SA_Opening_Balance sa_opening_balance;
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCountOpeningBalance != 0)
                                    {
                                        var values = line.Split(',');
                                        sa_opening_balance = new SA_Opening_Balance();
                                        if (!string.IsNullOrEmpty(values[0]))
                                        sa_opening_balance.SrNo = Convert.ToDecimal(values[0]);

                                        sa_opening_balance.Compcode = values[1];
                                        sa_opening_balance.Empno = values[2];
                                        strCompanyopBalance = sa_opening_balance.Compcode;
                                        sa_opening_balance.Yearcode = Convert.ToString(values[3]);
                                        sa_opening_balance.Empname = values[4];
                                        if (!string.IsNullOrEmpty(values[5]))
                                        sa_opening_balance.CEO_Bal = Convert.ToDecimal(values[5]);
                                        if (!string.IsNullOrEmpty(values[6]))
                                        sa_opening_balance.XEO_Bal = Convert.ToDecimal(values[6]);

                                        strYearOpBalance = sa_opening_balance.Yearcode;
                                        if (iCountOpeningBalance == 1)
                                            sa_opening_balance._tDeleteRecords = true;

                                        if (sa_opening_balance.InsertSAOpeningBalance())
                                            labelStatus.Text = string.Format("Info : File Name : {3} ,Company : {1} - Year : {2} , {0} row(s) inserted.", iCountOpeningBalance, strCompanyopBalance, strYearOpBalance, fileNaMEOpBalance);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }

                                    }
                                    iCountOpeningBalance++;
                                }

                                lblResult.Text += string.Format("File Name : {3},Company : {0}, Year : {1}, Count : {2}\n", strCompanyopBalance, strYearOpBalance, iCountOpeningBalance - 1, fileNaMEOpBalance);
                                #endregion
                                break;
                            case "SA_TRAN_IN":
                                #region sa_tran_in
                                int iCountSA_Tran_In = 0;
                                string strCompanySA_Tran_In = "", strYearSA_Tran_In = "";
                                string fileNaMESA_Tran_In = "SA_Tran_In";
                                SA_Tran_In sa_tran_in;
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCountSA_Tran_In != 0)
                                    {
                                        var values = line.Split(',');
                                        sa_tran_in = new SA_Tran_In();
                                        if (!string.IsNullOrEmpty(values[0]))
                                        sa_tran_in.Srno = Convert.ToDecimal(values[0]);

                                        sa_tran_in.Compcode = values[1];
                                        sa_tran_in.Empno = values[2];
                                        sa_tran_in.Compcode = sa_tran_in.Compcode;
                                        sa_tran_in.Yearcode = Convert.ToString(values[3]);
                                        sa_tran_in.Empname = values[4];
                                        if (!string.IsNullOrEmpty(values[5]))
                                        sa_tran_in.Month = Convert.ToDecimal(values[5]);
                                        if (!string.IsNullOrEmpty(values[6]))
                                        sa_tran_in.Year = Convert.ToDecimal(values[6]);
                                        if (!string.IsNullOrEmpty(values[7]))
                                        sa_tran_in.Amount = Convert.ToDecimal(values[7]);

                                        strYearSA_Tran_In = sa_tran_in.Yearcode;
                                        if (iCountSA_Tran_In == 1)
                                            sa_tran_in._tDeleteRecords = true;

                                        if (sa_tran_in.InsertSATranIn())
                                            labelStatus.Text = string.Format("Info : File Name : {3} ,Company : {1} - Year : {2} , {0} row(s) inserted.", iCountSA_Tran_In, strCompanySA_Tran_In, strYearSA_Tran_In, fileNaMESA_Tran_In);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }

                                    }
                                    iCountSA_Tran_In++;
                                }

                                lblResult.Text += string.Format("File Name : {3},Company : {0}, Year : {1}, Count : {2}\n", strCompanySA_Tran_In, strYearSA_Tran_In, iCountSA_Tran_In - 1, fileNaMESA_Tran_In);
                                #endregion
                                break;
                            case "PFLEDGERALL":
                                #region pfledgerall
                                int iCountPfledgerall = 0;
                                string strCompanyPfledgerall = "", strYearPfledgerall = "", strMonth = "";
                                string pfledgerfileNaME = "pfledgerall";
                                while (!reader.EndOfStream)
                                {
                                    Application.DoEvents();
                                    var line = reader.ReadLine();
                                    if (iCountPfledgerall != 0)
                                    {
                                        var values = line.Split(',');
                                        //empInterest = new EmpInterest();
                                        pfledgerall pfledgerallObj = new pfledgerall();
                                        pfledgerallObj.COMPCODE = string.IsNullOrEmpty(values[0]) ? string.Empty : values[0];
                                        strCompanyPfledgerall = pfledgerallObj.COMPCODE;
                                        pfledgerallObj.YEARCODE = string.IsNullOrEmpty(values[1]) ? string.Empty : values[1];
                                        pfledgerallObj.USERNAME = string.IsNullOrEmpty(values[2]) ? string.Empty : values[2];
                                        pfledgerallObj.EMPNO = string.IsNullOrEmpty(values[3]) ? string.Empty : values[3];
                                        pfledgerallObj.MONTHNAME = string.IsNullOrEmpty(values[4]) ? string.Empty : values[4];
                                        if (!string.IsNullOrEmpty(values[5]))
                                        {
                                            pfledgerallObj.UPTODATE = Convert.ToDateTime(values[5]);
                                        }
                                        pfledgerallObj.Serial = string.IsNullOrEmpty(values[6]) ? 0 : Convert.ToDouble(values[6]);
                                        pfledgerallObj.OwnCont = string.IsNullOrEmpty(values[7]) ? 0 : Convert.ToDecimal(values[7]);
                                        pfledgerallObj.CoCont = string.IsNullOrEmpty(values[8]) ? 0 : Convert.ToDecimal(values[8]);
                                        pfledgerallObj.VolCont = string.IsNullOrEmpty(values[9]) ? 0 : Convert.ToDecimal(values[9]);
                                        pfledgerallObj.OwnInt = string.IsNullOrEmpty(values[10]) ? 0 : Convert.ToDecimal(values[10]);
                                        pfledgerallObj.CoInt = string.IsNullOrEmpty(values[11]) ? 0 : Convert.ToDecimal(values[11]);
                                        pfledgerallObj.VolInt = string.IsNullOrEmpty(values[12]) ? 0 : Convert.ToDecimal(values[12]);
                                        pfledgerallObj.Total = string.IsNullOrEmpty(values[13]) ? 0 : Convert.ToDecimal(values[13]);
                                        pfledgerallObj.Col8 = string.IsNullOrEmpty(values[14]) ? 0 : Convert.ToDecimal(values[14]);
                                        pfledgerallObj.Col9 = string.IsNullOrEmpty(values[15]) ? 0 : Convert.ToDecimal(values[15]);
                                        pfledgerallObj.Col10 = string.IsNullOrEmpty(values[16]) ? 0 : Convert.ToDecimal(values[16]);
                                        pfledgerallObj.Ibbowtot = string.IsNullOrEmpty(values[17]) ? 0 : Convert.ToDecimal(values[17]);
                                        pfledgerallObj.Ibbcotot = string.IsNullOrEmpty(values[18]) ? 0 : Convert.ToDecimal(values[18]);
                                        pfledgerallObj.Ibbvoltot = string.IsNullOrEmpty(values[19]) ? 0 : Convert.ToDecimal(values[19]);
                                        pfledgerallObj.Tag = string.IsNullOrEmpty(values[20]) ? string.Empty : values[20];
                                        pfledgerallObj.NoOfMonth = string.IsNullOrEmpty(values[21]) ? 0 : Convert.ToDecimal(values[21]);
                                        pfledgerallObj.MonthA = string.IsNullOrEmpty(values[22]) ? 0 : Convert.ToDecimal(values[22]);
                                        pfledgerallObj.EmpSerial = string.IsNullOrEmpty(values[23]) ? 0 : Convert.ToDouble(values[23]);
                                        pfledgerallObj.monthdesc = string.IsNullOrEmpty(values[24]) ? string.Empty : values[24];
                                        pfledgerallObj.EmpName = string.IsNullOrEmpty(values[25]) ? string.Empty : values[25];
                                        pfledgerallObj.TranCode = string.IsNullOrEmpty(values[26]) ? string.Empty : values[26];
                                        pfledgerallObj.PfCode = string.IsNullOrEmpty(values[27]) ? string.Empty : values[27];
                                        pfledgerallObj.CFPF = string.IsNullOrEmpty(values[28]) ? 0 : Convert.ToDecimal(values[28]);
                                        pfledgerallObj.VCHNO = string.IsNullOrEmpty(values[29]) ? string.Empty : values[29];
                                        pfledgerallObj.Autoserial = string.IsNullOrEmpty(values[30]) ? 0 : Convert.ToDouble(values[30]);

                                        strMonth = pfledgerallObj.MONTHNAME;
                                        strYearInterest = pfledgerallObj.YEARCODE;
                                        if (iCountPfledgerall == 1)
                                            pfledgerallObj.TDeleteRecords = true;

                                        if (pfledgerallObj.InsertPFLedgerall())
                                            labelStatus.Text = string.Format("Info : File Name : {3} ,Company : {1} - Year : {2} , - Month : {4},  {0} row(s) inserted.", iCountPfledgerall, strCompanyPfledgerall, strYearPfledgerall, pfledgerfileNaME, strMonth);
                                        else
                                        {
                                            labelStatus.Text = "Error while inserting records, transaction terminated.";
                                            break;
                                        }

                                    }
                                    iCountPfledgerall++;
                                }

                                lblResult.Text += string.Format("File Name : {3},Company : {0}, Year : {1}, Count : {2}\n", strCompanyPfledgerall, strYearPfledgerall, iCountPfledgerall - 1, pfledgerfileNaME);
                                #endregion
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxCSVTable.SelectedIndex == -1)
                labelCSVStatus.Text = "Please select table";
            if (comboBoxCSVCompCode.SelectedIndex == -1)
                labelCSVStatus.Text = "Please select company code";
            //if(checkedListBoxCompanyCodes.SelectedItems.Count == 0)
            //    labelCSVStatus.Text = "Please select company code";
            if (comboBoxCSVYearCode.SelectedIndex == -1)
                labelCSVStatus.Text = "Please select yearcode";
            if (comboBoxCSVMonth.SelectedIndex == -1 && comboBoxCSVTable.SelectedText == "pfledgerall")
                labelCSVStatus.Text = "Please select month for pfledgerall table";


            DataAccess dataAccessCSV = new DataAccess();
            dataAccessCSV.strYearCode = Convert.ToString(comboBoxCSVYearCode.SelectedItem);
            dataAccessCSV.strTableName = Convert.ToString(comboBoxCSVTable.SelectedItem);
            dataAccessCSV.strCompCode = Convert.ToString(comboBoxCSVCompCode.SelectedItem);
            dataAccessCSV.strMonth = Convert.ToString(comboBoxCSVMonth.SelectedItem);


            //if (comboBoxCSVTable.SelectedText == "")
            //{
            //    DataTable dataTableempmast = dataAccessCSV.GetEmpmastResultSet();
            // empmastGenerateCSV(dataTableempmast, dataAccessCSV.strCompCode, dataAccessCSV.strYearCode, dataAccessCSV.strMonth);
            // }

            //else
            //{
            DataTable dataTableResultSet = dataAccessCSV.GetPfledgerallResultSet();
            GenerateCSV(dataTableResultSet, dataAccessCSV.strCompCode, dataAccessCSV.strYearCode, dataAccessCSV.strMonth, Convert.ToString(comboBoxCSVTable.SelectedItem));
            // }

        }



        //void empmastGenerateCSV(DataTable dataTableempmast, string strCompCode, string strYearCode, string strMonth)
        //{
        //    string strFilePath = string.Format("D://empmastcsv//{0}_{1}_{2}//empmast.csv", strCompCode, strYearCode, strMonth);
        //    StringBuilder sb = new StringBuilder();
        //    IEnumerable<string> columnNames = dataTableempmast.Columns.Cast<DataColumn>().
        //                                        Select(column => column.ColumnName);
        //    sb.AppendLine(string.Join("^", columnNames.ToArray()));
        //    foreach (DataRow row in dataTableempmast.Rows)
        //    {
        //        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
        //        sb.AppendLine(string.Join("^", fields.ToArray()));
        //    }
        //    if (!Directory.Exists("D://empmastcsv"))
        //        Directory.CreateDirectory("D://empmastcsv");
        //    if (!Directory.Exists("D://empmastcsv//" + strCompCode + "_" + strYearCode + "_" + strMonth))
        //        Directory.CreateDirectory("D://empmastcsv//" + strCompCode + "_" + strYearCode + "_" + strMonth);

        //    File.WriteAllText(strFilePath, sb.ToString());
        //    labelCSVStatus.Text = string.Format("File Generated @ {0}", strFilePath);

        //}



        void GenerateCSV(DataTable dataTableResultSet, string strCompCode, string strYearCode, string strMonth, string tableName)
        {
            string strFilePath = string.Format("D://pfledgerallCSV//{0}_{1}_{2}//{3}.csv", strCompCode, strYearCode, strMonth, tableName);
            StringBuilder sb = new StringBuilder();
            IEnumerable<string> columnNames = dataTableResultSet.Columns.Cast<DataColumn>().
                                                Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames.ToArray()));
            foreach (DataRow row in dataTableResultSet.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields.ToArray()));
            }
            if (!Directory.Exists("D://pfledgerallCSV"))
                Directory.CreateDirectory("D://pfledgerallCSV");
            if (!Directory.Exists("D://pfledgerallCSV//" + strCompCode + "_" + strYearCode + "_" + strMonth))
                Directory.CreateDirectory("D://pfledgerallCSV//" + strCompCode + "_" + strYearCode + "_" + strMonth);

            File.WriteAllText(strFilePath, sb.ToString());
            labelCSVStatus.Text = string.Format("File Generated @ {0}", strFilePath);

        }

        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
