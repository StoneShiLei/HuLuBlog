using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Stone.HuLuBlog.Domain.Model
{
    [SugarTable("User")]
    public class User:BaseEntity
    {
        [SugarColumn(IsNullable = false)]
        public string UserName { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Email { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Password { get; set; }

        public User() : base() { }
    }
}
