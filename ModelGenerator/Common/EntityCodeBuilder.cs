﻿/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：1e7ab7e0-8733-46b2-a556-1fbb0ad96298
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.ModelGenerator.Common
 * 类名称：EntityCodeBuilder
 * 文件名：EntityCodeBuilder
 * 创建年份：2015
 * 创建时间：2015-09-23 14:54:06
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using WEF.ModelGenerator.Model;
using System.Xml;
using System.Windows.Forms;
using WEF.Common;
using WEF.Cache;

namespace WEF.ModelGenerator.Common
{
    public class EntityCodeBuilder
    {
        private List<ColumnInfo> _columns = new List<ColumnInfo>();

        private string _tableName;

        private string _dbType;

        private string _nameSpace = "WEF.Models";

        private string _className;

        private bool _isView = false;

        private bool _isSZMDX = false;

        public EntityCodeBuilder(string tableName, string nameSpace, string className, List<ColumnInfo> columns, bool isView)
            : this(tableName, nameSpace, className, columns, isView, false)
        {

        }

        public EntityCodeBuilder(string tableName, string nameSpace, string className, List<ColumnInfo> columns, bool isView, bool isSZMDX, string dbType = null)
        {
            _isSZMDX = isSZMDX;
            _className = UtilsHelper.ReplaceSpace(className);
            _nameSpace = UtilsHelper.ReplaceSpace(nameSpace);
            _tableName = tableName;
            _dbType = dbType;
            if (_isSZMDX)
            {
                _className = UtilsHelper.ToUpperFirstword(_className);
            }
            _isView = isView;



            foreach (ColumnInfo col in columns)
            {
                col.Name = UtilsHelper.ReplaceSpace(col.Name);
                if (_isSZMDX)
                {
                    col.Name = UtilsHelper.ToUpperFirstword(col.Name);
                }

                col.DeText = UtilsHelper.ReplaceSpace(col.DeText);
                _columns.Add(col);
            }

        }

        public List<ColumnInfo> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        public string NameSpace
        {
            get { return _nameSpace; }
            set { _nameSpace = value; }
        }
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }
        public bool IsView
        {
            get { return _isView; }
            set { _isView = value; }
        }

        public string Builder()
        {
            Columns = DBToCSharp.DbtoCSColumns(Columns, DbType);

            StringPlus plus = new StringPlus();
            plus.AppendLine("//------------------------------------------------------------------------------");
            plus.AppendLine("// <auto-generated>");
            plus.AppendLine("//     此代码由" + System.Reflection.Assembly.GetExecutingAssembly().FullName + "生成;时间 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            plus.AppendLine("//     运行时版本:" + Environment.Version.ToString());
            plus.AppendLine("//     不建议手动更改此代码，如有需要请自行扩展partial类");
            plus.AppendLine("// </auto-generated>");
            plus.AppendLine("//------------------------------------------------------------------------------");
            plus.AppendLine();
            plus.AppendLine();
            plus.AppendLine("using System;");
            plus.AppendLine("using System.Runtime.Serialization;");
            plus.AppendLine("using WEF;");
            plus.AppendLine("using WEF.Common;");
            plus.AppendLine("using WEF.Section;");
            plus.AppendLine("using System.Collections.Generic;");
            plus.AppendLine();
            plus.AppendLine("namespace " + NameSpace);
            plus.AppendLine("{");
            plus.AppendLine();
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + ClassName + "");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "[Serializable, DataContract]");
            plus.AppendSpaceLine(1, "public partial class " + ClassName + " : Entity ");
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "private static string m_tableName;");
            plus.AppendSpaceLine(2, "public " + ClassName + "():base(\"" + TableName + "\") {m_tableName=\"" + TableName + "\";}");
            plus.AppendSpaceLine(2, "public " + ClassName + "(string tableName):base(tableName) { m_tableName=tableName;}");
            plus.AppendLine();
            plus.AppendLine(BuilderModel());
            plus.AppendLine(BuilderMethod());
            plus.AppendSpaceLine(1, "}");
            plus.AppendLine(BuilderRepository());
            plus.AppendLine("}");
            plus.AppendLine("");
            return plus.ToString();
        }

        private string BuilderModel()
        {
            StringPlus plus = new StringPlus();
            StringPlus plus2 = new StringPlus();
            StringPlus plus3 = new StringPlus();
            plus.AppendSpaceLine(2, "#region Model");
            foreach (ColumnInfo column in Columns)
            {
                plus2.AppendSpaceLine(2, "private " + column.DataTypeName + " _" + column.Name + ";");
                plus3.AppendSpaceLine(2, "/// <summary>");
                plus3.AppendSpaceLine(2, "/// " + column.Name + " " + column.DeText);
                plus3.AppendSpaceLine(2, "/// </summary>");
                plus3.AppendSpaceLine(2, "[DataMember]");
                plus3.AppendSpaceLine(2, "public " + column.DataTypeName + " " + column.Name);
                plus3.AppendSpaceLine(2, "{");
                plus3.AppendSpaceLine(3, "get{ return _" + column.Name + "; }");
                plus3.AppendSpaceLine(3, "set");
                plus3.AppendSpaceLine(3, "{");
                plus3.AppendSpaceLine(4, "this.OnPropertyValueChange(_." + column.Name + ",_" + column.Name + ",value);");
                plus3.AppendSpaceLine(4, "this._" + column.Name + "=value;");
                plus3.AppendSpaceLine(3, "}");
                plus3.AppendSpaceLine(2, "}");
            }
            plus.Append(plus2.Value);
            plus.Append(plus3.Value);
            plus.AppendSpaceLine(2, "#endregion");

            return plus.ToString();
        }

        private string BuilderMethod()
        {
            StringPlus plus = new StringPlus();

            plus.AppendSpaceLine(2, "#region Method");

            //只读
            if (IsView)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 是否只读");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public override bool IsReadOnly()");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "return true;");
                plus.AppendSpaceLine(2, "}");
            }

            ColumnInfo identityColumn = Columns.Find(delegate(ColumnInfo col) { return col.IsIdentity; });
            if (null != identityColumn)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 获取实体中的标识列");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public override Field GetIdentityField()");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "return _." + identityColumn.Name + ";");
                plus.AppendSpaceLine(2, "}");
            }

            List<ColumnInfo> primarykeyColumns = Columns.FindAll(delegate(ColumnInfo col) { return col.IsPrimaryKey; });
            if (null != primarykeyColumns && primarykeyColumns.Count > 0)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 获取实体中的主键列");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public override Field[] GetPrimaryKeyFields()");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "return new Field[] {");
                StringPlus plus2 = new StringPlus();
                foreach (ColumnInfo col in primarykeyColumns)
                {
                    plus2.AppendSpaceLine(4, "_." + col.Name + ",");
                }
                plus.Append(plus2.ToString().TrimEnd().Substring(0, plus2.ToString().TrimEnd().Length - 1));
                plus.AppendLine("};");
                plus.AppendSpaceLine(2, "}");
            }

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取列信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public override Field[] GetFields()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return new Field[] {");
            StringPlus plus3 = new StringPlus();
            foreach (ColumnInfo col in Columns)
            {
                plus3.AppendSpaceLine(4, "_." + col.Name + ",");
            }
            plus.Append(plus3.ToString().TrimEnd().Substring(0, plus3.ToString().TrimEnd().Length - 1));
            plus.AppendLine("};");
            plus.AppendSpaceLine(2, "}");


            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取值信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public override object[] GetValues()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return new object[] {");
            StringPlus plus4 = new StringPlus();
            foreach (ColumnInfo col in Columns)
            {
                plus4.AppendSpaceLine(4, "this._" + col.Name + ",");
            }
            plus.Append(plus4.ToString().TrimEnd().Substring(0, plus4.ToString().TrimEnd().Length - 1));
            plus.AppendLine("};");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "#endregion");
            plus.AppendLine();

            plus.AppendSpaceLine(2, "#region _Field");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 字段信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public class _");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "/// <summary>");
            plus.AppendSpaceLine(3, "/// " + TableName + " ");
            plus.AppendSpaceLine(3, "/// </summary>");
            plus.AppendSpaceLine(3, "public readonly static Field All = new Field(\"*\",m_tableName);");
            foreach (ColumnInfo col in Columns)
            {
                plus.AppendSpaceLine(3, "/// <summary>");
                plus.AppendSpaceLine(3, "/// " + col.Name + " " + col.DeText);
                plus.AppendSpaceLine(3, "/// </summary>");
                plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName,\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
            }
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "#endregion");
            plus.AppendLine();

            return plus.ToString();


        }

        private string BuilderRepository()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + ClassName + "操作类");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "public partial class " + ClassName + "Repository");
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "DBContext db;");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 当前实体查询上下文");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public ISearch<" + ClassName + "> Search(string tableName=\"\")");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "if (string.IsNullOrEmpty(tableName))");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "tableName=\"" + ClassName + "\";");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(4, "return db.Search<" + ClassName + ">(tableName);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 当前实体查询上下文");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public ISearch<" + ClassName + "> Search("+ ClassName + " entity)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(4, "return db.Search<" + ClassName + ">(entity);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 构造方法");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + "Repository()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "db = new DBContext();");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 构造方法");
            plus.AppendSpaceLine(2, "/// <param name=\"connStrName\">连接字符串中的名称</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + "Repository(string connStrName)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "db = new DBContext(connStrName);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 当前db操作上下文");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public DBContext DBContext");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "get");
            plus.AppendSpaceLine(3, "{");
            plus.AppendSpaceLine(4, "return db;");
            plus.AppendSpaceLine(3, "}");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取实体");
            plus.AppendSpaceLine(2, "/// <param name=\"pageIndex\">分页第几页</param>");
            plus.AppendSpaceLine(2, "/// <param name=\"pageSize\">分页一页取值</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + " Get"+ ClassName + "(int id)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return Search().Where(b => b.ID == id).First();");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取列表");
            plus.AppendSpaceLine(2, "/// <param name=\"pageIndex\">分页第几页</param>");
            plus.AppendSpaceLine(2, "/// <param name=\"pageSize\">分页一页取值</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public List<" + ClassName + "> GetList(int pageIndex, int pageSize)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return this.Search().Page(pageIndex, pageSize).ToList();");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取列表");
            plus.AppendSpaceLine(2, "/// <param name=\"tableName\">表名</param>");
            plus.AppendSpaceLine(2, "/// <param name=\"pageIndex\">分页第几页</param>");
            plus.AppendSpaceLine(2, "/// <param name=\"pageSize\">分页一页取值</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public List<" + ClassName + "> GetList(string tableName, int pageIndex=1, int pageSize=12)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return this.Search(tableName).Page(pageIndex, pageSize).ToList();");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 添加实体");
            plus.AppendSpaceLine(2, "/// <param name=\"obj\">传进的实体</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public int Insert(" + ClassName + " obj)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return db.Insert(obj);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 更新实体");
            plus.AppendSpaceLine(2, "/// <param name=\"obj\">传进的实体</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public int Update(" + ClassName + " obj)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return db.Update(obj);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 删除实体");
            plus.AppendSpaceLine(2, "/// <param name=\"obj\">传进的实体</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public int Delete(" + ClassName + " obj)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return db.Delete(obj);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 删除实体");
            plus.AppendSpaceLine(2, "/// <param name=\"id\">id</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public int Delete(int id)");

            plus.AppendSpaceLine(2, $"public int Delete({})");

            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "var obj = Search().Where(b => b.ID == id).First();");
            plus.AppendSpaceLine(3, "return db.Delete(obj);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 批量删除实体");
            plus.AppendSpaceLine(2, "/// <param name=\"obj\">传进的实体列表</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public int Deletes(List<" + ClassName + "> objs)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return db.Delete<" + ClassName + ">(objs);");
            plus.AppendSpaceLine(2, "}");


            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 执行sql语句");
            plus.AppendSpaceLine(2, "/// <param name=\"sql\"></param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public SqlSection ExecuteSQL(string sql)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return db.FromSql(sql);");
            plus.AppendSpaceLine(2, "}");


            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 执行存储过程");
            plus.AppendSpaceLine(2, "/// <param name=\"sql\"></param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public ProcSection ExcuteProc(string procName)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return db.FromProc(procName);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(1, "}");
            return plus.ToString();
        }

    }

}
