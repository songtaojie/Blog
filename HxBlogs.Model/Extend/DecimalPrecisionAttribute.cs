using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DecimalPrecisionAttribute:Attribute
    {
        #region Field
         private byte _precision = 18;
         public byte _scale = 4;
         #endregion
 
         #region Construct
         /// <summary>
         /// <para>自定义Decimal类型的精确度属性</para>
         /// </summary>
         /// <param name="precision">precision
         /// <para>精度（默认18）</para></param>
         /// <param name="scale">scale
         /// <para>小数位数（默认4）</para></param>
         public DecimalPrecisionAttribute(byte precision = 18, byte scale = 4)
         {
             Precision = precision;
             Scale = scale;
         } 
         #endregion
         
         #region Property
         /// <summary>
         /// 精确度（默认18）
         /// </summary>
         public byte Precision
         {
             get { return this._precision; }
             set { this._precision = value; }
         }
 
         /// <summary>
         /// 保留位数（默认4）
         /// </summary>
         public byte Scale
         {
             get { return this._scale; }
             set { this._scale = value; }
         }
        #endregion
    }

    /// <summary>
     /// 用于modelBuilder全局设置自定义精度属性
     /// </summary>
     public class DecimalPrecisionAttributeConvention
         : PrimitivePropertyAttributeConfigurationConvention<DecimalPrecisionAttribute>
     {
         public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DecimalPrecisionAttribute attribute)
         {
             if (attribute.Precision< 1 || attribute.Precision> 38)
             {
                 throw new InvalidOperationException("Precision must be between 1 and 38.");
             }
             if (attribute.Scale > attribute.Precision)
             {
                 throw new InvalidOperationException("Scale must be between 0 and the Precision value.");
             }
             configuration.HasPrecision(attribute.Precision, attribute.Scale);
         }
    }
}
