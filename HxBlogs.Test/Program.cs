using Hx.Framework.Mappers;
using HxBlogs.Model;
using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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
            //BlogContext c = new BlogContext();
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


            var param = Expression.Property(Expression.Constant(new Blog()), "IsDeleted");
            var bin = Expression.Equal(param, Expression.Constant("Y"));
            //var invokedExpr = Expression.Invoke(bin, param);
            var adnBin = Expression.AndAlso(bin, Expression.Constant(false));
            Console.WriteLine(adnBin.ToString());
            Console.ReadLine();
        }
    }
}
