using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    public class JobInfo: BaseEntity
    {
        /// <summary>
        /// 职位
        /// </summary>
        public string Position
        {
            get;set;
        }
        /// <summary>
        /// 行业
        /// </summary>
        public string Industry
        {
            get;set;
        }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string WorkUnit
        {
            get;set;
        }
        /// <summary>
        /// 工作年限
        /// </summary>
        public int WorkYear
        {
            get;set;
        }

        /// <summary>
        /// 目前状态
        /// </summary>
        public string Status
        {
            get;set;
        }
        /// <summary>
        /// 熟悉的技术
        /// </summary>
        public string Skills
        {
            get;set;
        }
        /// <summary>
        /// 擅长的领域
        /// </summary>
        public string GoodAreas
        {
            get;set;
        }
    }
}
