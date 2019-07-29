namespace HxBlogs.Model.Migrations
{
    using Hx.Common.Security;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HxBlogs.Model.Context.BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HxBlogs.Model.Context.BlogContext context)
        {
            //BasicInfo basicInfo = new BasicInfo();
            //JobInfo jobInfo = new JobInfo();
            //context.Set<BasicInfo>().Add(basicInfo);
            //context.Set<JobInfo>().Add(jobInfo);
            //context.Set<UserInfo>().Add(new UserInfo()
            //{
            //    UserName = "Admin",
            //    PassWord = Hx.Common.Security.SafeHelper.MD5TwoEncrypt("123456"),
            //    NickName = "超级管理员",
            //    //RealName = "管理员",
            //    Email = "stjworkemail@163.com",
            //    Admin = "Y",
            //    UseMdEdit = "Y",
            //    RegisterTime = DateTime.Now,
            //    BasicId = basicInfo.Id,
            //    JobId = jobInfo.Id
            //});
            //context.Set<Category>().AddRange(new Category[] {
            //    new Category{Name="前端",},
            //    new Category{Name="后端",},
            //    new Category{Name="编程语言",}
            //});
            //context.Set<BlogType>().AddRange(new BlogType[] {
            //    new BlogType{Name="原创",},
            //    new BlogType{Name="转载",},
            //    new BlogType{Name="翻译",}
            //});
            context.Set<SystemConfig>().Add(new SystemConfig());
        }
    }
}