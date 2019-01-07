using AutoMapper;
using HxBlogs.Framework.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Framework
{
    public class MapperManager
    {
        public void Execute<TSource, TDestination>(List<TSource> sourceList, List<TDestination> destList)
            where TSource : class, new()
            where TDestination : class, new()
        {
            if (sourceList == null || destList == null) return;
            if (sourceList.Count != destList.Count) throw new Exception("请确定源数据和目标数据对应!");
            for (int i = 0; i < sourceList.Count; i++)
            {
                TSource source = sourceList[i];
                TDestination dest = destList[i];
                Mapper.Initialize(cfg => cfg.CreateMap(typeof(TSource), typeof(TDestination)));
            }
        }
        /// <summary>
        /// 启动项目时进行初始化配置
        /// </summary>
        public void Start()
        {
            Mapper.Reset();
            Mapper.Initialize(c => c.AddProfile<MyMapperProfile>());
        }
        /// <summary>
        /// 数据映射
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TD Map<TD>(object source)
        {
            return Mapper.Map<TD>(source);
        }
    }
}
