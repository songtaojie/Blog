using Hx.Common.Cache;
using Hx.Common.Helper;
using Hx.Framework.Mappers;
using HxBlogs.IBLL;
using HxBlogs.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    public class Config
    {
        private static HxBlogs.Model.SystemConfig _systemConfig;
        /// <summary>
        /// 系统配置
        /// </summary>
        public static HxBlogs.Model.SystemConfig SystemConfig
        {
            get
            {
                if (_systemConfig == null)
                {
                    object json = MemcachedHelper.Get(ConstInfo.systemConfig);
                    if (json != null)
                    {
                        _systemConfig = JsonConvert.DeserializeObject<HxBlogs.Model.SystemConfig>(json.ToString());
                    }
                    else
                    {
                        ISystemConfigService configService =  Hx.Framework.ContainerManager.Resolve<ISystemConfigService>();
                        _systemConfig = configService.GetEntitiesNoTrack(c => true).First();
                    }
                }
                return _systemConfig;
            }
            set
            {
                _systemConfig = value;
                string json = JsonConvert.SerializeObject(value);
                MemcachedHelper.Set(ConstInfo.systemConfig, json);
            }
        }
    }
}