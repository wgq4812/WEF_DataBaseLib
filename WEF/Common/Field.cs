﻿/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using WEF.Expressions;

namespace WEF.Common
{
    /// <summary>
    /// 字段信息   
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    [Serializable]
    public class Field<T> : Field
        where T : Entity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">字段名</param>
        public Field(string fileName) : base(fileName, EntityCache.GetTableName<T>()) { }
    }

    /// <summary>
    /// 字段信息
    /// </summary>
    [Serializable]
    public class Field
    {
        #region 构造函数
        /// <summary>
        /// 空的构造函数
        /// </summary>
        Field()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName"></param>
        public Field(string fieldName) : this(fieldName, null) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        public Field(string fieldName, string tableName) : this(fieldName, tableName, null, null, fieldName) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        /// <param name="description"></param>
        public Field(string fieldName, string tableName, string description) : this(fieldName, tableName, null, null, description) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        /// <param name="parameterDbType"></param>
        /// <param name="parameterSize"></param>
        /// <param name="description"></param>
        public Field(string fieldName, string tableName, DbType? parameterDbType, int? parameterSize, string description) : this(fieldName, tableName, parameterDbType, parameterSize, description, null) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        /// <param name="parameterDbType"></param>
        /// <param name="parameterSize"></param>
        /// <param name="description"></param>
        /// <param name="aliasName"></param>
        public Field(string fieldName, string tableName, DbType? parameterDbType, int? parameterSize, string description, string aliasName)
        {
            this.fieldName = fieldName;
            this.tableName = tableName;
            this.description = description;
            this.aliasName = aliasName;
            this.parameterDbType = parameterDbType ?? DbType.String;
            this.parameterSize = parameterSize ?? 0;
        }
        #endregion

        #region 字段

        /// <summary>
        /// 字段名
        /// </summary>
        private string fieldName;

        /// <summary>
        /// 表名
        /// </summary>
        public string tableName;

        /// <summary>
        /// 字段别名
        /// </summary>
        private string aliasName;

        /// <summary>
        /// 字段描述
        /// </summary>
        private string description;

        /// <summary>
        /// 字段数据库中类型
        /// </summary>
        private DbType parameterDbType;

        /// <summary>
        /// 字段数据库中长度
        /// </summary>
        private int parameterSize;

        /// <summary>
        /// 所有字段   就是  *  
        /// </summary>
        public static readonly Field All = new Field("*");

        /// <summary>
        /// 值 
        /// </summary>
        //public object value;//2016-02-02新增
        #endregion

        #region 属性

        /// <summary>
        /// 字段数据库中类型
        /// </summary>
        public DbType ParameterDbType
        {
            get
            {
                return parameterDbType;
            }
        }

        /// <summary>
        /// 字段数据库中长度
        /// </summary>
        public int ParameterSize
        {
            get
            {
                return parameterSize;
            }
        }

        /// <summary>
        /// 返回 字段名
        /// </summary>
        public string FieldName
        {
            get
            {
                if (fieldName.Trim() == "*" || fieldName.IndexOf('\'') >= 0
                    || fieldName.IndexOf('(') >= 0 || fieldName.IndexOf(')') >= 0
                    || fieldName.Contains("{0}") || fieldName.Contains("{1}")
                    || fieldName.IndexOf(" as ", StringComparison.OrdinalIgnoreCase) >= 0
                    || fieldName.Contains("*")
                    || fieldName.IndexOf("distinct ", StringComparison.OrdinalIgnoreCase) >= 0
                    || fieldName.IndexOf('[') >= 0 || fieldName.IndexOf(']') >= 0
                    || fieldName.IndexOf('"') >= 0 || fieldName.IndexOf('`') >= 0)
                {
                    return fieldName;
                }

                return string.Concat("{0}", fieldName, "{1}");
            }
        }


        /// <summary>
        /// 返回  表名
        /// </summary>
        public string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                    return tableName;
                return string.Concat("{0}", tableName, "{1}");
            }
        }


        /// <summary>
        /// 返回  别名，当别名为空返回字段名
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(aliasName))
                    return fieldName;
                return aliasName;
            }
        }

        /// <summary>
        /// 返回属性名  即fileName
        /// </summary>
        public string PropertyName
        {
            get
            {
                return fieldName;
            }
        }

        /// <summary>
        /// 返回  别名
        /// </summary>
        public string AliasName
        {
            get
            {
                return aliasName;
            }
            set
            {
                aliasName = value;
            }
        }

        /// <summary>
        /// 返回  字段描述
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }

        /// <summary>
        /// 返回  表名.字段名
        /// </summary>
        public string TableFieldName
        {
            get
            {
                if (DBContext.Current != null && DBContext.Current.Db.DbProvider.DatabaseType == DatabaseType.PostgreSQL)
                {
                    return FieldName;
                }
                else
                {
                    if (string.IsNullOrEmpty(tableName))

                        return FieldName;

                    return string.Concat(TableName, ".", FieldName);
                }
            }
        }

        /// <summary>
        /// 返回  表名.字段名 AS 别名
        /// </summary>
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(aliasName))

                    return TableFieldName;

                return string.Concat(TableFieldName, " AS {0}", aliasName, "{1}");
            }
        }

        /// <summary>
        /// datepart   year
        /// </summary>
        public Field Year
        {
            get
            {
                return new Field(string.Concat("datepart(year,", this.TableFieldName, ")")).As(this);
            }
        }

        /// <summary>
        /// datepart   month
        /// </summary>
        public Field Month
        {
            get
            {
                return new Field(string.Concat("datepart(month,", this.TableFieldName, ")")).As(this);
            }
        }

        /// <summary>
        /// datepart  day
        /// </summary>
        public Field Day
        {
            get
            {
                return new Field(string.Concat("datepart(day,", this.TableFieldName, ")")).As(this);
            }
        }

        /// <summary>
        /// 倒叙
        /// </summary>
        public OrderByOperation Desc
        {
            get
            {
                return new OrderByOperation(this, OrderByOperater.DESC);
            }
        }


        /// <summary>
        /// 正序
        /// </summary>
        public OrderByOperation Asc
        {
            get
            {
                return new OrderByOperation(this, OrderByOperater.ASC);
            }
        }


        /// <summary>
        /// 分组
        /// </summary>
        public GroupByOperation GroupBy
        {
            get
            {
                return new GroupByOperation(this);
            }
        }

        #endregion

        #region 方法

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool Equals(Field field)
        {
            if (null == (object)field)
                return false;
            return this.FullName.Equals(field.FullName);
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(Field field)
        {
            if (null == (object)field || string.IsNullOrEmpty(field.PropertyName))
                return true;
            return false;
        }


        /// <summary>
        /// 设置所属表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Field SetTableName(string tableName)
        {
            return new Field(this.fieldName, tableName, this.parameterDbType, this.parameterSize, this.description, this.aliasName);
        }

        /// <summary>
        /// AS
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public Field As(string aliasName)
        {
            return new Field(this.fieldName, this.tableName, this.parameterDbType, this.parameterSize, this.description, aliasName);
        }


        /// <summary>
        /// AS
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public Field As(Field field)
        {
            return As(field.Name);
        }

        /// <summary>
        /// 判断字段是否为Null
        /// </summary>
        /// <param name="field">字段实体</param>
        /// <returns></returns>
        public Field IsNull(Field field)
        {
            return new Field(string.Concat("isnull(", this.TableFieldName, ",", field.TableFieldName, ")")).As(this);
        }

        /// <summary>
        /// 判断字段是否为Null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Field IsNull(object value)
        {
            return new Field(string.Concat("isnull(", this.TableFieldName, ",", DataUtils.FormatValue(value), ")")).As(this);
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <returns></returns>
        public Field Count()
        {
            if (this.PropertyName.Trim().Equals("*"))
                return new Field("count(*)").As("cnt");
            return new Field(string.Concat("count(", this.TableFieldName, ")")).As(this);
        }

        /// <summary>
        /// Distinct
        /// </summary>
        /// <returns></returns>
        public Field Distinct()
        {
            if (this.PropertyName.Trim().Equals("*"))
                return this;
            return new Field(string.Concat("distinct ", this.TableFieldName)).As(this);
        }

        /// <summary>
        /// Sum
        /// </summary>
        /// <returns></returns>
        public Field Sum()
        {
            return new Field(string.Concat("sum(", this.TableFieldName, ")")).As(this);
        }

        /// <summary>
        /// Avg
        /// </summary>
        /// <returns></returns>
        public Field Avg()
        {
            return new Field(string.Concat("avg(", this.TableFieldName, ")")).As(this);
        }

        /// <summary>
        /// len
        /// </summary>
        /// <returns></returns>
        public Field Len()
        {
            return new Field(string.Concat("len(", this.TableFieldName, ")")).As(this);
        }


        /// <summary>
        /// ltrim and  rtrim
        /// </summary>
        /// <returns></returns>
        public Field Trim()
        {
            return new Field(string.Concat("ltrim(rtrim(", this.TableFieldName, "))")).As(this);
        }

        /// <summary>
        /// Max
        /// </summary>
        /// <returns></returns>
        public Field Max()
        {
            return new Field(string.Concat("max(", this.TableFieldName, ")")).As(this);
        }

        /// <summary>
        /// Min
        /// </summary>
        /// <returns></returns>
        public Field Min()
        {
            return new Field(string.Concat("min(", this.TableFieldName, ")")).As(this);
        }

        /// <summary>
        /// Left
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public Field Left(int length)
        {
            return new Field(string.Concat("left(", this.TableFieldName, ",", length.ToString(), ")")).As(this);
        }


        /// <summary>
        /// Right
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public Field Right(int length)
        {
            return new Field(string.Concat("right(", this.TableFieldName, ",", length.ToString(), ")")).As(this);
        }


        /// <summary>
        /// substring
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public Field Substring(int startIndex, int endIndex)
        {
            return new Field(string.Concat("substring(", this.TableFieldName, ",", startIndex.ToString(), ",", endIndex.ToString(), ")")).As(this);
        }


        /// <summary>
        /// charindex
        /// </summary>
        /// <param name="subString"></param>
        /// <returns></returns>
        public Field IndexOf(string subString)
        {
            return new Field(string.Concat("charindex(", this.TableFieldName, ",", DataUtils.FormatValue(subString), ") - 1")).As(this);
        }

        /// <summary>
        /// replace
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public Field Replace(string oldValue, string newValue)
        {
            return new Field(string.Concat("replace(", this.TableFieldName, ",", DataUtils.FormatValue(oldValue), ",", DataUtils.FormatValue(newValue), ")")).As(this);
        }

        /// <summary>
        /// 创建whereclip
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        private static WhereOperation createWhereClip(Field leftField, Field rightField, QueryOperator oper)
        {
            if (IsNullOrEmpty(leftField) || IsNullOrEmpty(rightField))
                return null;

            return new WhereOperation(leftField.TableFieldName + DataUtils.ToString(oper) + rightField.TableFieldName);
        }

        /// <summary>
        /// 创建Field
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        private static Field createField(Field leftField, Field rightField, QueryOperator oper)
        {
            if (IsNullOrEmpty(leftField) && IsNullOrEmpty(rightField))
                return null;

            if (IsNullOrEmpty(leftField))
                return rightField;

            if (IsNullOrEmpty(rightField))
                return leftField;

            return new Field(leftField.TableFieldName + DataUtils.ToString(oper) + rightField.TableFieldName);
        }


        /// <summary>
        /// &
        /// </summary>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public Field BitwiseAND(Field rightField)
        {
            return createField(this, rightField, QueryOperator.BitwiseAND).As(this);
        }

        /// <summary>
        /// |
        /// </summary>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public Field BitwiseOR(Field rightField)
        {
            return createField(this, rightField, QueryOperator.BitwiseOR).As(this);
        }

        /// <summary>
        /// ^
        /// </summary>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public Field BitwiseXOR(Field rightField)
        {
            return createField(this, rightField, QueryOperator.BitwiseXOR).As(this);
        }

        /// <summary>
        /// ~
        /// </summary>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public Field BitwiseNOT(Field rightField)
        {
            return createField(this, rightField, QueryOperator.BitwiseNOT).As(this);
        }


        private static string formatValue(object value)
        {
            if (null == value)
                return string.Empty;

            return value.ToString();

            //return value.ToString().Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }

        /// <summary>
        /// like '%value%' 模糊查询，同Like
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereOperation Contain(object value)
        {
            return new WhereOperation(this, string.Concat(likeString, value, likeString), QueryOperator.Like);
        }
        /// <summary>
        /// like '%value%' 模糊查询，同  Contain
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereOperation Like(object value)
        {
            return Contain(value);
        }
        /// <summary>
        /// BeginWith  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereOperation BeginWith(object value)
        {
            return new WhereOperation(this, string.Concat(value, likeString), QueryOperator.Like);
        }

        /// <summary>
        /// EndWith  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereOperation EndWith(object value)
        {
            return new WhereOperation(this, string.Concat(likeString, value), QueryOperator.Like);
        }

        /// <summary>
        /// LIKE %
        /// </summary>
        private const string likeString = "%";

        /// <summary>
        /// IN
        /// </summary>
        private const string selectInString = " IN ";

        /// <summary>
        /// NOT IN
        /// </summary>
        private const string selectNotInString = " NOT IN ";

        /// <summary>
        /// 子查询
        /// </summary>
        /// <param name="field"></param>
        /// <param name="join"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private WhereOperation selectInOrNotIn<T>(Field field, string join, params T[] values)
        {
            return selectInOrNotIn<T>(field, join, true, values);
        }


        /// <summary>
        /// 子查询
        /// </summary>
        /// <param name="field"></param>
        /// <param name="join"></param>
        /// <param name="isParameter">是否参数化</param>
        /// <param name="values"></param>
        /// <returns></returns>
        private WhereOperation selectInOrNotIn<T>(Field field, string join, bool isParameter, params T[] values)
        {
            if (values.Length == 0)
            {
                if (join == selectNotInString)//2017-02-28 新增
                {
                    return WhereOperation.All;
                }
                return new WhereOperation("1=2");
                //2015-09-22注释
                //return WhereClip.All;
            }
            Check.Require(!Field.IsNullOrEmpty(field),
                "filed could not be null or empty");
            Check.Require((null != values && values.Length > 0),
                "values could not be null or empty");

            var whereString = new StringBuilder(field.TableFieldName);
            whereString.Append(join);
            whereString.Append("(");
            var ps = new List<Parameter>();
            var inWhere = new StringBuilder();
            var i = 0;
            foreach (T value in values)
            {
                i++;
                string paraName = null;

                if (isParameter)
                {
                    paraName = DataUtils.MakeUniqueKey(field);
                    // paraName = field.tableName + field.Name + i;
                    Parameter p = new Parameter(paraName, value, field.ParameterDbType, field.ParameterSize);
                    ps.Add(p);
                }
                else
                {
                    if (value == null)
                        continue;

                    paraName = value.ToString();

                    if (string.IsNullOrEmpty(paraName))
                        continue;

                }

                inWhere.Append(",");
                inWhere.Append(paraName);
            }
            whereString.Append(inWhere.ToString().Substring(1));
            whereString.Append(")");

            return new WhereOperation(whereString.ToString(), ps.ToArray());
        }



        /// <summary>
        /// SelectIn  
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation SelectIn(params object[] values)
        {
            return selectInOrNotIn<object>(this, selectInString, values);
        }
        /// <summary>
        /// 同SelectIn。
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation In(params object[] values)
        {
            return SelectIn(values);
        }



        /// <summary>
        /// where field in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public WhereOperation SelectIn<T>(params T[] values)
        {
            if (typeof(T) == typeof(int))
            {
                return selectInOrNotIn<T>(this, selectInString, false, values);
            }

            return selectInOrNotIn<T>(this, selectInString, values);
        }
        /// <summary>
        /// where field in (value,value,value)。同SelectIn。传入Array或List&lt;T>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation In<T>(params T[] values)
        {
            return SelectIn(values);
        }
        /// <summary>
        /// where field in (value,value,value)。 传入Array或List&lt;T>。
        /// </summary>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public WhereOperation SelectIn<T>(List<T> values)
        {
            return SelectIn(values.ToArray());
        }
        /// <summary>
        /// where field in (value,value,value)。同SelectIn。传入Array或List&lt;T>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation In<T>(List<T> values)
        {
            return SelectIn(values.ToArray());
        }

        /// <summary>
        /// where field in (value,value,value)。同In.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation SelectNotIn(params object[] values)
        {
            return selectInOrNotIn<object>(this, selectNotInString, values);
        }
        /// <summary>
        /// 同SelectNotIn。
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation NotIn(params object[] values)
        {
            return SelectNotIn(values);
        }

        /// <summary>
        /// SelectNotIn  。传入Array或List&lt;T>。
        /// </summary>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public WhereOperation SelectNotIn<T>(params T[] values)
        {

            if (typeof(T) == typeof(int))
            {
                return selectInOrNotIn<T>(this, selectNotInString, false, values);
            }

            return selectInOrNotIn<T>(this, selectNotInString, values);
        }
        /// <summary>
        /// 同SelectNotIn。传入Array或List&lt;T>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation NotIn<T>(params T[] values)
        {
            return SelectNotIn(values);
        }
        /// <summary>
        /// SelectNotIn。传入Array或List&lt;T>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation SelectNotIn<T>(List<T> values)
        {
            return SelectNotIn(values.ToArray());
        }
        /// <summary>
        /// 同SelectNotIn。传入Array或List&lt;T>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereOperation NotIn<T>(List<T> values)
        {
            return SelectNotIn(values.ToArray());
        }
        /// <summary>
        /// SubQueryIn   子查询  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public WhereOperation SubQueryIn(Search from)
        {
            return subQuery(this, from, selectInString);
        }

        /// <summary>
        /// SubQueryNotIn   子查询  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public WhereOperation SubQueryNotIn(Search from)
        {
            return subQuery(this, from, selectNotInString);
        }

        /// <summary>
        /// 组合 子查询
        /// </summary>
        /// <param name="field"></param>
        /// <param name="from"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        private WhereOperation subQuery(Field field, Search from, QueryOperator oper)
        {
            return subQuery(field, from, DataUtils.ToString(oper));
        }

        /// <summary>
        /// 组合 子查询
        /// </summary>
        /// <param name="field"></param>
        /// <param name="from"></param>
        /// <param name="join"></param>
        /// <returns></returns>
        private WhereOperation subQuery(Field field, Search from, string join)
        {
            if (Field.IsNullOrEmpty(field))
                return null;
            if (from.DbProvider.DatabaseType == DatabaseType.MySql)
            {
                return new WhereOperation(string.Concat(field.TableFieldName, join, "(SELECT * FROM (", from.GetPagedFromSection().SqlString, ") AS TEMP" + DataUtils.GetNewParamCount() + ")"), from.Parameters.ToArray());
            }
            return new WhereOperation(string.Concat(field.TableFieldName, join, "(", from.GetPagedFromSection().SqlString, ")"), from.Parameters.ToArray());
        }

        /// <summary>
        /// SubQueryEqual   子查询  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public WhereOperation SubQueryEqual(Search from)
        {
            return subQuery(this, from, QueryOperator.Equal);
        }

        /// <summary>
        /// SubQueryNotEqual   子查询  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public WhereOperation SubQueryNotEqual(Search from)
        {
            return subQuery(this, from, QueryOperator.NotEqual);
        }

        /// <summary>
        /// SubQueryLess   子查询  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public WhereOperation SubQueryLess(Search from)
        {
            return subQuery(this, from, QueryOperator.Less);
        }

        /// <summary>
        /// SubQueryLessOrEqual   子查询  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public WhereOperation SubQueryLessOrEqual(Search from)
        {
            return subQuery(this, from, QueryOperator.LessOrEqual);
        }

        /// <summary>
        /// SubQueryGreater   子查询  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public WhereOperation SubQueryGreater(Search from)
        {
            return subQuery(this, from, QueryOperator.Greater);
        }

        /// <summary>
        /// SubQueryGreaterOrEqual   子查询  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public WhereOperation SubQueryGreaterOrEqual(Search from)
        {
            return subQuery(this, from, QueryOperator.GreaterOrEqual);
        }


        /// <summary>
        /// 字段 为null <example>field is null</example>
        /// </summary>
        /// <returns></returns>
        public WhereOperation IsNull()
        {
            return new WhereOperation(string.Concat(this.TableFieldName, " is null "));
        }
        /// <summary>
        /// 字段 为null <example>field is not null</example>
        /// </summary>
        /// <returns></returns>
        public WhereOperation IsNotNull()
        {
            return new WhereOperation(string.Concat(this.TableFieldName, " is not null "));
        }

        /// <summary>
        /// Between操作
        /// </summary>
        /// <example>
        /// <![CDATA[ a>=value and a<=value ]]>
        /// </example>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        public WhereOperation Between(object leftValue, object rightValue)
        {
            return this >= leftValue && this <= rightValue;
        }


        #region 操作符重载

        #region WhereClip
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static WhereOperation operator ==(Field leftField, Field rightField)
        {
            return createWhereClip(leftField, rightField, QueryOperator.Equal);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static WhereOperation operator !=(Field leftField, Field rightField)
        {
            return createWhereClip(leftField, rightField, QueryOperator.NotEqual);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static WhereOperation operator >(Field leftField, Field rightField)
        {
            return createWhereClip(leftField, rightField, QueryOperator.Greater);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static WhereOperation operator >=(Field leftField, Field rightField)
        {
            return createWhereClip(leftField, rightField, QueryOperator.GreaterOrEqual);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static WhereOperation operator <(Field leftField, Field rightField)
        {
            return createWhereClip(leftField, rightField, QueryOperator.Less);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static WhereOperation operator <=(Field leftField, Field rightField)
        {
            return createWhereClip(leftField, rightField, QueryOperator.LessOrEqual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WhereOperation operator ==(Field field, object value)
        {
            return new WhereOperation(field, value, QueryOperator.Equal);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static WhereOperation operator ==(object value, Field field)
        {
            return new WhereOperation(field, value, QueryOperator.Equal);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WhereOperation operator !=(Field field, object value)
        {
            return new WhereOperation(field, value, QueryOperator.NotEqual);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static WhereOperation operator !=(object value, Field field)
        {
            return new WhereOperation(field, value, QueryOperator.NotEqual);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WhereOperation operator >(Field field, object value)
        {
            return new WhereOperation(field, value, QueryOperator.Greater);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static WhereOperation operator >(object value, Field field)
        {
            return new WhereOperation(field, value, QueryOperator.Less);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WhereOperation operator >=(Field field, object value)
        {
            return new WhereOperation(field, value, QueryOperator.GreaterOrEqual);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static WhereOperation operator >=(object value, Field field)
        {
            return new WhereOperation(field, value, QueryOperator.LessOrEqual);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WhereOperation operator <(Field field, object value)
        {
            return new WhereOperation(field, value, QueryOperator.Less);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static WhereOperation operator <(object value, Field field)
        {
            return new WhereOperation(field, value, QueryOperator.Greater);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WhereOperation operator <=(Field field, object value)
        {
            return new WhereOperation(field, value, QueryOperator.LessOrEqual);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static WhereOperation operator <=(object value, Field field)
        {
            return new WhereOperation(field, value, QueryOperator.GreaterOrEqual);
        }

        #endregion


        /// <summary>
        /// +
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Field operator +(Field leftField, Field rightField)
        {
            return createField(leftField, rightField, QueryOperator.Add);
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Field operator -(Field leftField, Field rightField)
        {
            return createField(leftField, rightField, QueryOperator.Subtract);
        }

        /// <summary>
        /// *
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Field operator *(Field leftField, Field rightField)
        {
            return createField(leftField, rightField, QueryOperator.Multiply);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Field operator /(Field leftField, Field rightField)
        {
            return createField(leftField, rightField, QueryOperator.Divide);
        }

        /// <summary>
        /// %
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Field operator %(Field leftField, Field rightField)
        {
            return createField(leftField, rightField, QueryOperator.Modulo);
        }


        /// <summary>
        /// +
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression operator +(Field leftField, object value)
        {
            return new Expression(leftField, value, QueryOperator.Add);
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression operator -(Field leftField, object value)
        {
            return new Expression(leftField, value, QueryOperator.Subtract);
        }


        /// <summary>
        /// *
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression operator *(Field leftField, object value)
        {
            return new Expression(leftField, value, QueryOperator.Multiply);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression operator /(Field leftField, object value)
        {
            return new Expression(leftField, value, QueryOperator.Divide);
        }

        /// <summary>
        /// %
        /// </summary>
        /// <param name="leftField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression operator %(Field leftField, object value)
        {
            return new Expression(leftField, value, QueryOperator.Modulo);
        }


        /// <summary>
        /// +
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Expression operator +(object value, Field rightField)
        {
            return new Expression(rightField, value, QueryOperator.Add, false);
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Expression operator -(object value, Field rightField)
        {
            return new Expression(rightField, value, QueryOperator.Subtract, false);
        }

        /// <summary>
        /// *
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Expression operator *(object value, Field rightField)
        {
            return new Expression(rightField, value, QueryOperator.Multiply, false);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Expression operator /(object value, Field rightField)
        {
            return new Expression(rightField, value, QueryOperator.Divide, false);
        }

        /// <summary>
        /// %
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rightField"></param>
        /// <returns></returns>
        public static Expression operator %(object value, Field rightField)
        {
            return new Expression(rightField, value, QueryOperator.Modulo, false);
        }

        #endregion

        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Field
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        public FieldAttribute(string fieldName)
        {
            this.Field = fieldName;
        }
    }
}
