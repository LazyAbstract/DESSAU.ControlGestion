using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.TimeSheetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class TimeSheetController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerTimeSheet(VerTimeSheetFormModel FORM)
        {
            VerTimeSheetViewModel model = new VerTimeSheetViewModel(FORM);
            IQueryable<UsuarioCategoriaProyecto> usuarioCategoriaProyectos = db.UsuarioCategoriaProyectos.Where(x =>
                x.IdUsuario == _CurrentUsuario.IdUsuario &&
                x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto != 
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente);
            IQueryable<TimeSheet> timeSheets = db.TimeSheets.Where(x => x.IdUsuario == _CurrentUsuario.IdUsuario);
            if (ModelState.IsValid)
            {
                if (FORM.Fecha.HasValue)
                {
                    timeSheets = timeSheets.Where(x => model.FechaDesdeHasta.Contains(x.Fecha));
                }
                if (FORM.IdCategoria.HasValue)
                {
                    timeSheets = timeSheets.Where(x => x.Actividad.CategoriaActividads.Any(y => y.IdCategoria == FORM.IdCategoria.Value));
                    usuarioCategoriaProyectos = usuarioCategoriaProyectos.Where(x=>x.IdCategoria == FORM.IdCategoria);
                }                
            }
            model.UsuarioCategoriaProyectos = usuarioCategoriaProyectos;
            model.TimeSheets = timeSheets;
            return View(model);
        }
    }
}