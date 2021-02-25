using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Domain.Model
{
    public class Article:BaseEntity
    {
        [SugarColumn(IsNullable = false)]
        public string UserID { get; set; }

        [SugarColumn(IsNullable = false)]
        public string UserName { get; set; }

        [SugarColumn(IsNullable = false)]
        public string TagID { get; set; }

        [SugarColumn(IsNullable = false)]
        public string TagName { get; set; }

        [SugarColumn(IsNullable = false)]
        public string ArticleTitle { get; set; }

        public string ArticleContent { get; set; }

        public string HtmlContent { get; set; }

        public string MarkDownContent { get; set; }

        public bool IsDelete { get; set; }

        public bool IsRecommend { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int ReadCount { get; set; }

        public int CommentCount { get; set; }

        public string ImagePath { get; set; }

        public Article() : base() { }
    }
}
