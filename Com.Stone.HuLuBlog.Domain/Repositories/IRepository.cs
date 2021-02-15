using Com.Stone.HuLuBlog.Infrastructure.AutoFac;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Com.Stone.HuLuBlog.Domain.Repositories
{
    public interface IRepository<TEntity>:IDependency where TEntity:class,IEntity,new()
    {
        IRepositoryContext DbContext { get; }
        SqlSugarClient SugarClient { get; }
        SimpleClient<TEntity> SimpleClient { get; }

        #region 查
        /// <summary>
        /// 查询一组数据
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <returns></returns>
        IList<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// 查询一组数据
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        IList<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] orderBy);

        /// <summary>
        /// 查询一个实体
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键查询一个实体
        /// </summary>
        /// <param name="pk">主键值</param>
        /// <returns></returns>
        TEntity GetByPk(object pk);

        /// <summary>
        /// 查询是否包含此条数据
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询符合条件的数据数量
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 增
        /// <summary>
        /// 插入并返回实体  只是自identity 添加到参数的实体里面并返回，没有查2次库
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// 批量插入（优化效率），返回影响行数
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Add(List<TEntity> entities);
        #endregion

        #region 删
        /// <summary>
        /// 根据指定的列批量删除，返回影响行数
        /// </summary>
        /// <param name="where">指定的列</param>
        /// <param name="value">依据指定列的值</param>
        /// <returns></returns>
        int Remove(Expression<Func<TEntity, object>> where, List<object> value);

        /// <summary>
        /// 根据表达式删除一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Remove(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键批量删除，返回影响行数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        int Remove(List<object> ids);

        /// <summary>
        /// 根据实体批量删除，返回影响行数
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Remove(List<TEntity> entitys);

        /// <summary>
        /// 根据主键删除，返回布尔值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Remove(object id);
        #endregion

        #region 改
        /// <summary>
        /// 更新数据，返回影响行数
        /// </summary>
        /// <param name="entity">需要更新的实体</param>
        /// <param name="updateColumns">需要更新的列，需包含依据条件列</param>
        /// <param name="whereColumns">更新的依据条件，为空则依据主键</param>
        /// <returns></returns>
        int Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns = null, Expression<Func<TEntity, object>> whereColumns = null);

        /// <summary>
        /// 更新数据，返回影响行数
        /// </summary>
        /// <param name="entities">需要更新的实体列表</param>
        /// <param name="updateColumns">需要更新的列，需包含依据条件列</param>
        /// <param name="whereColumns">更新的依据条件，为空则依据主键</param>
        /// <returns></returns>
        int Update(List<TEntity> entitys, Expression<Func<TEntity, object>> updateColumns = null, Expression<Func<TEntity, object>> whereColumns = null);

        /// <summary>
        /// 更新一条数据 返回布尔值
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Update(TEntity entity);

        /// <summary>
        /// 根据表达式中的列进行更新，只更新部分字段
        /// </summary>
        /// <param name="columns">更新的列和更新的值</param>
        /// <param name="where">where条件</param>
        /// <returns></returns>
        int Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> @where);
        #endregion

        #region 事务
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBackTran();
        #endregion

    }
}
