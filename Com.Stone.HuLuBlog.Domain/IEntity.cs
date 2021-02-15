using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Stone.HuLuBlog.Domain
{
    public interface IEntity
    {
        string ID { get; set; }
        DateTime AddDateTime { get; set; }
    }
}
