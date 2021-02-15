using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Infrastructure;
using SqlSugar;

namespace Com.Stone.HuLuBlog.Domain.Model
{
    public  class BaseEntity:IEntity
    {
        [SugarColumn(IsPrimaryKey = true,IsNullable = false)]
        public string ID { get; set; }

        public DateTime AddDateTime { get; set; }

        public BaseEntity()
        {
            ID = Utils.GetGuidStr();
            AddDateTime = DateTime.Now;
        }
    }
}
