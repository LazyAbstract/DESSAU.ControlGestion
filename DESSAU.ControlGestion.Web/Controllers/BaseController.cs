using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class BaseController : Controller
    {
        public DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public Usuario _CurrentUsuario
        {
            get{
                try{
                    return (Usuario)Session["CurrentUsuario"];
                }
                catch(Exception ex)
                {
                    return new Usuario();
                }
            }
        }

        public string Mensaje
        {
            set
            {
                TempData["Mensaje"] = value;
            }
        }
    }
}