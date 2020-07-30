﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由WEF数据库工具, Version=3.2.3.5, Culture=neutral, PublicKeyToken=null生成;时间 2019-04-17 17:46:20.828
//     运行时版本:4.0.30319.42000
//     不建议手动更改此代码，如有需要请自行扩展partial类
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Runtime.Serialization;
using WEF;
using WEF.Common;
using WEF.Section;
using System.Collections.Generic;

namespace WEF.Models
{

    /// <summary>
    /// 实体类DBUserPoint
    /// </summary>
    [Serializable, DataContract, TableAttribute("tb_userpoint")]
    public partial class DBUserPoint : Entity
    {
        private static string m_tableName;
        public DBUserPoint() : base("tb_userpoint") { m_tableName = "tb_userpoint"; }
        public DBUserPoint(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _Uid;
        private decimal? _Points;
        /// <summary>
        /// Uid 
        /// </summary>
        [DataMember]
        public string Uid
        {
            get { return _Uid; }
            set
            {
                this.OnPropertyValueChange(_.Uid, _Uid, value);
                this._Uid = value;
            }
        }
        /// <summary>
        /// Points 
        /// </summary>
        [DataMember]
        public decimal? Points
        {
            get { return _Points; }
            set
            {
                this.OnPropertyValueChange(_.Points, _Points, value);
                this._Points = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
                _.Uid};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
                _.Uid,
                _.Points};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
                this._Uid,
                this._Points};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// tb_userpoint 
            /// </summary>
            public readonly static Field All = new Field("*", m_tableName);
            /// <summary>
            /// Uid 
            /// </summary>
            public readonly static Field Uid = new Field("uid", m_tableName, "uid");
            /// <summary>
            /// Points 
            /// </summary>
            public readonly static Field Points = new Field("points", m_tableName, "points");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBUserPoint操作类
    /// </summary>
    public partial class DBUserPointRepository
    {
        DBContext db;
        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        public ISearch<DBUserPoint> Search(string tableName = "")
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = "tb_userpoint";
            }
            return db.Search<DBUserPoint>(tableName);
        }
        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        public ISearch<DBUserPoint> Search(DBUserPoint entity)
        {
            return db.Search<DBUserPoint>(entity);
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public DBUserPointRepository()
        {
            db = new DBContext();
        }
        /// <summary>
        /// 构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBUserPointRepository(string connStrName)
        {
            db = new DBContext(connStrName);
        }
        /// <summary>
        /// 当前db操作上下文
        /// </summary>
        public DBContext DBContext
        {
            get
            {
                return db;
            }
        }
        /// <summary>
        /// 获取实体
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        public DBUserPoint GetDBUserPoint(string Uid)
        {
            return Search().Where(b => b.Uid == Uid).First();
        }
        /// <summary>
        /// 获取列表
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        public List<DBUserPoint> GetList(int pageIndex, int pageSize)
        {
            return this.Search().Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 获取列表
        /// <param name="tableName">表名</param>
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        public List<DBUserPoint> GetList(string tableName, int pageIndex = 1, int pageSize = 12)
        {
            return this.Search(tableName).Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 添加实体
        /// <param name="obj">传进的实体</param>
        /// </summary>
        public int Insert(DBUserPoint obj)
        {
            return db.Insert(obj);
        }
        /// <summary>
        /// 更新实体
        /// <param name="obj">传进的实体</param>
        /// </summary>
        public int Update(DBUserPoint obj)
        {
            return db.Update(obj);
        }
        /// <summary>
        /// 删除实体
        /// <param name="obj">传进的实体</param>
        /// </summary>
        public int Delete(DBUserPoint obj)
        {
            return db.Delete(obj);
        }
        /// <summary>
        /// 删除实体
        /// <param name="Uid">Uid</param>
        /// </summary>
        public int Delete(string Uid)
        {
            var obj = Search().Where(b => b.Uid == Uid).First();
            return db.Delete(obj);
        }
        /// <summary>
        /// 批量删除实体
        /// <param name="obj">传进的实体列表</param>
        /// </summary>
        public int Deletes(List<DBUserPoint> objs)
        {
            return db.Delete<DBUserPoint>(objs);
        }
        /// <summary>
        /// 执行sql语句
        /// <param name="sql"></param>
        /// </summary>
        public SqlSection ExecuteSQL(string sql)
        {
            return db.FromSql(sql);
        }
        /// <summary>
        /// 执行存储过程
        /// <param name="sql"></param>
        /// </summary>
        public ProcSection ExcuteProc(string procName)
        {
            return db.FromProc(procName);
        }
    }

}

