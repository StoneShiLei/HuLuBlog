using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;
using Com.Stone.HuLuBlog.Infrastructure;

namespace Com.Stone.HuLuBlog.Application.ServiceImpl
{
    public class FriendLinkServiceImpl: ApplicationService<FriendLink>, IFriendLinkService
    {
        public FriendLinkServiceImpl(IFriendLinkRepository friendLinkRepository) : base(friendLinkRepository)
        {

        }
    }
}
