using MySql.Data.Entity;
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
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    internal class BlogContext:DbContext
    {
        public BlogContext() : base("name=MyContext")
        {
            Database.SetInitializer(new SeedDataInitializer());
        }
        /// <summary>
        /// 移除EF映射默认给表名添加“s“或者“es”
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());
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
        public DbSet<BlogTag> BlogTag { get; set; }
    }
    internal class SeedDataInitializer : CreateDatabaseIfNotExists<BlogContext>
    {
        protected override void Seed(BlogContext context)
        {
            base.Seed(context);
            context.Set<User>().Add(new User()
            {
                UserName = "Admin",
                PassWord = Hx.Common.Security.SafeHelper.MD5TwoEncrypt("123456"),
                NickName = "超级管理员",
                RealName = "管理员",
                Email = "stjworkemail@163.com",
                Admin = "Y",
                UseMdEdit="Y",
                RegisterTime = DateTime.Now
            });
            context.Set<Category>().AddRange(new Category[] {
                new Category{Name="前端",},
                new Category{Name="后端",},
                new Category{Name="编程语言",}
            });
            context.Set<BlogType>().AddRange(new BlogType[] {
                new BlogType{Name="原创",},
                new BlogType{Name="转载",},
                new BlogType{Name="翻译",}
            });
        }
    }

}
