using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Domain.Model
{
    [SugarTable("Comment")]
    public class Comment : BaseEntity
    {
        [SugarColumn(IsNullable = false)]
        public string ArticleID { get; set; }

        [SugarColumn(IsNullable = false)]
        public string UserID { get; set; }

        [SugarColumn(IsNullable = false)]
        public string UserName { get; set; } 

        public string CommentContent { get; set; }

        public Comment() : base() { }
    }
}
