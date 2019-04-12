﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WEF.Common;
using WEF.Expressions;
using WEF.MvcPager;

namespace WEF
{
    public interface ISearch<T> where T : Entity
    {
        Search<T> AddSelect(Search fromSection);
        Search<T> AddSelect(Search fromSection, string aliasName);
        Search<T> CrossJoin<TEntity>(WhereClip where) where TEntity : Entity;
        Search<T> Distinct();
        T First();
        TResult First<TResult>() where TResult : class;
        Search<T> From(int startIndex, int endIndex);
        Search<T> FullJoin<TEntity>(WhereClip where) where TEntity : Entity;
        Search<T> GroupBy(Expression<Func<T, object>> lambdaGroupBy);
        Search<T> GroupBy(GroupByClip groupBy);
        Search<T> GroupBy(params Field[] fields);
        Search<T> Having(Expression<Func<T, bool>> lambdaHaving);
        Search<T> Having(Where where);
        Search<T> Having(WhereClip havingWhere);
        Search<T> InnerJoin(Search fs);
        Search<T> InnerJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaWhere, string asName = "") where TEntity : Entity;
        Search<T> InnerJoin<TEntity>(WhereClip where, string asName = "", string asName2 = "") where TEntity : Entity;
        Search<T> LeftJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaWhere) where TEntity : Entity;
        Search<T> LeftJoin<TEntity>(WhereClip where) where TEntity : Entity;
        Search<T> OrderBy(Expression<Func<T, object>> lambdaOrderBy);
        Search<T> OrderBy(OrderByClip orderBy);
        Search<T> OrderBy(params Field[] f);
        Search<T> OrderBy(params OrderByClip[] orderBys);
        Search<T> OrderByDescending(Expression<Func<T, object>> lambdaOrderBy);
        Search<T> OrderByDescending(params Field[] f);
        Search<T> Page(int pageSize, int pageIndex);
        Search<T> Refresh();
        Search<T> RightJoin<TEntity>(WhereClip where) where TEntity : Entity;
        Search<T> Select(Expression<Func<T, bool>> lambdaSelect);
        Search<T> Select(Expression<Func<T, object>> lambdaSelect);
        Search<T> Select(params Field[] fields);
        Search<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere);
        Search<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object>> lambdaSelect);
        Search<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere);
        Search<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object>> lambdaSelect);
        Search<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere);
        Search<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> lambdaSelect);
        Search<T> Select<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere);
        Search<T> Select<T2, T3>(Expression<Func<T, T2, T3, object>> lambdaSelect);
        Search<T> Select<T2>(Expression<Func<T, T2, bool>> lambdaWhere);
        Search<T> Select<T2>(Expression<Func<T, T2, object>> lambdaSelect);
        Search<T> SetCacheTimeOut(int timeout);
        IEnumerable<T> ToEnumerable();
        T ToFirst();
        TResult ToFirst<TResult>() where TResult : class;
        T ToFirstDefault();
        List<T> ToList();
        List<TResult> ToList<TResult>();
        Search<T> Top(int topCount);
        Search<T> Union(Search fromSection);
        Search<T> UnionAll(Search fromSection);
        Search<T> Where(Expression<Func<T, bool>> lambdaWhere);
        Search<T> Where(Where whereParam);
        Search<T> Where(Where<T> whereParam);
        Search<T> Where(WhereClip where);
        Search<T> Where<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere);
        Search<T> Where<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere);
        Search<T> Where<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere);
        Search<T> Where<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere);
        Search<T> Where<T2>(Expression<Func<T, T2, bool>> lambdaWhere);

        PagedList<T> GetPagedList(int pageIndex, int pageSize, string orderBy, bool sorted);
    }
}