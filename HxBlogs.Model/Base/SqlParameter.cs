using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Base
{
    public class SqlParameter
    {
        /// <summary>
        /// 参数名字
        /// </summary>
        private string parameterName;
        private object value;
        //
        // 摘要:
        //     使用名称和值初始化一个SqlParameter
        //     
        //
        // 参数:
        //   parameterName:
        //     要映射的参数的名称。
        //
        //   value:
        //     An System.Object that is the value of the MySql.Data.MySqlClient.MySqlParameter.
        public SqlParameter(string parameterName, object value)
        {
            this.parameterName = parameterName;
            this.value = value;
        }
        //
        // 摘要:
        //    使用参数名称和数据类型实例化对象
        //     .
        //
        // 参数:
        //   parameterName:
        //     要映射的参数的名称。
        //
        //   dbType:
        //    参数的类型
        public SqlParameter(string parameterName, MySqlDbType dbType)
        {

        }
        //public SqlParameter(string parameterName, object value)
        //{
        //    this.parameterName = parameterName;
        //    this.value = value;
        //}
        /// <summary>
        /// 把参数映射为查询可识别的参数
        /// </summary>
        /// <returns></returns>
        public DbParameter ToDbParameter()
        {
            return new MySqlParameter(this.parameterName, this.value);
        }

    }
}
