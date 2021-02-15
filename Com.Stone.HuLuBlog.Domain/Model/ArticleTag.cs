using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Domain.Model
{
    [SugarTable("ArticleTag")]
    public class ArticleTag:BaseEntity
    {
        [SugarColumn(IsNullable = false)]
        public string UserID { get; set; }

        [SugarColumn(IsNullable = false)]
        public string TagName { get; set; }

        [SugarColumn(IsNullable = false)]
        public int ArticleCount { get; set; }

        public ArticleTag() : base() { }
    }
}
