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
using System.IO;

namespace HxBlogs.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime? time = DateTime.Now;
            Console.WriteLine(time.ToString());
            Console.ReadLine();
        }
        static void CheckCode() {
            string str = "--璇烽€夋嫨鐪佷唤--";
            EncodingInfo[] encodingInfos =  Encoding.GetEncodings();
            Encoding gb2312 = Encoding.UTF8;
            Encoding encoding;
            string text = "";
            foreach (EncodingInfo info in encodingInfos)
            {
                try
                {
                    encoding = info.GetEncoding();
                    byte[] temp = encoding.GetBytes(str);
                    byte[] temp1 = Encoding.Convert(encoding, gb2312, temp);
                    string result = gb2312.GetString(temp1);
                    text += result + "编码：" + encoding.BodyName+ Environment.NewLine;
                }
                catch (Exception e)
                {

                }
            }
            File.WriteAllText(@"D:\a.txt", text);
            Console.WriteLine(text);
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
