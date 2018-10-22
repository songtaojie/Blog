using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Context
{
    public class BlogContext:DbContext
    {
        public BlogContext() : base("name=MyContext")
        {
        }
        /// <summary>
        /// 移除EF映射默认给表名添加“s“或者“es”
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// 模型验证出错时抛出异常
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errMsg = ex.EntityValidationErrors.SelectMany(o => o.ValidationErrors).Select(o => o.ErrorMessage);
                var fullErrMsg = ex.Message + ":" + string.Join(";", errMsg);
                throw new DbEntityValidationException(fullErrMsg, ex.EntityValidationErrors);
            }
        }

        public DbSet<Blog> Blog { get; set; } 
    }
}
