using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Com.Stone.HuLuBlog.Domain;
using Com.Stone.HuLuBlog.Domain.Repositories;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Repositories.SqlSugar
{
    public abstract class SqlSugarRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        public IRepositoryContext DbContext { get; }
        public SqlSugarClient SugarClient { get; }
        public SimpleClient<TEntity> SimpleClient { get; }

        protected SqlSugarRepository(Type contextType, ISqlSugarRepositoryContext context)
        {
            //根据子类提供的contextType  取得数据库上下文
            var dbContext = context.GetDbContext(contextType);
            if (dbContext != null)
            {
                this.DbContext = dbContext;
                this.SugarClient = dbContext.DbContext;
                this.SimpleClient = dbContext.DbContext.GetSimpleClient<TEntity>();
            }
        }

        #region 查

        public IList<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate = null)
        {
            return SugarClient.Queryable<TEntity>().WhereIF(predicate != null, predicate).ToList();
        }

        public IList<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] orderBy)
        {
            var query = SugarClient.Queryable<TEntity>().WhereIF(predicate != null, predicate);
            if (orderBy.Length > 0)
            {
                foreach (var o in orderBy)
                {
                    query.OrderBy(o);
                }
            }
            return query.ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return SugarClient.Queryable<TEntity>().Where(predicate).First();
        }

        public TEntity GetByPk(object pk)
        {
            return SimpleClient.GetById(pk);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return SugarClient.Queryable<TEntity>().WhereIF(predicate != null, predicate).Count();
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return SugarClient.Queryable<TEntity>().Where(predicate).Any();
        }
        #endregion

        #region 增

        public TEntity Add(TEntity entity)
        {
            return SugarClient.Insertable(entity).ExecuteReturnEntity();
        }

        public int Add(List<TEntity> entities)
        {
            return SugarClient.Insertable(entities).ExecuteCommand();
        }
        #endregion

        #region 删

        public int Remove(Expression<Func<TEntity, object>> where, List<object> value)
        {
            return SugarClient.Deleteable<TEntity>().In(where, value).ExecuteCommand();
        }

        public int Remove(Expression<Func<TEntity, bool>> predicate)
        {
            return SugarClient.Deleteable(predicate).ExecuteCommand();
        }

        public int Remove(List<object> ids)
        {
            return SugarClient.Deleteable<TEntity>(ids).ExecuteCommand();
        }

        public int Remove(List<TEntity> entitys)
        {
            return SugarClient.Deleteable(entitys).ExecuteCommand();
        }

        public bool Remove(object id)
        {
            return SimpleClient.DeleteById(id);
        }
        #endregion

        #region 改

        public int Update(TEntity entity,
            Expression<Func<TEntity, object>> updateColumns = null,
            Expression<Func<TEntity, object>> whereColumns = null)
        {
            var command = SugarClient.Updateable(entity);
            if (updateColumns != null)
            {
                command.UpdateColumns(updateColumns);
            }
            if (whereColumns != null)
            {
                command.WhereColumns(whereColumns);
            }
            return command.ExecuteCommand();
        }

        public int Update(List<TEntity> entities,
            Expression<Func<TEntity, object>> updateColumns = null,
            Expression<Func<TEntity, object>> whereColumns = null)
        {
            var command = SugarClient.Updateable(entities);
            if (updateColumns != null)
            {
                command.UpdateColumns(updateColumns);
            }
            if (whereColumns != null)
            {
                command.WhereColumns(whereColumns);
            }
            return command.ExecuteCommand();
        }

        public bool Update(TEntity entity)
        {
            return SimpleClient.Update(entity);
        }

        public int Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> @where)
        {
            return SugarClient.Updateable<TEntity>().SetColumns(columns).Where(@where).ExecuteCommand();
        }

        #endregion

        #region 事务

        public void BeginTran()
        {
            SugarClient.Ado.BeginTran();
        }

        public void CommitTran()
        {
            SugarClient.Ado.CommitTran();
        }

        public void RollBackTran()
        {
            SugarClient.Ado.RollbackTran();
        }

        #endregion
    }
}
