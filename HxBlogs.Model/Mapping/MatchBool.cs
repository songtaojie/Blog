using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Mapping
{
    internal class MatchBool : BaseMatch<int>
    {
        public MatchBool(object input) : base(input)
        {
        }

        public override bool Available
        {
            get
            {
                bool success = bool.TryParse(this.stringValue, out bool temp);
                this.result = temp ? 1 : 0;
                return success;
            }
        }
    }
}
