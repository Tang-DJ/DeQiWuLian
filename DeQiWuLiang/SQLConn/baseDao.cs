using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

namespace DeQiWuLiang.SQLConn
{
    public class BaseDao
    {



        // public static string ConnectionString = "Data Source=120.25.223.75;Initial Catalog=tansportation;Persist Security Info=True;User ID=sa;Password=snwo20";


        //   public static string ConnectionString = "Data Source=120.25.223.75;Initial Catalog=reduTest;Persist Security Info=True;User ID=sa;Password=snwo20";
        //   public static string ConnectionString = "Data Source=120.25.223.75;Initial Catalog=reduV1;Persist Security Info=True;User ID=sa;Password=snwo20";


        public static string ConnectionString = "Data Source=120.25.223.75,5077;Initial Catalog=WuYuHotel;Persist Security Info=True;User ID=sa;Password=snwo20";





        // public static string ConnectionString = "Data Source=cqutpt.ticp.net,36932;Initial Catalog=tansportation;Persist Security Info=True;User ID=sa;Password=snwo20";

        //public static string ConnectionString = "Data Source=localhost;Initial Catalog=transportation;Integrated Security=True";

        //public static string ConnectionString = "Data Source=DESKTOP-KI5HH45\SQLEXPRESS;Integrated Security=True";


        public static SqlConnection getConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            return conn;
        }
        /***************直接执行一条sql，返回影响的行数***************/
        /// <summary>
        /// 运行sql
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回影响的条数</returns>
        public static int execute(string sql)
        {
            SqlCommand command = new SqlCommand(sql);//SDFASDF
            SqlConnection sqlConn = getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            //SqlDataReader rd = command.ExecuteReader();
            int count = command.ExecuteNonQuery();
            int i = 0;
            //while (rd.Read())
            //{
            //    i++;
            //}
            //rd.Close();
            sqlConn.Close();
            return count;
        }

        /// <summary>
        /// 运行sql并返回受影响个数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回影响的条数</returns>
        public static int sqlCount(string sql)
        {
            SqlCommand command = new SqlCommand(sql);//SDFASDF
            SqlConnection sqlConn = getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            SqlDataReader rd = command.ExecuteReader();
            //int count = command.ExecuteNonQuery();
            int i = 0;
            while (rd.Read())
            {
                i++;
            }
            rd.Close();
            sqlConn.Close();
            return i;
        }



        /***************直接执行一条sql，返回影响的OleDbDataReader***************/
        /// <summary>
        /// 运行sql
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回SqlDataReader对象</returns>
        /// 
        public static SqlDataReader executeReader(string sql)
        {
            SqlCommand command = new SqlCommand(sql);
            SqlConnection sqlConn = getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            return command.ExecuteReader();
        }
        //@author:yuantingfei
        /// <summary>
        /// 根据条件查找数据
        /// </summary>
        /// <param name="table">数据库表名</param>
        /// <param name="properties">字段（字符串数组）</param>
        /// <param name="condition">字符串，条件</param>
        /// <returns>字典对象，可以由json方法转化为json</returns>
        public List<Dictionary<string, object>> findMapByProperties(String table, String[] properties, String condition)
        {
            string pros = "";
            int prosLength = properties.Length;
            for (int i = 0; i < prosLength; i++)
            {
                pros += properties[i] + ", ";
            }
            pros = pros.Substring(0, pros.Length - 2);
            string sql = "SELECT " + pros + " FROM [" + table + "]";
            if (!(condition == null) && (!("".Equals(condition))))
            {
                sql += " WHERE 1=1 AND " + condition;
            }
            System.Diagnostics.Debug.WriteLine("[sql]: " + sql);

            SqlCommand command = new SqlCommand(sql);
            SqlConnection sqlConn = getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            SqlDataReader read = command.ExecuteReader();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> map = null;
            while (read.Read())
            {
                map = new Dictionary<string, object>();
                for (int i = 0; i < prosLength; i++)
                {
                    map.Add(properties[i], read[properties[i]]);
                }
                list.Add(map);
                map = null;

            }
            read.Close();

            sqlConn.Close();
            return list;
        }
        //@author:yuantingfei
        /// <summary>
        /// 根据条件找到数据
        /// </summary>
        /// <param name="table">数据库表名</param>
        /// <param name="properties">字段（字符串数组）</param>
        /// <param name="condition">字符串，条件</param>
        /// <param name="sortField">排序字段，没有可以传""</param>
        /// <param name="order">正序还是反序  asc  desc</param>  
        /// <param name="needLink">是否使用连表外键   ture  false</param>
        /// <param name="curPage">当前页面(分页用)，取出全部传-1</param>
        /// <param name="limit">每页大小(分页用)，取出全部传-1</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> findMapByPropertiesWithLimit(string table, string[] properties, string condition, string sortField, string order, bool needLink, int curPage, int limit)
        {

            string sql = "";
            string pros = "";
            int prosLength = properties.Length;

            if (condition == null || "".Equals(condition))
            {
                condition = "1=1";
            }
            for (int i = 0; i < prosLength; i++)
            {
                pros += properties[i] + ", ";
            }
            pros = pros.Substring(0, pros.Length - 2);
            if (curPage == -1 || limit == -1)
            {
                sql = "SELECT " + pros + " FROM [" + table + "]";
                if (!(condition == null) && (!("".Equals(condition.Trim()))))
                {
                    sql += " WHERE 1=1 and " + condition;
                }
                if (!(sortField == null) && (!("".Equals(sortField.Trim()))))
                {
                    sql += " order by " + sortField + " ";
                }
                if (!(order == null) && (!("".Equals(order.Trim()))))
                {
                    order = order.ToUpper();
                    sql += order.ToUpper();
                }
            }
            else
            {
                if ("".Equals(order) || order == null)
                {
                    order = "asc";
                }
                order = order.ToUpper();
                if (curPage == 1 || curPage == 0)
                {
                    if (!(condition == null) && (!("".Equals(condition.Trim()))))
                    {
                        sql = "SELECT TOP " + limit + " " + pros + " FROM [" + table + "] WHERE 1=1 AND " + condition + " ORDER BY " + sortField + " " + order;
                    }
                    else
                    {
                        sql = "SELECT TOP " + limit + " " + pros + " FROM [" + table + "] ORDER BY " + sortField + " " + order;
                    }

                }
                else
                {
                    if (order.Trim().ToLower().Contains("asc"))
                    {
                        if (!(condition == null) && (!("".Equals(condition.Trim()))))
                        {
                            sql = "SELECT TOP " + limit + " " + pros + " FROM [" + table +
                            "] WHERE " + condition + " and " + sortField + " > (SELECT MAX(" + sortField + ") FROM (SELECT TOP " + (curPage - 1) * limit +
                            " " + sortField + " FROM [" + table + "] WHERE " + condition + " ORDER BY " + sortField + " " + order + " ) w ) ORDER BY " + sortField + " " + order;
                        }
                        else
                        {
                            sql = "SELECT TOP " + limit + " " + pros + " FROM [" + table +
                                "] WHERE " + sortField + " > (SELECT MAX(" + sortField + ") FROM (SELECT TOP " + (curPage - 1) * limit +
                                " " + sortField + " FROM [" + table + "] WHERE " + condition + " ORDER BY " + sortField + " " + order + ") w) ORDER BY " + sortField + " " + order;
                        }
                    }
                    else
                    {
                        if (!(condition == null) && (!("".Equals(condition.Trim()))))
                        {
                            sql = "SELECT TOP " + limit + " " + pros + " FROM [" + table +
                                "] WHERE " + condition + " and " + sortField + " < (SELECT MIN(" + sortField + ") FROM (SELECT TOP " + (curPage - 1) * limit +
                                " " + sortField + " FROM [" + table + "] WHERE " + condition + " ORDER BY " + sortField + " " + order + ") w) ORDER BY " + sortField + " " + order;

                        }
                        else
                        {
                            sql = "SELECT TOP " + limit + " " + pros + " FROM [" + table +
                                "] WHERE " + sortField + " < (SELECT MIN(" + sortField + ") FROM (SELECT TOP " + (curPage - 1) * limit +
                                " " + sortField + " FROM [" + table + "]  WHERE " + condition + " ORDER BY " + sortField + " " + order + ") w ) ORDER BY " + sortField + " " + order;
                        }
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("[sql]: " + sql);
            SqlCommand command = new SqlCommand(sql);
            SqlConnection sqlConn = getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            SqlDataReader read = command.ExecuteReader();

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> map = null;
            while (read.Read())
            {
                map = new Dictionary<string, object>();
                for (int i = 0; i < prosLength; i++)
                {
                    if (properties[i].Contains(" as "))
                    {
                        string[] arr = properties[i].Split(new string[] { " as " }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            if (arr[1] != null)
                            {
                                //properties[i] = arr[1];
                                map.Add(arr[1], read[arr[1]]);
                            }
                        }
                        catch (Exception)
                        {
                            map.Add(properties[i], read[properties[i]]);
                            Dictionary<string, object> datalog = new Dictionary<string, object>();
                            datalog.Add("type", int.Parse("1"));
                            datalog.Add("OpTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            datalog.Add("OpID", "0");
                            datalog.Add("OpLog", "异常：在 findMapByPropertiesWithLimit里面 sql=" + sql);
                            save("T_LOG", datalog);
                        }

                    }
                    else
                    {
                        map.Add(properties[i], read[properties[i]]);
                    }

                }
                list.Add(map);
                map = null;

            }
            read.Close();
            sqlConn.Close();
            return list;
        }
        //@author:yuantingfei
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="msg">键值对</param>
        /// <returns>影响的行数</returns>
        public int save(string table, Dictionary<string, object> msg)
        {
            string[] properties = (string[])msg.Keys.ToArray();
            string sql = "INSERT INTO [" + table + "] ";
            string setPro = "(";
            foreach (string key in msg.Keys)
            {
                if (key != "mapID")
                    setPro += "[" + key + "], ";
            }

            setPro = setPro.Substring(0, setPro.Length - 2);
            setPro += ") VALUES(";

            foreach (object value in msg.Values)
            {
                if (value == null)
                {
                    setPro += "null, ";
                }
                else if (Object.ReferenceEquals(value.GetType(), new int().GetType()) || Object.ReferenceEquals(value.GetType(), new bool().GetType()))
                {
                    setPro += value + ", ";
                }
                else
                {
                    setPro += ("'" + value + "', ");
                }

            }
            setPro = setPro.Substring(0, setPro.Length - 2);
            setPro += ")";
            sql += setPro;
            System.Diagnostics.Debug.Write("[sql]:" + sql);

            SqlCommand command = new SqlCommand(sql);
            SqlConnection sqlConn = getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            int count = command.ExecuteNonQuery();
            sqlConn.Close();
            return count;
        }

        //@author:yuantingfei
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="msg">键值对</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int update(String table, Dictionary<string, object> msg, string condition)
        {

            string[] properties = (string[])msg.Keys.ToArray();
            string sql = "UPDATE [" + table + "] SET ";
            string setPro = "";
            foreach (KeyValuePair<string, object> kvp in msg)
            {
                if (kvp.Value != null && kvp.Key != "mapID")
                {
                    setPro += kvp.Key + " = ";
                    if (Object.ReferenceEquals(kvp.Value.GetType(), new int().GetType()) || Object.ReferenceEquals(kvp.Value.GetType(), new bool().GetType()))
                    {
                        setPro += kvp.Value + ", ";
                    }
                    else
                    {
                        setPro += ("'" + kvp.Value + "', ");
                    }
                }
            }

            setPro = setPro.Substring(0, setPro.Length - 2);
            setPro += " WHERE 1=1 AND " + condition;
            sql += setPro;

            SqlCommand command = new SqlCommand(sql);
            SqlConnection sqlConn = getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            int count = command.ExecuteNonQuery();
            sqlConn.Close();
            return count;
        }

        public static void writeLog(string action, string id)
        {
            string sqlString = "INSERT INTO t_log(type,oplog,optime,opid) VALUES(1,'" + action.Replace("'", "''") + "',CONVERT(varchar(100), GETDATE(), 20)," + id + ")";
            SqlConnection sqlConn = getConnection();
            SqlCommand command = new SqlCommand(sqlString);
            sqlConn.Open();
            command.Connection = sqlConn;
            command.ExecuteNonQuery();
            sqlConn.Close();
        }
        public static void BindAtDrop(DropDownList obj, DataTable TableName, string valueField, string textField)
        {
            obj.DataSource = TableName;
            obj.DataValueField = valueField;
            obj.DataTextField = textField;
            obj.DataBind();
        }
        public static DataTable NewTable(string TransactSQL, string TableName)
        {
            SqlCommand command = new SqlCommand(TransactSQL);
            SqlConnection sqlConn = SQLConn.BaseDao.getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }

            DataTable tb = new DataTable(TableName);
            int i;
            SqlDataReader reader = command.ExecuteReader();
            for (i = 0; i < reader.FieldCount; i++)
            {
                tb.Columns.Add(reader.GetName(i).Trim());
            }
            reader.Close();
            sqlConn.Close();
            return tb;
        }

        public static void FillTable(string TransactSQL, DataTable TableName)
        {
            SqlCommand command = new SqlCommand(TransactSQL);
            SqlConnection sqlConn = SQLConn.BaseDao.getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }

            DataRow row;
            int i;

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                row = TableName.NewRow();
                for (i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i].ToString();
                }
                TableName.Rows.Add(row);
            }
            TableName.AcceptChanges();
            reader.Close();
            sqlConn.Close();

        }

        public List<Dictionary<string, object>> GetList(String sql, String[] properties)
        {
            int prosLength = properties.Length;
            SqlCommand command = new SqlCommand(sql);
            SqlConnection sqlConn = getConnection();
            command.Connection = sqlConn;
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            SqlDataReader read = command.ExecuteReader();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> map = null;
            while (read.Read())
            {
                map = new Dictionary<string, object>();
                for (int i = 0; i < prosLength; i++)
                {
                    map.Add(properties[i], read[properties[i]]);
                }
                list.Add(map);
                map = null;

            }
            read.Close();
            sqlConn.Close();
            return list;
        }
    }
}