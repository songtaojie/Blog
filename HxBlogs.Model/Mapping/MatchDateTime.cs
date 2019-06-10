using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Mapping
{
    internal class MatchDateTime : BaseMatch<DateTime>
    {
        public MatchDateTime(object input) : base(input)
        {
        }
        public override bool Available
        {
            get
            {
                return DateTime.TryParse(this.stringValue, out this.result);
            }
        }
    }
}
