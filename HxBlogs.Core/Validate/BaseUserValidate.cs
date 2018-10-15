using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Security;
namespace HxBlogs.Core.Validate
{
    public class BaseUserValidate
    {
        //验证用户
        protected bool ValidateUser(string userName, string password)
        {
            string encrypt = SafeHelper.MD5TwoEncrypt(password);
            return false;
        }
    }
}
