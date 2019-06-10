using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Mapping
{
    internal interface IMatchValue
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        bool Available { get; }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        object GetValue();
    }
    internal class MatchFactory
    {
        public static IMatchValue Create(SqlDbType paramType, object value)
        {
            IMatchValue result;
            switch (paramType)
            {
                case SqlDbType.Int:
                    result = new MatchInt(value);
                    break;
                case SqlDbType.DateTime:
                    result = new MatchDateTime(value);
                    break;
                case SqlDbType.Numeric:
                    result = new MatchDecimal(value);
                    break;
                case SqlDbType.Bool:
                    result = new MatchBool(value);
                    break;
                case SqlDbType.Long:
                    result = new MatchLong(value);
                    break;
                default:
                    result = new MatchString(value);
                    break;
            }
            return result;
        }
    }
}
