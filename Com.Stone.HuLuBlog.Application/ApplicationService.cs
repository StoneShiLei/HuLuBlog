using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain;
using Com.Stone.HuLuBlog.Domain.Repositories;
using Com.Stone.HuLuBlog.Infrastructure;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Application
{
    public abstract class ApplicationService<T> : IService<T> where T : class, IEntity, new()
    {
        public IRepository<T> Repository { get; }
        public LogOperation Logger { get; }

        #region 构造方法
        protected ApplicationService(IRepository<T> repository)
        {
            Repository = repository;

            //打印sql
            Logger = new LogOperation(GetType());
            Repository.SugarClient.Aop.OnLogExecuting = (sql, pars) => Logger.Debug(sql);
        }
        #endregion

        #region 基础的批量增删改
        /// <summary>
        /// 实体批量插入，返回新的实体列表，每个实体产生一条insert语句
        /// 如不需要返回实体最新状态，使用其他方法实现批量插入
        /// </summary>
        /// <typeparam name="TList">实体列表容器</typeparam>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dataObjects">实体列表数据</param>
        /// <param name="process">对每个实体的附加操作</param>
        /// <returns></returns>
        protected TList PerformCreateObjects<TList>(TList dataObjects,
            Action<T> process = null)
            where TList : List<T>, new()
        {
            if (dataObjects == null) throw new ArgumentNullException("dataObjects is null");

            TList results = new TList();
            if (dataObjects.Count <= 0) return results;

            try
            {
                Repository.BeginTran();
                foreach (var entity in dataObjects)
                {
                    process?.Invoke(entity);
                    T result = Repository.Add(entity);
                    results.Add(result);
                }
                Repository.CommitTran();
                return results;
            }
            catch (Exception ex)
            {
                Repository.RollBackTran();
                throw ex;
            }
        }

        /// <summary>
        /// 实体批量更新
        /// </summary>
        /// <typeparam name="TList">实体列表容器</typeparam>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dataObjects">需要更新的实体列表</param>
        /// <param name="whereColumns">需要更新的列，需包含依据条件列</param>
        /// <param name="updateColumns">更新的依据条件，为空则依据主键</param>
        /// <returns></returns>
        protected int PerformUpdateObjects<TList>(TList dataObjects,
            Expression<Func<T, object>> whereColumns = null,
            Expression<Func<T, object>> updateColumns = null)
            where TList : List<T>, new()
        {
            if (dataObjects == null) throw new ArgumentNullException("dataObjects is null");
            if (dataObjects.Count > 0)
            {
                return Repository.Update(dataObjects, updateColumns, whereColumns);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 数据的批量删除,不指定条件则根据主键删除
        /// </summary>
        /// <typeparam name="T">实体的类型</typeparam>
        /// <param name="ids">value值的集合</param>
        /// <param name="whereColumns">删除数据的依据条件，为空则依据主键删除</param>
        /// <returns></returns>
        protected int PerformDeleteObjects(IList<object> ids,
            Expression<Func<T, object>> whereColumns = null)
        {
            if (ids == null) throw new ArgumentNullException("ids is null");

            if (ids.Count > 0)
            {
                if (whereColumns == null)
                {
                    return Repository.Remove(ids.ToList());
                }
                else
                {
                    return Repository.Remove(whereColumns, ids.ToList());
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 增
        public T Add(T entity)
        {
            return Repository.Add(entity);
        }

        public IList<T> Add(List<T> entitys, Action<T> process = null)
        {
            return PerformCreateObjects(entitys, process);
        }

        public bool Add(List<T> entitys)
        {
            return Repository.Add(entitys) > 0;
        }
        #endregion

        #region 删
        public bool Remove(List<object> ids, Expression<Func<T, object>> whereColumns = null)
        {
            return PerformDeleteObjects(ids, whereColumns) > 0;
        }

        public bool Remove(object key)
        {
            return Repository.Remove(key);
        }

        public bool Remove(Expression<Func<T, bool>> where)
        {
            return Repository.Remove(where) > 0;
        }

        public bool Remove(List<object> keys)
        {
            return Repository.Remove(keys) > 0;
        }
        #endregion

        #region 改
        public bool Update(T entity)
        {
            return Repository.Update(entity);
        }

        public bool Update(List<T> entitys, Expression<Func<T, object>> whereColumns = null, Expression<Func<T, object>> updateColumns = null)
        {
            return PerformUpdateObjects(entitys, whereColumns, updateColumns) > 0;
        }

        public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> @where)
        {
            return Repository.Update(columns, where) > 0;
        }
        #endregion

        #region 查
        public T GetByPkValue(object pkValue)
        {
            return Repository.GetByPk(pkValue);
        }

        public T GetByClause(Expression<Func<T, bool>> predicate)
        {
            return Repository.Get(predicate);
        }

        public IList<T> GetAll()
        {
            return Repository.GetAllList();
        }

        public IList<T> GetAllByClause(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] orderBy)
        {
            return Repository.GetAllList(predicate, orderBy);
        }
        #endregion

        #region 分页查询
        public PagedResult<T> GetPagedList(Expression<Func<T, bool>> predicate, int pageIndex = 1, int pageSize = 20, SortOrder orderBy = SortOrder.UnSpecified, Expression<Func<T, dynamic>> sortPredicate = null)
        {

            var totalCount = 0;

            OrderByType order;
            switch (orderBy)
            {
                case SortOrder.Ascending:
                    order = OrderByType.Asc;
                    break;
                case SortOrder.Descending:
                    order = OrderByType.Desc;
                    break;
                default:
                    throw new Exception("无法识别的OrderBy参数");
            }

            //Repository.SugarClient.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            //{
            //    var yyy = sql;
            //    var xxx = Repository.SugarClient.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
            //};

            var pagedList = Repository.SugarClient.Queryable<T>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(orderBy != SortOrder.UnSpecified, sortPredicate, order)
                .ToPageList(pageIndex, pageSize, ref totalCount);



            PagedResult<T> result = new PagedResult<T>(totalCount,
                (totalCount + pageSize - 1) / pageSize,
                pageSize, pageIndex, pagedList);

            return result;
        }

        #endregion
    }
}
