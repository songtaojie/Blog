using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Context
{
    internal class DbSession : IDbSession
    {
        public DbContext DbContext
        {
            get
            {
                return DbFactory.GetDbContext();
            }
        }

        public bool SaveChange()
        {
            return this.DbContext.SaveChanges() > 0;
        }
    }
}
