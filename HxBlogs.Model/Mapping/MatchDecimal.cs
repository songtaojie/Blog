using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Mapping
{
    internal class MatchDecimal : BaseMatch<decimal>
    {
        public MatchDecimal(object input) : base(input)
        {
        }

        public override bool Available
        {
            get
            {
                return decimal.TryParse(this.stringValue, out this.result);
            }
        }
    }
}
