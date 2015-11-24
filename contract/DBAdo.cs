using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace contract
{
    class DBAdo
    {
        private static OleDbConnection conn;
        private static OleDbTransaction OTrans;
        private static string constr = "";

        //Provider=Microsoft.Jet.OLEDB.4.0;Data Source="C:\Documents and Settings\Administrator\桌面\工业合同管理系统20110515\contract\contract\bin\Debug\ctemp.mdb";Persist Security Info=True;Jet OLEDB:Database Password=snsoft123
        private static string constrA = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "ctemp.mdb;Persist Security Info=True;Jet OLEDB:Database Password=snsoft123";
        //private static string constr = "";//"Data Source=" + Class_Constant.IP1 + ";Initial Catalog=N7_syzdcs;Provider=SQLOLEDB;Persist Security Info=True;User ID=sa";

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="conStr">连接字符串</param>
        public static void setConStr(string conStr)
        {
            constr = conStr;
        }

        /// <summary>
        /// 执行SQL返回OleDbDataReader
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回OleDbDataReader</returns>
        public static OleDbDataReader DrFillSql(String sql)
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constr);
                conn.Open();
                OTrans = conn.BeginTransaction();
                OleDbCommand cmd = new OleDbCommand(sql, conn, OTrans);
                OleDbDataReader dr = cmd.ExecuteReader();
                OTrans.Commit();
                return dr;
            }
            catch (Exception ex)
            {
                OTrans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行SQL返回DATASET
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tabname">表名</param>
        /// <returns>返回的DATASET</returns>
        public static DataSet DsFillSql(String sql, String tabname)
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constr);
                conn.Open();
                OleDbDataAdapter oledb = new OleDbDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                oledb.Fill(ds, tabname);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行SQL返回DATATABLE
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tabname">表名</param>
        /// <returns>返回的DATATABLE</returns>
        public static DataTable DtFillSql(String sql)
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constr);
                conn.Open();
                OleDbDataAdapter oledb = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                oledb.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行SQL 无返回
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public static void ExecuteNonQuerySql(String sql)
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constr);
                conn.Open();
                OTrans = conn.BeginTransaction();
                OleDbCommand cmd = new OleDbCommand(sql, conn, OTrans);
                cmd.ExecuteNonQuery();
                OTrans.Commit();
            }
            catch (Exception ex)
            {
                OTrans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行SQL返回表第一行第一列
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public static object ExecuteScalarSql(String sql)
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constr);
                conn.Open();
                OTrans = conn.BeginTransaction();
                OleDbCommand cmd = new OleDbCommand(sql, conn, OTrans);
                object ores = cmd.ExecuteScalar();
                OTrans.Commit();
                return ores;
            }
            catch (Exception ex)
            {
                OTrans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行存储过程无返回值,只输入存储过程名即可不能带EXEC
        /// </summary>
        /// <param name="sql">存储过程名</param>
        /// <param name="pars">参数数组</param>
        public static void ExecuteNonQuerySqlProcedure(String sql, OleDbParameter[] pars)
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constr);
                conn.Open();
                OTrans = conn.BeginTransaction();
                OleDbCommand cmd = new OleDbCommand(sql, conn, OTrans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(pars);
                cmd.ExecuteNonQuery();
                OTrans.Commit();
            }
            catch (Exception ex)
            {
                OTrans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行存储过程返回参数集合,只输入存储过程名即可不能带EXEC
        /// </summary>
        /// <param name="sql">存储过程名</param>
        /// <param name="pars">参数数组</param>
        /// <returns>存储过程参数集合</returns>        
        public static OleDbParameterCollection ExecuteScalarProcedure(String sql, OleDbParameter[] pars)
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constr);
                conn.Open();
                OTrans = conn.BeginTransaction();
                OleDbCommand cmd = new OleDbCommand(sql, conn, OTrans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(pars);
                cmd.ExecuteNonQuery();
                OTrans.Commit();
                return cmd.Parameters;
            }
            catch (Exception ex)
            {
                OTrans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void AExecuteNonQuerySql(String sql) //执行SQL 无返回
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constrA);
                conn.Open();
                OTrans = conn.BeginTransaction();
                OleDbCommand cmd = new OleDbCommand(sql, conn, OTrans);
                cmd.ExecuteNonQuery();
                OTrans.Commit();
            }
            catch (Exception e1)
            {
                OTrans.Rollback();
                MessageBox.Show("" + e1, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable ADtFillSql(String sql) //返回DATASET
        {
            try
            {
                Console.WriteLine(sql);
                conn = new OleDbConnection(constrA);
                conn.Open();
                OleDbDataAdapter oledb = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                oledb.Fill(dt);
                return dt;
            }
            catch (Exception e2)
            {
                MessageBox.Show("" + e2, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
