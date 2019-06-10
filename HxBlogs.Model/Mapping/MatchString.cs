using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Mapping
{
    internal class MatchString : BaseMatch<string>
    {
        public MatchString(object input) : base(input)
        {
        }

        public override bool Available
        {
            get
            {
                this.result = this.stringValue;
                return !string.IsNullOrEmpty(this.stringValue);
            }
        }
    }
}
