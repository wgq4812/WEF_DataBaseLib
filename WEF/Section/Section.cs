﻿/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using WEF.Common;

namespace WEF.Section
{

    /// <summary>
    /// Section
    /// </summary>
    public abstract class Section
    {
        protected DBContext _dbContext;
        protected DbCommand _dbCommand;
        protected DbTransaction _dbTransaction = null;

        public Section(DBContext dbContext)
        {
            Check.Require(dbContext, "dbContext", Check.NotNullOrEmpty);
            this._dbContext = dbContext;
        }

        #region 执行

        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public virtual object ToScalar()
        {
            return (_dbTransaction == null ? this._dbContext.ExecuteScalar(_dbCommand) : this._dbContext.ExecuteScalar(_dbCommand, _dbTransaction));
        }


        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public TResult ToScalar<TResult>()
        {
            return DataUtils.ConvertValue<TResult>(ToScalar());
        }

        /// <summary>
        /// 返回第一个实体，同ToFirst()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public TEntity First<TEntity>()
        {
            return ToFirst<TEntity>();
        }
        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity ToFirst<TEntity>()
        {
            TEntity t = default(TEntity);
            using (IDataReader reader = ToDataReader())
            {
                var result = EntityUtils.ReaderToEnumerable<TEntity>(reader).ToArray();
                if (result.Any())
                {
                    t = result.First();
                }
            }
            return t;
        }

        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity ToFirstDefault<TEntity>()
            where TEntity : Entity
        {
            TEntity t = ToFirst<TEntity>();

            if (t == null)
                t = DataUtils.Create<TEntity>();

            return t;
        }
        /// <summary>
        /// 返回实体列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public List<TEntity> ToList<TEntity>()
        {
            using (IDataReader reader = ToDataReader())
            {
                return EntityUtils.ReaderToEnumerable<TEntity>(reader).ToList();
            }
        }
        /// <summary>
        /// 返回懒加载数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> ToEnumerable<TEntity>()
        {
            IEnumerable<TEntity> result;
            using (IDataReader reader = ToDataReader())
            {
                var info = new EntityUtils.CacheInfo()
                {
                    Deserializer = EntityUtils.GetDeserializer(typeof(TEntity), reader, 0, -1, false)
                };
                while (reader.Read())
                {
                    dynamic next = info.Deserializer(reader);
                    yield return (TEntity)next;
                }
            }
        }

        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <returns></returns>
        public virtual IDataReader ToDataReader()
        {
            return (_dbTransaction == null ? this._dbContext.ExecuteReader(_dbCommand) : this._dbContext.ExecuteReader(_dbCommand, _dbTransaction));
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <returns></returns>
        public virtual DataSet ToDataSet()
        {
            return (_dbTransaction == null ? this._dbContext.ExecuteDataSet(_dbCommand) : this._dbContext.ExecuteDataSet(_dbCommand, _dbTransaction));
        }


        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            return this.ToDataSet().Tables[0];
        }

        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <returns></returns>
        public virtual int ExecuteNonQuery()
        {
            return (_dbTransaction == null ? this._dbContext.ExecuteNonQuery(_dbCommand) : this._dbContext.ExecuteNonQuery(_dbCommand, _dbTransaction));
        }


        #endregion

    }
}

