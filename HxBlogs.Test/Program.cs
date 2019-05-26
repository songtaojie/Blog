using Hx.Framework.Mappers;
using HxBlogs.Model;
using HxBlogs.Model.Context;
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
            Blog b = DbFactory.GetDbContext().Set<Blog>().Find(4);
            string html = b.ContentHtml;
            string phtml = FilterHtmlP(html);
            Console.WriteLine(phtml);
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
    }
}
