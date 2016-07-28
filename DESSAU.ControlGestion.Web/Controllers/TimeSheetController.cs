using AutoMapper;
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
            VerTimeSheetViewModel model = new VerTimeSheetViewModel(FORM, db);
            IQueryable<UsuarioCategoriaProyecto> usuarioCategoriaProyectos = db.UsuarioCategoriaProyectos.Where(x =>
                x.Usuario.Correo == User.Identity.Name &&
                x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente);
            IQueryable<TimeSheet> timeSheets = db.TimeSheets.Where(x => x.UsuarioCategoriaProyecto.IdUsuario == _CurrentUsuario.IdUsuario);
            if (ModelState.IsValid)
            {
                if (FORM.Fecha.HasValue)
                {
                    timeSheets = timeSheets.Where(x => model.getFechaDesdeHasta.Contains(x.Fecha));
                }
                if (FORM.IdCategoria.HasValue)
                {
                    timeSheets = timeSheets.Where(x => x.Actividad.CategoriaActividads.Any(y => y.IdCategoria == FORM.IdCategoria.Value));
                    usuarioCategoriaProyectos = usuarioCategoriaProyectos.Where(x => x.IdCategoria == FORM.IdCategoria);
                }
            }
            model.UsuarioCategoriaProyectos = usuarioCategoriaProyectos;
            if(timeSheets.Any())
            {
                model.TimeSheetFORM = Mapper.Map<IEnumerable<TimeSheet>, IEnumerable<TimeSheetDTO>>(timeSheets);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult VerTimeSheet(VerTimeSheetFormModel FORM, IEnumerable<TimeSheetDTO> TimeSheetFORM)
        {
            VerTimeSheetViewModel model = new VerTimeSheetViewModel(FORM, db);
            model.TimeSheetFORM = TimeSheetFORM;
            IQueryable<UsuarioCategoriaProyecto> usuarioCategoriaProyectos = db.UsuarioCategoriaProyectos.Where(x =>
                x.IdUsuario == _CurrentUsuario.IdUsuario &&
                x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente);
            if (ModelState.IsValid)
            {
                if (FORM.IdCategoria.HasValue)
                {
                    usuarioCategoriaProyectos = usuarioCategoriaProyectos.Where(x => x.IdCategoria == FORM.IdCategoria);
                }
            }
            model.UsuarioCategoriaProyectos = usuarioCategoriaProyectos;
            return View(model);
        }

    }
}