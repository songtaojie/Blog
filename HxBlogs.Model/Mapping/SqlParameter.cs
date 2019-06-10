using HxBlogs.Model.Mapping;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    [Serializable]
    public class SqlParameter : IParameter
    {
        /// <summary>
        /// 参数名字
        /// </summary>
        private object paramValue;
        private readonly SqlDbType paramType;
      
        public SqlDbType ParamType
        {
            get
            {
                return this.paramType;
            }
        }
        public SqlParameter()
        {
            this.paramType = SqlDbType.Varchar;
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
        public SqlParameter(SqlDbType dbType)
        {
            this.paramType = dbType;
        }


        public object ParamValue
        {
            get
            {
                return MatchFactory.Create(this.ParamType, this.paramValue).GetValue();
            }
            set
            {
                this.paramValue = value;
            }
        }

        public DbParameter Create(string paramName)
        {
            MySqlDbType dbType = MySqlDbType.VarChar;
            switch (this.paramType)
            {
                case SqlDbType.Bool:
                    dbType = MySqlDbType.Bit;
                    break;
                case SqlDbType.Varchar:
                    dbType = MySqlDbType.VarChar;
                    break;
                case SqlDbType.DateTime:
                    dbType = MySqlDbType.DateTime;
                    break;
                case SqlDbType.Int:
                    dbType = MySqlDbType.Int32;
                    break;
                case SqlDbType.Long:
                    dbType = MySqlDbType.Int64;
                    break;
                case SqlDbType.Numeric:
                    dbType = MySqlDbType.Decimal;
                    break;
                default:
                    dbType = MySqlDbType.VarChar;
                    break;
            }

            return new MySqlParameter(paramName, dbType)
            {
                Value = GetParamValue(this.ParamValue)
            };
        }
        private object GetParamValue(object paramValue)
        {
            return (paramValue != null) ? paramValue : DBNull.Value;
        }
    }
    public enum SqlDbType
    {
        Bool,
        Varchar,
        Int,
        Long,
        DateTime,
        Numeric
    }
}
