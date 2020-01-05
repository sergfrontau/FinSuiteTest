using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace FinTest
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                var mapperConfigurations = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(
                    type => type.BaseType != null &&
                    type.BaseType == typeof(AutoMapper.Profile));

                foreach (object mapping in mapperConfigurations.Select(Activator.CreateInstance))
                {
                    cfg.AddProfile((dynamic)mapping);
                }
            });


            return config;
        }


    }
}