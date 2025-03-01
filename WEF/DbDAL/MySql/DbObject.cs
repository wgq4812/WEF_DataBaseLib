using MySql.Data.MySqlClient;

using System;
using System.Data;
using System.Text;
namespace WEF.DbDAL.MySql
{
    /// <summary>
    /// 数据库信息类。
    /// </summary>
    public class DbObject : IDbObject
    {
        bool isdbosp = false;

        object _locker = new object();

        #region  属性
        public string DbType
        {
            get { return "MySQL"; }
        }
        private string _dbconnectStr;
        public string DbConnectStr
        {
            set { _dbconnectStr = value; }
            get { return _dbconnectStr; }
        }

        MySqlConnection _connect = new MySqlConnection();

        #endregion

        #region 构造函数，构造基本信息
        public DbObject()
        {
            IsDboSp();
        }

        /// <summary>
        /// 构造一个数据库连接
        /// </summary>
        /// <param name="connect"></param>
        public DbObject(string DbConnectStr)
        {
            _dbconnectStr = DbConnectStr;
            _connect.ConnectionString = DbConnectStr;
        }
        /// <summary>
        /// 构造一个连接字符串
        /// </summary>
        /// <param name="SSPI">是否windows集成认证</param>
        /// <param name="Ip">服务器IP</param>
        /// <param name="User">用户名</param>
        /// <param name="Pass">密码</param>
        public DbObject(bool SSPI, string Ip, string User, string Pass)
            : this(SSPI, Ip, User, Pass, "3306")
        {

        }

        /// <summary>
        /// 构造一个连接字符串
        /// </summary>
        /// <param name="SSPI">是否windows集成认证</param>
        /// <param name="Ip">服务器IP</param>
        /// <param name="User">用户名</param>
        /// <param name="Pass">密码</param>
        /// <param name="port">端口</param>
        /// <param name="dataBase">dataBase</param>
        public DbObject(bool SSPI, string Ip, string User, string Pass, string port, string dataBase = "")
        {
            _connect = new MySqlConnection();

            if (!string.IsNullOrEmpty(dataBase) && dataBase != "全部")
            {
                if (SSPI)
                {
                    _dbconnectStr = String.Format("server={0};user id={1}; password={2}; Port={3};database={4}; pooling=true", Ip, User, Pass, port, dataBase);//database=mysql
                }
                else
                {
                    _dbconnectStr = String.Format("server={0};user id={1}; password={2}; Port={3};database={4}; pooling=true", Ip, User, Pass, port, dataBase);//database=mysql
                }
            }
            else
            {
                if (SSPI)
                {
                    _dbconnectStr = String.Format("server={0};user id={1}; password={2}; Port={3};database=; pooling=true", Ip, User, Pass, port);//database=mysql
                }
                else
                {
                    _dbconnectStr = String.Format("server={0};user id={1}; password={2}; Port={3};database=; pooling=true", Ip, User, Pass, port);//database=mysql
                }
            }
            _connect.ConnectionString = _dbconnectStr;
        }


        #endregion

        #region  是否采用sp(存储过程)的方式获取数据结构信息
        /// <summary>
        /// 是否采用sp的方式获取数据结构信息
        /// </summary>
        /// <returns></returns>
        private bool IsDboSp()
        {
            return isdbosp;
        }

        #endregion

        #region 打开数据库 OpenDB(string DbName)

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="dbName">要打开的数据库</param>
        /// <returns></returns>
        public MySqlCommand OpenDB(string dbName)
        {
            if (string.IsNullOrEmpty(dbName)) throw new Exception("DbName 不能为空！");

            if (_connect.ConnectionString != _dbconnectStr)
            {
                if (_connect.State != ConnectionState.Closed)
                    _connect.Close();
                _connect.ConnectionString = _dbconnectStr;
            }

            var dbCommand = new MySqlCommand();

            if (_connect.State != ConnectionState.Open)
            {
                try
                {
                    _connect.Open();
                }
                catch (Exception ex)
                {
                    if (ex.Message == "由于意外的数据包格式，握手失败。")
                    {
                        if (_dbconnectStr.EndsWith(";"))
                            _dbconnectStr += "SslMode=None;";
                        else
                            _dbconnectStr += ";SslMode=None;";
                        _connect.ConnectionString = _dbconnectStr;
                        _connect.Open();
                    }
                }
            }

            dbCommand.Connection = _connect;
            dbCommand.CommandTimeout = 1200;
            dbCommand.CommandText = "use " + dbName + "";
            dbCommand.ExecuteNonQuery();
            return dbCommand;
        }


        /// <summary>
        /// 打开数据库
        /// </summary>
        public void OpenDB()
        {
            lock (_locker)
            {
                if (_connect.ConnectionString == "")
                {
                    _connect.ConnectionString = _dbconnectStr;
                }

                if (_connect.ConnectionString != _dbconnectStr)
                {
                    _connect.Close();
                    _connect.ConnectionString = _dbconnectStr;
                }
                if (_connect.State == System.Data.ConnectionState.Closed)
                {
                    _connect.Open();
                }
            }
        }
        #endregion

        #region ADO.NET 操作

        public int ExecuteSql(string dbName, string sqlStr)
        {
            using (MySqlCommand dbCommand = OpenDB(dbName))
            {
                dbCommand.CommandText = sqlStr;
                return dbCommand.ExecuteNonQuery();
            }
        }


        public IDataReader GetDataReader(string dbName, string sqlStr)
        {
            MySqlCommand dbCommand = OpenDB(dbName);
            dbCommand.CommandText = sqlStr;
            return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public DataSet Query(string DbName, string SQLString)
        {
            lock (_locker)
            {
                DataSet ds = new DataSet();
                OpenDB(DbName);
                MySqlDataAdapter adapter = new MySqlDataAdapter(SQLString, _connect);
                adapter.Fill(ds, "ds");
                adapter.Dispose();
                return ds;
            }
        }


        public MySqlDataReader ExecuteReader(string DbName, string strSQL)
        {
            MySqlCommand cmd = OpenDB(DbName);
            cmd.CommandText = strSQL;
            MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return myReader;
        }


        public object GetSingle(string DbName, string SQLString)
        {
            MySqlCommand dbCommand = OpenDB(DbName);
            dbCommand.CommandText = SQLString;
            object obj = dbCommand.ExecuteScalar();
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }
        }



        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string DbName, string storedProcName, IDataParameter[] parameters, string tableName)
        {

            OpenDB(DbName);
            DataSet dataSet = new DataSet();
            MySqlDataAdapter sqlDA = new MySqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(_connect, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);
            return dataSet;

        }


        private MySqlCommand BuildQueryCommand(MySqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            MySqlCommand command = new MySqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (MySqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// List根据字符串排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0代表相等，-1代表y大于x，1代表x大于y</returns>
        private int CompareStrByOrder(string x, string y)
        {
            if (x == "")
            {
                if (y == "")
                {
                    return 0;// If x is null and y is null, they're equal. 
                }
                else
                {
                    return -1;// If x is null and y is not null, y is greater. 
                }
            }
            else
            {
                if (y == "")  // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    int retval = x.CompareTo(y);
                    return retval;
                }
            }
        }

        #endregion


        #region 得到数据库的名字列表 GetDBList()


        /// <summary>
        /// 得到数据库的名字列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDBList()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("name", typeof(string));
            dt.Columns.Add(dc);
            string strSql = "SHOW DATABASES";
            using (MySqlDataReader reader = ExecuteReader("mysql", strSql))
            {
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetString(0);
                    dt.Rows.Add(dr);
                }
                reader.Close();
            }
            return dt.OrderBy("name");
        }
        #endregion

        #region  得到数据库的所有表和视图 的名字



        /// <summary>
        /// 得到数据库的所有表名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTables(string DbName)
        {
            string strSql = "SHOW TABLE STATUS";//order by id
            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            DataRow[] drs = dt.Select("Comment<>'VIEW' and Engine is not null");
            DataTable newdt = dt.Clone();
            foreach (DataRow dr in drs)
            {
                DataRow newdr = newdt.NewRow();
                newdr["name"] = dr["name"];
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
        public DataTable GetTablesSP(string DbName)
        {
            MySqlParameter[] parameters = {
                    new MySqlParameter("@table_name", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_owner", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_qualifier", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_type", MySqlDbType.VarChar,100)
            };
            parameters[0].Value = null;
            parameters[1].Value = null;
            parameters[2].Value = null;
            parameters[3].Value = "'TABLE'";

            DataSet ds = RunProcedure(DbName, "sp_tables", parameters, "ds");
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dt.Columns["TABLE_QUALIFIER"].ColumnName = "db";
                dt.Columns["TABLE_OWNER"].ColumnName = "cuser";
                dt.Columns["TABLE_NAME"].ColumnName = "name";
                dt.Columns["TABLE_TYPE"].ColumnName = "type";
                dt.Columns["REMARKS"].ColumnName = "remarks";
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到数据库的所有视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetVIEWs(string DbName)
        {
            string strSql = "SHOW TABLE STATUS";//order by id
            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            DataRow[] drs = dt.Select("Comment='VIEW' and Engine is  null");
            DataTable newdt = dt.Clone();
            foreach (DataRow dr in drs)
            {
                DataRow newdr = newdt.NewRow();
                newdr["name"] = dr["name"];
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
        /// <summary>
        /// 得到数据库的所有视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetVIEWsSP(string DbName)
        {
            MySqlParameter[] parameters = {
                    new MySqlParameter("@table_name", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_owner", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_qualifier", MySqlDbType.VarChar,384),
                    new MySqlParameter("@table_type", MySqlDbType.VarChar,100)
            };
            parameters[0].Value = null;
            parameters[1].Value = null;
            parameters[2].Value = null;
            parameters[3].Value = "'VIEW'";

            DataSet ds = RunProcedure(DbName, "sp_tables", parameters, "ds");
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dt.Columns["TABLE_QUALIFIER"].ColumnName = "db";
                dt.Columns["TABLE_OWNER"].ColumnName = "cuser";
                dt.Columns["TABLE_NAME"].ColumnName = "name";
                dt.Columns["TABLE_TYPE"].ColumnName = "type";
                dt.Columns["REMARKS"].ColumnName = "remarks";
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到数据库的所有表和视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTabViews(string DbName)
        {
            string strSql = "SHOW TABLES";//order by id
            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            return dt;
        }
        /// <summary>
        /// 得到数据库的所有表和视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTabViewsSP(string DbName)
        {
            return GetTabViews(DbName);
        }

        /// <summary>
        /// 得到数据库的所有存储过程名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetProcs(string DbName)
        {
            return null;
        }
        #endregion

        #region  得到数据库的所有表和视图 的列表信息
        /// <summary>
        /// 得到数据库的所有表的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTablesInfo(string DbName)
        {
            string strSql = "SHOW TABLE STATUS";//order by id
            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            DataRow[] drs = dt.Select("Comment<>'VIEW' and Engine is not null");
            DataTable newdt = dt.Clone();
            foreach (DataRow dr in drs)
            {
                DataRow newdr = newdt.NewRow();
                newdr["name"] = dr["name"];
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
        /// <summary>
        /// 得到数据库的所有视图的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetVIEWsInfo(string DbName)
        {
            string strSql = "SHOW TABLE STATUS";//order by id
            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            DataRow[] drs = dt.Select("Comment='VIEW' and Engine is null");
            DataTable newdt = dt.Clone();
            foreach (DataRow dr in drs)
            {
                DataRow newdr = newdt.NewRow();
                newdr["name"] = dr["name"];
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
        /// <summary>
        /// 得到数据库的所有表和视图的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTabViewsInfo(string DbName)
        {
            string strSql = "SHOW TABLE STATUS";//order by id
            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            return dt;
        }
        /// <summary>
        /// 得到数据库的所有存储过程的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetProcInfo(string DbName)
        {
            return null;
        }
        #endregion

        #region 得到对象定义语句
        /// <summary>
        /// 得到视图或存储过程的定义语句
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public string GetObjectInfo(string DbName, string objName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select b.text ");
            strSql.Append("from sysobjects a, syscomments b  ");
            strSql.Append("where a.xtype='p' and a.id = b.id  ");
            strSql.Append(" and a.name= '" + objName + "'");
            return GetSingle(DbName, strSql.ToString()).ToString();
        }
        #endregion

        #region 得到(快速)数据库里表的列名和类型 GetColumnList(string DbName,string TableName)

        /// <summary>
        /// 得到数据库里表或视图的列名和类型
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public DataTable GetColumnList(string DbName, string TableName)
        {
            return GetColumnInfoList(DbName, TableName);

        }
        public DataTable GetColumnListSP(string DbName, string TableName)
        {
            return GetColumnInfoList(DbName, TableName);
        }
        #endregion


        #region 得到表的列的详细信息 GetColumnInfoList(string DbName,string TableName)

        /// <summary>
        /// 得到数据库里表或视图的列的详细信息
        /// </summary>
        /// <param name="dbName">库</param>
        /// <param name="tableName">表</param>
        /// <returns></returns>
        public DataTable GetColumnInfoList(string dbName, string tableName)
        {
            try
            {
                string sql = $"show full columns from {tableName}";
                DataTable columnsTables = CreateColumnTable();
                DataRow dr;
                var reader = ExecuteReader(dbName, sql);
                int n = 1;
                while (reader.Read())
                {
                    dr = columnsTables.NewRow();
                    dr[0] = n.ToString();
                    if ((!Object.Equals(reader["Field"], null)) && (!Object.Equals(reader["Field"], System.DBNull.Value)))
                    {
                        string tname = reader["Field"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                dr["ColumnName"] = Encoding.Default.GetString((Byte[])reader["Field"]);
                                break;
                            case "":
                                break;
                            default:
                                dr["ColumnName"] = reader["Field"].ToString();
                                break;
                        }
                    }
                    string typename = string.Empty;
                    if ((!Object.Equals(reader["Type"], null)) && (!Object.Equals(reader["Type"], System.DBNull.Value)))
                    {
                        string tname = reader["Type"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                typename = Encoding.Default.GetString((Byte[])reader["Type"]);
                                break;
                            case "":
                                break;
                            default:
                                typename = reader["Type"].ToString();
                                break;
                        }
                    }
                    string len = "", pre = "", scal = "";

                    TypeNameProcess(typename, out typename, out len, out pre, out scal);

                    dr["TypeName"] = typename;

                    dr["Length"] = len;
                    dr["Preci"] = pre;
                    dr["Scale"] = scal;
                    if ((!Object.Equals(reader["Key"], null)) && (!Object.Equals(reader["Key"], System.DBNull.Value)))
                    {
                        string skey = "";
                        string tname = reader["Key"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                skey = Encoding.Default.GetString((Byte[])reader["Key"]);
                                break;
                            case "":
                                break;
                            default:
                                skey = reader["Key"].ToString();
                                break;
                        }
                        dr["isPK"] = (skey.Trim() == "PRI") ? "√" : "";
                        dr["IsIdentity"] = (skey.Trim() == "PRI") ? "√" : "";
                    }
                    if ((!Object.Equals(reader["Null"], null)) && (!Object.Equals(reader["Null"], System.DBNull.Value)))
                    {
                        string snull = "";
                        string tname = reader["Null"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                snull = Encoding.Default.GetString((Byte[])reader["Null"]);
                                break;
                            case "":
                                break;
                            default:
                                snull = reader["Null"].ToString();
                                break;
                        }
                        dr["cisNull"] = (snull.Trim() == "YES") ? "√" : "";
                    }
                    if ((!Object.Equals(reader["Default"], null)) && (!Object.Equals(reader["Default"], System.DBNull.Value)))
                    {
                        string tname = reader["Default"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                dr["DefaultVal"] = Encoding.Default.GetString((Byte[])reader["Default"]);
                                break;
                            case "":
                                break;
                            default:
                                dr["DefaultVal"] = reader["Default"].ToString();
                                break;
                        }
                    }
                    dr["IsIdentity"] = "";
                    if ((!Object.Equals(reader["Comment"], null)) && (!Object.Equals(reader["Comment"], System.DBNull.Value)))
                    {

                        string tname = reader["Comment"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                dr["DeText"] = Encoding.UTF8.GetString((Byte[])reader["Comment"]);
                                break;
                            case "":
                                dr["DeText"] = "";
                                break;
                            default:
                                dr["DeText"] = reader["Comment"].ToString();
                                break;
                        }
                    }

                    columnsTables.Rows.Add(dr);
                    n++;
                }
                reader.Close();
                return columnsTables;
            }
            catch (System.Exception ex)
            {
                throw new Exception("获取列数据失败" + ex.Message);
            }

        }

        /// <summary>
        /// 得到数据库里表或视图的列的详细信息
        /// </summary>
        /// <param name="dbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public DataTable GetColumnInfoList2(string dbName, string TableName)
        {
            try
            {
                string sql = $"SELECT column_name AS '列名', data_type AS '数据类型',column_key as 'Key', character_maximum_length  AS '字符长度', numeric_precision AS '数字长度', numeric_scale AS '小数位数', is_nullable AS '是否允许非空', CASE WHEN extra = 'auto_increment' THEN 1 ELSE 0 END AS '是否自增', column_default  AS  '默认值', column_comment  AS  '备注' FROM Information_schema.columns WHERE table_Name='{TableName}' order by ORDINAL_POSITION;";
                DataTable columnsTables = CreateColumnTable();
                DataRow dr;
                var reader = ExecuteReader(dbName, sql);
                int n = 1;
                while (reader.Read())
                {
                    dr = columnsTables.NewRow();
                    dr[0] = n.ToString();
                    if ((!Object.Equals(reader["列名"], null)) && (!Object.Equals(reader["列名"], System.DBNull.Value)))
                    {
                        string tname = reader["列名"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                dr["ColumnName"] = Encoding.Default.GetString((Byte[])reader["列名"]);
                                break;
                            case "":
                                break;
                            default:
                                dr["ColumnName"] = reader["列名"].ToString();
                                break;
                        }
                    }
                    string typename = string.Empty;
                    if ((!Object.Equals(reader["数据类型"], null)) && (!Object.Equals(reader["数据类型"], System.DBNull.Value)))
                    {
                        string tname = reader["数据类型"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                typename = Encoding.Default.GetString((Byte[])reader["数据类型"]);
                                break;
                            case "":
                                break;
                            default:
                                typename = reader["数据类型"].ToString();
                                break;
                        }
                    }
                    string len = "", pre = "", scal = "";
                    TypeNameProcess(typename, out typename, out len, out pre, out scal);

                    len = reader["字符长度"].ToString();
                    pre = reader["数字长度"].ToString();
                    scal = reader["小数位数"].ToString();

                    dr["TypeName"] = typename;

                    dr["Length"] = len;
                    dr["Preci"] = pre;
                    dr["Scale"] = scal;
                    if ((!Object.Equals(reader["Key"], null)) && (!Object.Equals(reader["Key"], System.DBNull.Value)))
                    {
                        string skey = "";
                        string tname = reader["Key"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                skey = Encoding.Default.GetString((Byte[])reader["Key"]);
                                break;
                            case "":
                                break;
                            default:
                                skey = reader["Key"].ToString();
                                break;
                        }
                        dr["isPK"] = (skey.Trim() == "PRI") ? "√" : "";
                        dr["IsIdentity"] = (skey.Trim() == "PRI") ? "√" : "";
                    }
                    if ((!Object.Equals(reader["是否允许非空"], null)) && (!Object.Equals(reader["是否允许非空"], System.DBNull.Value)))
                    {
                        string snull = "";
                        string tname = reader["是否允许非空"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                snull = Encoding.Default.GetString((Byte[])reader["是否允许非空"]);
                                break;
                            case "":
                                break;
                            default:
                                snull = reader["是否允许非空"].ToString();
                                break;
                        }
                        dr["cisNull"] = (snull.Trim() == "YES") ? "√" : "";
                    }
                    if ((!Object.Equals(reader["默认值"], null)) && (!Object.Equals(reader["默认值"], System.DBNull.Value)))
                    {
                        string tname = reader["默认值"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                dr["DefaultVal"] = Encoding.Default.GetString((Byte[])reader["默认值"]);
                                break;
                            case "":
                                break;
                            default:
                                dr["DefaultVal"] = reader["默认值"].ToString();
                                break;
                        }
                    }
                    dr["IsIdentity"] = "";
                    if ((!Object.Equals(reader["备注"], null)) && (!Object.Equals(reader["备注"], System.DBNull.Value)))
                    {

                        string tname = reader["备注"].GetType().Name;
                        switch (tname)
                        {
                            case "Byte[]":
                                dr["DeText"] = Encoding.UTF8.GetString((Byte[])reader["备注"]);
                                break;
                            case "":
                                dr["DeText"] = "";
                                break;
                            default:
                                dr["DeText"] = reader["备注"].ToString();
                                break;
                        }
                    }

                    columnsTables.Rows.Add(dr);
                    n++;
                }
                reader.Close();
                return columnsTables;
            }
            catch (System.Exception ex)
            {
                throw new Exception("获取列数据失败" + ex.Message);
            }

        }



        //对类型名称 解析
        private void TypeNameProcess(string strName, out string TypeName, out string Length, out string Preci, out string Scale)
        {
            TypeName = strName;
            int n = strName.IndexOf("(");
            Length = "";
            Preci = "";
            Scale = "";
            if (n > 0)
            {
                TypeName = strName.Substring(0, n);
                switch (TypeName.Trim().ToUpper())
                {
                    //只有大小(M)
                    case "TINYINT":
                    case "SMALLINT":
                    case "MEDIUMINT":
                    case "INT":
                    case "INTEGER":
                    case "BIGINT":
                    case "TIMESTAMP":
                    case "CHAR":
                    case "VARCHAR":
                        {
                            int m = strName.IndexOf(")");
                            Length = strName.Substring(n + 1, m - n - 1);
                        }
                        break;
                    case "FLOAT"://(M,D)
                    case "DOUBLE":
                    case "REAL":
                    case "DECIMAL":
                    case "DEC":
                    case "NUMERIC":
                        {
                            int m = strName.IndexOf(")");
                            string strlen = strName.Substring(n + 1, m - n - 1);
                            int i = strlen.IndexOf(",");
                            Length = strlen.Substring(0, i);
                            Scale = strlen.Substring(i + 1);
                        }
                        break;
                    case "ENUM"://(M1,M2,M3)
                    case "SET":
                        {
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        public DataTable GetColumnInfoListSP(string DbName, string TableName)
        {
            return null;
        }

        #endregion


        #region 得到数据库里表的主键 GetKeyName(string DbName,string TableName)

        //创建列信息表
        public DataTable CreateColumnTable()
        {
            DataTable table = new DataTable();
            DataColumn col;

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "colorder";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "ColumnName";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "deText";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "TypeName";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Length";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Preci";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Scale";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "IsIdentity";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "isPK";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "cisNull";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "defaultVal";
            table.Columns.Add(col);



            return table;
        }

        /// <summary>
        /// 得到数据库里表的主键
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public DataTable GetKeyName(string DbName, string TableName)
        {
            DataTable dtkey = CreateColumnTable();
            DataTable dt = GetColumnInfoList(DbName, TableName);
            DataRow[] rows = dt.Select(" isPK='√' ");
            foreach (DataRow row in rows)
            {
                DataRow nrow = dtkey.NewRow();
                nrow["colorder"] = row["colorder"];
                nrow["ColumnName"] = row["ColumnName"];
                nrow["TypeName"] = row["TypeName"];
                nrow["Length"] = row["Length"];
                nrow["Preci"] = row["Preci"];
                nrow["Scale"] = row["Scale"];
                nrow["IsIdentity"] = row["IsIdentity"];
                nrow["isPK"] = row["isPK"];
                nrow["cisNull"] = row["cisNull"];
                nrow["defaultVal"] = row["defaultVal"];
                nrow["deText"] = row["deText"];
                dtkey.Rows.Add(nrow);
            }
            return dtkey;


        }
        #endregion

        #region 得到数据表里的数据 GetTabData(string DbName,string TableName)

        /// <summary>
        /// 得到数据表里的数据
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DataTable GetTabData(string DbName, string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from " + TableName + "");
            return Query(DbName, strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 根据SQL查询得到数据表里的数据
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DataTable GetTabDataBySQL(string DbName, string strSQL)
        {
            return Query(DbName, strSQL).Tables[0];
        }

        public DataTable GetTabData(string DbName, string TableName, int TopNum)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("select  * from " + TableName);
            if (TopNum > 0)
            {
                builder.Append(" limit 0," + TopNum.ToString());
            }
            return this.Query(DbName, builder.ToString()).Tables[0];
        }


        #endregion


        #region 数据库属性操作

        /// <summary>
        /// 修改表名称
        /// </summary>
        /// <param name="OldName"></param>
        /// <param name="NewName"></param>
        /// <returns></returns>
        public bool RenameTable(string DbName, string OldName, string NewName)
        {
            try
            {
                MySqlCommand dbCommand = OpenDB(DbName);
                dbCommand.CommandText = "RENAME TABLE " + OldName + " TO " + NewName + "";
                dbCommand.ExecuteNonQuery();
                return true;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return false;
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>	
        public bool DeleteTable(string DbName, string TableName)
        {
            try
            {
                MySqlCommand dbCommand = OpenDB(DbName);
                dbCommand.CommandText = "DROP TABLE " + TableName + "";
                dbCommand.ExecuteNonQuery();
                return true;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return false;
            }
        }
        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public bool DeleteView(string DbName, string TableName)
        {
            try
            {
                MySqlCommand dbCommand = OpenDB(DbName);
                dbCommand.CommandText = "DROP VIEW " + TableName + "";
                dbCommand.ExecuteNonQuery();
                return true;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return false;
            }
        }

        /// <summary>
        /// 得到版本号
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            try
            {
                string strSql = "execute master..sp_msgetversion ";//select @@version
                return Query("master", strSql).Tables[0].Rows[0][0].ToString();
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return "";
            }
        }


        /// <summary>
        /// 得到创建表 的脚本
        /// </summary>
        /// <returns></returns>
        public string GetTableScript(string DbName, string TableName)
        {
            string strScript = "";
            string strSql = "SHOW CREATE TABLE " + TableName;
            MySqlDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {
                strScript = reader.GetString(1);
            }
            reader.Close();
            return strScript;

        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public object GetScale(string DbName, string sqlString)
        {
            try
            {
                MySqlCommand dbCommand = OpenDB(DbName);
                dbCommand.CommandText = sqlString;
                return dbCommand.ExecuteScalar();
            }
            catch
            {
                return null;
            }
        }
        #endregion



    }
}