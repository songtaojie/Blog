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
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HxBlogs.Model.Context.BlogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

//            context.Set<Blog>().AddRange(new Blog[] {
//                new Blog{
//                    Title="ExtJs6.5 按指定样式导出",
//                    TypeId = 1,
//                    CatId = 1,
//                    Content = @"最近项目使用ExtJs6.5进行开发，<script>alert('sss');</script>要把Grid列表中的数据导出到Excel文件并带上导出的字体颜色，
//字体大小以及背景色等样式，ExtJs中自带的有一个Grid列表导出的插件
//[Ext.grid.plugin.Exporter ](https://docs.sencha.com/extjs/6.5.1/modern/Ext.grid.plugin.Exporter.html)
//可是经过了解并使用后发现它在导出样式方面的设置好像不起作用，而且有些设置了背景色的样式导致它设置的背景是
//全黑的，不知道是我设置的问题还是什么问题，于是就使用ExtJs提供的一些Excel的操作类实现了一个导出的功能，
//主要是使用[Ext.exporter.file.excel.Workbook](https://docs.sencha.com/extjs/6.5.1/classic/Ext.exporter.file.excel.Workbook.html)
//这个类以及相关的类来组织成Excel的xml格式的文件并导出成exxcel文件,里面有很多地方都不是很规范，所用的一些代码文件以及样式文件的相关资源如下：",
//                    IsTop = "Y"

//                },
//                new Blog
//                {
//                    Title="前后端开发",
//                    TypeId = 2,
//                    CatId = 2,
//                    Content = @"最近项目使用ExtJs6.5进行开发，要把Grid列表中的数据导出到Excel文件并带上导出的字体颜色，
//字体大小以及背景色等样式，ExtJs中自带的有一个Grid列表导出的插件
//可是经过了解并使用后发现它在导出样式方面的设置好像不起作用，而且有些设置了背景色的样式导致它设置的背景是
//全黑的，不知道是我设置的问题还是什么问题，于是就使用ExtJs提供的一些Excel的操作类实现了一个导出的功能，
//这个类以及相关的类来组织成Excel的xml格式的文件并导出成exxcel文件,里面有很多地方都不是很规范，所用的一些代码文件以及样式文件的相关资源如下：",
//                    IsTop = "Y"
//                }
//            });
        }
    }
}
