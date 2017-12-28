using DESSAU.ControlGestion.Web.Models.HomeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("IndexEWP");
            IndexViewModel model = new IndexViewModel(UsuarioActual.IdUsuario);
            model.Usuario = UsuarioActual;
            return View(model);
        }

        public ActionResult IndexEWP()
        {
            IndexViewModel model = new IndexViewModel(UsuarioActual.IdUsuario);
            model.Usuario = UsuarioActual;
            return View(model);
        }
    }
}