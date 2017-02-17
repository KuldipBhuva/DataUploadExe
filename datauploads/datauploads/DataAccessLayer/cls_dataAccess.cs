using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;


/// <summary>
/// Summary description for cls_dataAccess
/// </summary>
/// 

namespace DataAccessLayer
{
    public class DataAccess
    {
        private SqlCommand cmd;
        private SqlConnection lConnect;
        private connection cn;
        private SqlDataReader reader;
        private SqlDataAdapter adp;


        public string CompCode { get; set; }
        public DataAccess()
        {
            cn = new connection();
            lConnect = cn.OpenConnetion();
            cmd = new SqlCommand("", lConnect);
            cmd.CommandType = CommandType.StoredProcedure;
        }



        public DataTable GetAllYears()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd.CommandText = "GetAllYears";
                reader = cmd.ExecuteReader();
                dtSlab.Load(reader);
                return dtSlab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.CloseConnection();
            }
        }
        public void GetAuditedYear(out string strAuditYear)
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd.CommandText = "GetYearCodes";
                cmd.Parameters.AddWithValue("@compcode", CompCode);
                reader = cmd.ExecuteReader();
                dtSlab.Load(reader);
                strAuditYear = Convert.ToString(dtSlab.Rows[0]["AuditYear"]);
                //return dtSlab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.CloseConnection();
            }
        }
        public void LogException(Exception ex)
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd.CommandText = "LogException";
                cmd.Parameters.AddWithValue("@errorMessage", ex.Message.ToString());
                cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace.ToString());
                reader = cmd.ExecuteReader();
                dtSlab.Load(reader);

                //return dtSlab;
            }
            catch
            {

            }
            finally
            {
                cn.CloseConnection();
            }
        }



        public string GetTableColumns(string tablename, string columns)
        {
            try
            {

                DataTable dtSlab = new DataTable();
                cmd.CommandText = "GetColumnsOfTable";
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@tableName", tablename);
                cmd.Parameters.Add(new SqlParameter("@columns", SqlDbType.VarChar, 8000, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Current, columns));
                cmd.ExecuteNonQuery();
                string columnvalue = Convert.ToString(cmd.Parameters["@columns"].Value);
                //dtSlab.Load(reader);
                return columnvalue;
                //return dtSlab;
            }
            catch
            {
                return "*";
            }
            finally
            {
                cn.CloseConnection();
            }
        }

        public System.Data.DataTable GetSumDataTable(string Table, string COndition)
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd.CommandText = "GetSumDataTable";
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@table", Table);
                cmd.Parameters.AddWithValue("@condition", COndition);
                reader = cmd.ExecuteReader();
                dtSlab.Load(reader);
                return dtSlab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.CloseConnection();
            }
        }

        public string TableName { get; set; }

        public string WhereCondition { get; set; }

        public string YearCode { get; set; }
        public bool CheckAuditYear()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd.CommandText = "CheckAuditYear";
                cmd.Parameters.AddWithValue("@compCode", this.CompCode);//.Replace("'","''"));
                cmd.Parameters.AddWithValue("@yearcode", this.YearCode);//.Replace("'", "''"));
                reader = cmd.ExecuteReader();
                dtSlab.Load(reader);
                //return dtSlab;
                bool result = false;
                if (dtSlab.Rows.Count > 0)
                {
                    if (Convert.ToString(dtSlab.Rows[0]["IsValid"]) == "Audit")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.CloseConnection();
            }
        }
        public bool CheckDataExists()
        {
            try
            {
                DataTable dtSlab = new DataTable();
                cmd.CommandText = "CheckDataExist";
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@table", TableName);//.Replace("'","''"));
                cmd.Parameters.AddWithValue("@condition", WhereCondition);//.Replace("'", "''"));
                reader = cmd.ExecuteReader();
                dtSlab.Load(reader);
                //return dtSlab;
                bool result = false;
                if (dtSlab.Rows.Count > 0)
                {
                    if (Convert.ToString(dtSlab.Rows[0]["Result"]) == "1")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.CloseConnection();
            }
        }

        public string strTableName { get; set; }
        public string strYearCode { get; set; }
        public string strMonth { get; set; }
        public string strCompCode { get; set; }

        public DataTable GetPfledgerallResultSet()
        {
            localConnection cnction = new localConnection();
            try
            {
                lConnect = cnction.OpenConnetion();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dataTablePfledgerall = new DataTable();
                cmd.CommandText = "GetPfledgerallDataSet";
                cmd.Parameters.AddWithValue("@table", strTableName);//.Replace("'","''"));
                cmd.Parameters.AddWithValue("@yearCode", strYearCode);//.Replace("'", "''"));
                cmd.Parameters.AddWithValue("@month", strMonth);//.Replace("'","''"));
                cmd.Parameters.AddWithValue("@compcode", strCompCode);//.Replace("'", "''"));
                reader = cmd.ExecuteReader();
                dataTablePfledgerall.Load(reader);
                //return dtSlab;
                return dataTablePfledgerall;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnction.CloseConnection();
            }
        }


        public DataTable GetEmpmastResultSet()
        {
            localConnection cnction = new localConnection();
            try
            {
                lConnect = cnction.OpenConnetion();
                cmd = new SqlCommand("", lConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dataTablePfledgerall = new DataTable();
                cmd.CommandText = "Getempmast";
                cmd.Parameters.AddWithValue("@table", strTableName);//.Replace("'","''"));
                cmd.Parameters.AddWithValue("@yearCode", strYearCode);//.Replace("'", "''"));
                cmd.Parameters.AddWithValue("@month", strMonth);//.Replace("'","''"));
                cmd.Parameters.AddWithValue("@compcode", strCompCode);//.Replace("'", "''"));
                reader = cmd.ExecuteReader();
                dataTablePfledgerall.Load(reader);
                //return dtSlab;
                return dataTablePfledgerall;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnction.CloseConnection();
            }
        }


    }

}

