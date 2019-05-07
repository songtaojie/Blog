using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.BLL
{
    /// <summary>
    /// 包含事务的操作类
    /// </summary>
    public class TransactionManager
    {
        private static IDbSession _dbSession;
        protected internal static IDbSession DbSession
        {
            get
            {
                if (_dbSession == null)
                {
                    _dbSession = DbFactory.GetDbSession();
                }
                return _dbSession;
            }
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="handler"></param>
        public static void Excute(EventHandler handler)
        {
            Exception inner = null;
            using (DbContextTransaction transaction = DbSession.DbContext.Database.BeginTransaction())
            {
                try
                {
                    handler?.Invoke(null, EventArgs.Empty);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    inner = e;
                    transaction.Rollback();
                }
            }
            if (inner != null)
            {
                throw new System.Reflection.TargetInvocationException(inner);
            }
        }
        /// <summary>
        /// 事务提交时，最后一次性保存所有的更改
        /// </summary>
        /// <returns></returns>
        public static bool SaveChanges()
        {
            return DbSession.SaveChange();
        }
    }
}
