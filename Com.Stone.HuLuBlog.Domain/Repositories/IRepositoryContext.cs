using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Domain.Repositories
{
    public interface IRepositoryContext
    {
        SqlSugarClient DbContext { get; }
    }
}
