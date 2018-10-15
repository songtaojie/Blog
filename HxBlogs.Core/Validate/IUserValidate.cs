using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Core.Validate
{
    public interface IUserValidate
    {
        bool ValidateUser(ValidateUserPackage p);
    }
}
