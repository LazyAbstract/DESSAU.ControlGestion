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
    [Authorize]
    public class TimeSheetController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerTimeSheet(VerTimeSheetFormModel FORM, int? IdTipoTimeSheet)
        {
            //  pequeño haack pa hacer que valla donde quiero
            if (IdTipoTimeSheet.HasValue) FORM.IdTipoTimeSheet = IdTipoTimeSheet;
            if (FORM.IdTipoTimeSheet == TipoTimeSheet.Planificacion) FORM.ClaseBootstrap = "primary";
            else FORM.ClaseBootstrap = "info";

            VerTimeSheetViewModel model = new VerTimeSheetViewModel(FORM, db);
            IQueryable<UsuarioCategoriaProyecto> usuarioCategoriaProyectos = db.UsuarioCategoriaProyectos.Where(x =>
                x.IdUsuario == UsuarioActual.IdUsuario &&
                x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente);
            IQueryable<TimeSheet> timeSheets = usuarioCategoriaProyectos.SelectMany(x => x.TimeSheets);
            if (ModelState.IsValid)
            {
                if (FORM.IdCategoria.HasValue)
                {
                    usuarioCategoriaProyectos = usuarioCategoriaProyectos.Where(x => x.IdCategoria == FORM.IdCategoria);
                    timeSheets = usuarioCategoriaProyectos.SelectMany(x => x.TimeSheets);
                }
                if (FORM.Fecha.HasValue)
                {
                    timeSheets = timeSheets.Where(x => model.getFechaDesdeHasta.Contains(x.Fecha));
                }
            }
            model.UsuarioCategoriaProyectos = usuarioCategoriaProyectos;
            if (timeSheets.Any())
            {
                model.TimeSheetFORM = timeSheets.GroupBy(x => x.IdUsuarioCategoriaProyecto)
                    .Select(x => new TimeSheetCategoriaProyectoDTO()
                    {
                        IdUsuarioCategoriaProyecto = x.Key,
                        TimeSheetDTOs = Mapper.Map<IEnumerable<TimeSheet>, IEnumerable<TimeSheetDTO>>(x)
                    });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult VerTimeSheet(VerTimeSheetFormModel FORM, IEnumerable<TimeSheetCategoriaProyectoDTO> TimeSheetFORM)
        {
            // No se porque no me pasa la fecha como hidden :(
            FORM.Fecha = FORM.Fecha.GetValueOrDefault(TimeSheetFORM.First().TimeSheetDTOs.Min(x=>x.Fecha.Value));
            VerTimeSheetViewModel model = new VerTimeSheetViewModel(FORM, db);
            model.TimeSheetFORM = TimeSheetFORM;
            //model.FORM.ClaseBootstrap = "info";
            IQueryable<UsuarioCategoriaProyecto> usuarioCategoriaProyectos = db.UsuarioCategoriaProyectos.Where(x =>
                x.IdUsuario == UsuarioActual.IdUsuario &&
                x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente);
            if (FORM.IdCategoria.HasValue)
            {
                usuarioCategoriaProyectos = usuarioCategoriaProyectos.Where(x => x.IdCategoria == FORM.IdCategoria);
            }
            if (ModelState.IsValid)
            {
                List<TimeSheetCategoriaProyectoDTO> timeSheetCategoriaProyectoDTOs = new List<TimeSheetCategoriaProyectoDTO>();
                foreach (var timeSheetFORMItem in TimeSheetFORM)
                {
                    List<TimeSheet> resultTimeSheet = new List<TimeSheet>();
                    foreach (var timeSheetDTO in timeSheetFORMItem.TimeSheetDTOs)
                    {
                        if (timeSheetDTO.Horas.HasValue && (timeSheetDTO.Horas.Value != 0 || timeSheetDTO.IdTimeSheet.HasValue))
                        {
                            TimeSheet timeSheet = db.TimeSheets.SingleOrDefault(x => x.IdTimeSheet == timeSheetDTO.IdTimeSheet);
                            if (timeSheet == null)
                            {
                                timeSheet = new TimeSheet()
                                {
                                    Fecha = timeSheetDTO.Fecha.Value,
                                    IdActividad = timeSheetDTO.IdActividad.GetValueOrDefault(0),
                                    IdUsuarioCategoriaProyecto = timeSheetFORMItem.IdUsuarioCategoriaProyecto.GetValueOrDefault(0)
                                };
                            }
                            switch (FORM.IdTipoTimeSheet)
                            {
                                case TipoTimeSheet.Planificacion:
                                    timeSheet.HorasPlanificadas = timeSheetDTO.Horas.GetValueOrDefault(0);
                                    break;
                                case TipoTimeSheet.Reportado:
                                    timeSheet.HorasReportadas = timeSheetDTO.Horas.GetValueOrDefault(0);
                                    break;
                            }
                            if (!timeSheetDTO.IdTimeSheet.HasValue)
                            {
                                db.TimeSheets.InsertOnSubmit(timeSheet);
                            }
                            db.SubmitChanges();
                            resultTimeSheet.Add(timeSheet);
                        }

                    }
                    timeSheetCategoriaProyectoDTOs.Add(new TimeSheetCategoriaProyectoDTO()
                    {
                        IdUsuarioCategoriaProyecto = timeSheetFORMItem.IdUsuarioCategoriaProyecto,
                        TimeSheetDTOs = Mapper.Map<IEnumerable<TimeSheet>, IEnumerable<TimeSheetDTO>>(resultTimeSheet)
                    });

                }
                model.TimeSheetFORM = timeSheetCategoriaProyectoDTOs;
            }
            model.UsuarioCategoriaProyectos = usuarioCategoriaProyectos;
            return View(model);
        }

    }
}