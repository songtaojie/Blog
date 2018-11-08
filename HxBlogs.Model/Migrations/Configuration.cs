namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HxBlogs.Model.Context.BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HxBlogs.Model.Context.BlogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            UserInfo info = context.Set<UserInfo>().FirstOrDefault(u => u.UserName == "Admin");
            if (info == null)
            {
                string pwd = Common.Security.SafeHelper.MD5Encrypt("123456");
                context.Set<UserInfo>().AddOrUpdate(new UserInfo()
                {
                    UserName = "Admin",
                    PassWord = pwd,
                    PwdConfirm = pwd,
                    NickName = "超级管理员",
                    RealName = "管理员",
                    Email = "stjworkemail@163.com",
                    IsRoot = "Y"
                });
            }
        }
    }
}
