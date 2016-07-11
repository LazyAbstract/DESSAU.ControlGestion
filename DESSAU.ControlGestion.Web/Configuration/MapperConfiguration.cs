using AutoMapper;
using DESSAU.ControlGestion.Web.MapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Configuration
{
    public class MapperConfiguration : IConfigurable
    {
        public void Configure()
        {
            Mapper.Initialize(x => GetConfiguration(Mapper.Configuration));
        }

        private void GetConfiguration(IConfiguration configuration)
        {
            var profiles = typeof(DefaultProfileMapper).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
            foreach (var profile in profiles)
            {
                configuration.AddProfile(Activator.CreateInstance(profile) as Profile);
            }
        }
    }
}