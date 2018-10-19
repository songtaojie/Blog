using HxBlogs.IDAL;
using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.BLL
{
    internal abstract class BaseService<T> where T:class,new()
    {
        private static IDbSession _dbSession;
        internal IBaseDal<T> baseDal;

        public IDbSession DbSession
        {
            get {
                if (_dbSession == null)
                {
                    _dbSession = DbFactory.GetDbSession();
                }
                return _dbSession;
            }
        }
    }
}
