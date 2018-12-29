using AutoMapper;
using HxBlogs.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Framework.Mappers
{
    public class MyMapperProfile : Profile
    {
        public MyMapperProfile()
        {
            List<Assembly> allAssembly = TypeHelper.GetAllAssembly();
            if (allAssembly != null && allAssembly.Count > 0)
            {
                Type baseType = typeof(IAutoMapper<>);
                foreach (Assembly ass in allAssembly)
                {
                    try
                    {
                        var types = ass.GetExportedTypes().Where(t => !t.IsInterface && !t.IsAbstract);
                        foreach (Type sourceType in types)
                        {
                            var genericTypes = sourceType.GetGenericInterfaces(baseType);
                            if (genericTypes != null && genericTypes.Count() > 0)
                            {
                                foreach (Type t in genericTypes)
                                {
                                    Type destType =  t.GetGenericElementType();
                                    if (destType.IsClass)
                                    {
                                        CreateMap(sourceType, destType);
                                        CreateMap(destType, sourceType);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception){}
                }
            }
        }
        
    }
}
