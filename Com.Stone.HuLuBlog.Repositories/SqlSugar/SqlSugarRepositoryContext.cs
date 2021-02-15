using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Com.Stone.HuLuBlog.Domain.Repositories;

namespace Com.Stone.HuLuBlog.Repositories.SqlSugar
{
    public class SqlSugarRepositoryContext : ISqlSugarRepositoryContext
    {
        // ThreadLocal代表线程本地存储，主要相当于一个静态变量
        // 但静态变量在多线程访问时需要显式使用线程同步技术。
        // 使用ThreadLocal变量，每个线程都会一个拷贝，从而避免了线程同步带来的性能开销
        // 使用字典支持多个context切换
        private readonly ThreadLocal<Dictionary<Type, IRepositoryContext>> _localCtx =
            new ThreadLocal<Dictionary<Type, IRepositoryContext>>(() =>
            new Dictionary<Type, IRepositoryContext>() {
                { typeof(HuLuBlogContext),new HuLuBlogContext() }
            });

        public IRepositoryContext GetDbContext(Type contextType)
        {
            return _localCtx.Value[contextType];
        }
    }
}
