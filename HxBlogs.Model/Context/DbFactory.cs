using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Context
{
    public static class DbFactory
    {
        /// <summary>
        /// 获取EF的执行上下文
        /// </summary>
        /// <returns></returns>
        public static DbContext GetDbContext()
        {
            DbContext db = CallContext.GetData("DbContext") as DbContext;
            if (db == null)
            {
                db = new BlogContext();
                CallContext.SetData("DbContext", db);
            }
            return db;
        }
        /// <summary>
        /// 获取IDbSession对象
        /// </summary>
        /// <returns></returns>
        public static IDbSession GetDbSession()
        {
            IDbSession db = CallContext.GetData("DbSession") as IDbSession;
            if (db == null)
            {
                db = new DbSession();
                CallContext.SetData("DbSession", db);
            }
            return db;
        }
    }
}
