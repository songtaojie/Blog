using Hx.Framework.Mappers;
using HxBlogs.Model;
using HxBlogs.Model.Context;
using Hx.Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace HxBlogs.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //Type baseType = typeof(IAutoMapper<>);
            //foreach (Assembly ass in assemblies)
            //{
            //    try
            //    {
            //        var types = ass.GetExportedTypes().Where(t => !t.IsInterface && !t.IsAbstract);
            //        foreach (Type sourceType in types)
            //        {
            //            var genericTypes = sourceType.GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == baseType);
            //            if (genericTypes != null && genericTypes.Count() > 0)
            //            {
            //                foreach (Type destType in genericTypes)
            //                {
            //                    if (destType.IsClass)
            //                    {

            //                    }
            //                }
            //                //Type destType = interType.GetGenericElementType();
            //                //if (destType.IsClass)
            //                //{
            //                //    CreateMap(type, destType);
            //                //}
            //            }
            //        }
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
            //foreach (var s in assemblies)
            //{
            //    foreach (Type type in s.GetTypes())
            //    {
            //        Console.WriteLine(type.Namespace+"名称："+type.Name);
            //    }
            //}
            //
            //c.Blog.Add(new Blog()
            //{
            //    Title="这是一篇测试",
            //    Content="测试"
            //});
            //c.SaveChanges();
            //Console.WriteLine("成功");
            //Assembly ass = Assembly.GetExecutingAssembly();
            //Assembly.
            //Type[] types = assembly.GetExportedTypes();
            //foreach (Type type in types)
            //{
            //    if (typeof(BaseModel).IsAssignableFrom(type) && !type.IsAbstract)
            //    {
            //        Console.WriteLine(type.FullName);
            //    }
            //}
            //Blog b = DbFactory.GetDbContext().Set<Blog>().Find(4);
            //string html = b.ContentHtml;
            //string phtml = FilterHtmlP(html);
            Console.WriteLine(PublicCount(r=>r.UserId==1));
            Console.ReadLine();
        }

        static string FilterHtmlP(string html)
        {
            Regex rReg = new Regex(@"<P>[\s\S]*?</P>", RegexOptions.IgnoreCase);
            //Match math = rReg.Match(html);
            // var matchs = Regex.Matches(html, @"(?i)<p[^>]*?>[^<>]*?</p>");
            var matchs = Regex.Matches(html, @"<P>[\s\S]*?</P>", RegexOptions.IgnoreCase);
            StringBuilder sb = new StringBuilder();
            foreach (Match match in matchs)
            {
                string pContent = match.Value;
                pContent = Regex.Replace(pContent, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                pContent = Regex.Replace(pContent, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
                sb.Append(pContent);
            }

            return sb.ToString();
        }
        public static int PublicCount(Expression<Func<Blog,bool>> lambda)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(Blog), "b");
            MemberExpression publicProp = Expression.Property(parameterExp, "IsPublish"),
                 deleteProp = Expression.Property(parameterExp, "IsDeleted"),
                 privateProp = Expression.Property(parameterExp, "IsPrivate");
            BinaryExpression publishExp = Expression.Equal(publicProp, Expression.Constant("Y"));
            BinaryExpression deleteExp = Expression.Equal(deleteProp, Expression.Constant("N"));
            BinaryExpression privateExp = Expression.Equal(privateProp, Expression.Constant("N"));
            var bin = Expression.AndAlso(Expression.AndAlso(publishExp, deleteExp), privateExp);
            var lambdaWhere = Expression.Lambda<Func<Blog, bool>>(bin, parameterExp);
            return Count(lambda.And(lambdaWhere));
        }

        public static int Count(Expression<Func<Blog,bool>> lambda)
        {
            return DbFactory.GetDbContext().Set<Blog>().Count(lambda);
        }
    }
}
