using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Mapping
{
    internal abstract class BaseMatch<T> : IMatchValue
    {
        public BaseMatch(object input)
        {
            this.input = input;
            this.stringValue = string.Format("{0}", input);
        }

        public abstract bool Available { get; }

        public object GetValue()
        {
            object obj;
            if (this.Available)
            {
                obj = this.result;
            }
            else
            {
                obj = null;
            }
            return obj;
        }

        protected object input;
        protected string stringValue;
        protected T result;
    }
}
