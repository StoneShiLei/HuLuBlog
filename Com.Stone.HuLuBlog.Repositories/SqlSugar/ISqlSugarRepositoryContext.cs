using Com.Stone.HuLuBlog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Stone.HuLuBlog.Repositories.SqlSugar
{
    public interface ISqlSugarRepositoryContext
    {
        IRepositoryContext GetDbContext(Type contextType);
    }
}
