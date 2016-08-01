using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Configuration;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DESSAU.ControlGestion.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(Rut), new RutModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new CurrentCultureDateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime), new CurrentCultureDateTimeBinder());
            FluentValidationModelValidatorProvider.Configure();


            IList<IConfigurable> configurations = new List<IConfigurable>();
            configurations.Add(new MapperConfiguration());
            //configurations.Add(new InitSimpleMembership());
            foreach (var configuration in configurations)
            {
                configuration.Configure();
            }
        }
    }
}
