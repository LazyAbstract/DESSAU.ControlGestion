using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.NominaModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class NominaController : BaseController
    {
        // GET: Nomina
        public ActionResult Index()
        {
            return RedirectToAction("VerNomina");
        }

        [HttpGet]
        public ActionResult VerNomina(int? pagina, VerNominaFormModel Form)
        {
            VerNominaViewModel model = new VerNominaViewModel();
            IQueryable<UsuarioCategoriaProyecto> Nominas = db.UsuarioCategoriaProyectos
                    .OrderBy(x => x.Categoria.Nombre).ThenBy(x => x.Usuario.ApellidoPaterno);
            if (Form.IdProyecto.HasValue) Nominas = db.UsuarioCategoriaProyectos
                    .Where(x => x.IdProyecto == Form.IdProyecto);
            model.Nominas = Nominas.ToPagedList(pagina ?? 1, 10);
            return View(model);
        }
    }
}