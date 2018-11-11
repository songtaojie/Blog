using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.IDAL
{
    public partial interface IUserInfoDal : IBaseDal<UserInfo>
    {
        UserInfo QueryUserByName(string username);
    }
}
