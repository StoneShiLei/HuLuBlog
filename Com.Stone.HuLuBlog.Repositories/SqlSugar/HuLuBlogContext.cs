using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using Com.Stone.HuLuBlog.Domain.Repositories;
using Com.Stone.HuLuBlog.Infrastructure;

namespace Com.Stone.HuLuBlog.Repositories.SqlSugar
{
    public class HuLuBlogContext:IRepositoryContext
    {
        public SqlSugarClient DbContext { get; }

        public HuLuBlogContext()
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Configurations.ConnStrHuLuBlog,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            DbContext = db;
        }
    }
}
