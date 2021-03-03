﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;

namespace Com.Stone.HuLuBlog.Repositories.SqlSugar
{
    public class FriendLinkRepository : SqlSugarRepository<FriendLink>, IFriendLinkRepository
    {
        public FriendLinkRepository(ISqlSugarRepositoryContext context) : base(typeof(HuLuBlogContext), context)
        {
        }
    }
}
