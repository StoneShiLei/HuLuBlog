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
        public string ArticleID { get; set; }

        public string UserID { get; set; }

        [SugarColumn(IsNullable = false)]
        public string UserName { get; set; }

        [SugarColumn(IsNullable = false)]
        public string CommentContent { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Email { get; set; }

        public string PID { get; set; }

        [SugarColumn(IsNullable = false)]
        public bool IsReceive { get; set; }

        [SugarColumn(IsNullable = false)]
        public bool IsChild { get; set; }

        public string ReplyTo { get; set; }

        public string ReplyToID { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<Comment> ChildComments { get; set; }

        public Comment() : base() { }

    }
}
