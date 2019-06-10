using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Mapping
{
    internal class MatchLong : BaseMatch<long>
    {
        public MatchLong(object input) : base(input)
        {
        }

        public override bool Available
        {
            get
            {
                return long.TryParse(this.stringValue, out this.result);
            }
        }
    }
}
