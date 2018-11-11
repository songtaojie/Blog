using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.IBLL
{
    public partial interface IUserInfoService : IBaseService<UserInfo>
    {
        bool Exist(string userName);
    }
}
