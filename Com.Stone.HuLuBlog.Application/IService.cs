using Com.Stone.HuLuBlog.Domain;
using Com.Stone.HuLuBlog.Domain.Repositories;
using Com.Stone.HuLuBlog.Infrastructure;
using Com.Stone.HuLuBlog.Infrastructure.AutoFac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Com.Stone.HuLuBlog.Application
{
    public interface IService<T>:IDependency where T:class,IEntity,new()
    {
        IRepository<T> Repository { get; }

        #region 查
        /// <summary>
        /// 根据主键查询一条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns></returns>
        T GetByPkValue(object pkValue);

        /// <summary>
        /// 根据表达式查询一条数据
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns></returns>
        T GetByClause(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询所有数据（无分页，慎用）
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// 根据表达式查询一组数据（可排序）
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="orderBy">排序方式,可选</param>
        /// <returns></returns>
        IList<T> GetAllByClause(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] orderBy);
        #endregion

        #region 增
        /// <summary>
        /// 插入一个实体并返回其最新状态
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// 插入一组实体并返回其最新状态（产生多条sql）
        /// </summary>
        /// <param name="entitys">实体</param>
        /// <param name="process">插入前对实体的操作</param>
        /// <returns></returns>
        IList<T> Add(List<T> entitys, Action<T> process = null);

        /// <summary>
        /// 插入一组实体并返回布尔值（效率优化）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool Add(List<T> entitys);
        #endregion

        #region 删

        /// <summary>
        /// 删除一组数据，whereColumns为空则依据主键删除
        /// </summary>
        /// <param name="ids">条件值的集合</param>
        /// <param name="whereColumns">条件列</param>
        /// <returns></returns>
        bool Remove(List<object> ids, Expression<Func<T, object>> whereColumns = null);

        /// <summary>
        /// 根据主键删除一条数据
        /// </summary>
        /// <param name="key">主键值</param>
        /// <returns></returns>
        bool Remove(object key);

        /// <summary>
        /// 根据表达式删除数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Remove(Expression<Func<T, bool>> @where);

        /// <summary>
        /// 根据主键批量删除
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        bool Remove(List<object> keys);
        #endregion

        #region 改
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 批量更新数据，可选更新部分字段
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="whereColumns">更新依据的条件列</param>
        /// <param name="updateColumns">需要更新的字段</param>
        /// <returns></returns>
        bool Update(List<T> entitys, Expression<Func<T, object>> whereColumns = null,
            Expression<Func<T, object>> updateColumns = null);

        /// <summary>
        /// 根据表达式中的列进行更新，只更新部分字段
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> @where);
        #endregion

        #region 分页

        /// <summary>
        /// 分页查询,返回分页结果集
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="sortPredicate">排序字段</param>
        /// <returns></returns>
        PagedResult<T> GetPagedList(Expression<Func<T, bool>> predicate, int pageIndex = 1, int pageSize = 20, SortOrder orderBy = SortOrder.UnSpecified, Expression<Func<T, dynamic>> sortPredicate = null);


        #endregion
    }
}
