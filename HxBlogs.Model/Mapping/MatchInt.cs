using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Mapping
{
    internal class MatchInt : BaseMatch<int>
    {
        public MatchInt(object input) : base(input)
        {
        }

        public override bool Available
        {
            get
            {
                return int.TryParse(this.stringValue, out this.result);
            }
        }
    }
}
