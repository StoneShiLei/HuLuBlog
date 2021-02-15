using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Stone.HuLuBlog.Domain.Model;
using Com.Stone.HuLuBlog.Domain.Repositories;

namespace Com.Stone.HuLuBlog.Application.ServiceImpl
{
    partial class UserServiceImpl : ApplicationService<User>, IUserService
    {
        public IUserRepository UserRepository { get; }

        public UserServiceImpl(IUserRepository userRepository) : base(userRepository)
        {
            UserRepository = userRepository;
        }
    }
}

