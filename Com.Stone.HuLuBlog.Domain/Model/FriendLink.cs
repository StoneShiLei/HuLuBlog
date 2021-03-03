using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Domain.Model
{
    public class FriendLink:BaseEntity
    {
        [SugarColumn(IsNullable = false)]
        public string IconUrl { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Title { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Domain { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Url { get; set; }
    }
}
