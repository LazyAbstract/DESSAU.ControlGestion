using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.TimeSheetEWPModels;
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
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult VerTimeSheet(VerTimeSheetFormModel FORM, int? IdTipoTimeSheet)
        {
            //  pequeño haack pa hacer que valla donde quiero
            if (IdTipoTimeSheet.HasValue) FORM.IdTipoTimeSheet = IdTipoTimeSheet;
            if (FORM.IdTipoTimeSheet == TipoTimeSheet.Planificacion) FORM.ClaseBootstrap = "primary";
            else FORM.ClaseBootstrap = "info";
            int idUsuario;         

            VerTimeSheetViewModel model = new VerTimeSheetViewModel(FORM, db);
            if (FORM.IdUsuario.HasValue)
            {
                idUsuario = FORM.IdUsuario.Value;
                model.EsReporte = true;
            }
            else
            {
                idUsuario = UsuarioActual.IdUsuario;
            }
            IQueryable<UsuarioCategoriaProyecto> usuarioCategoriaProyectos = db.UsuarioCategoriaProyectos.Where(x =>
                x.IdUsuario == idUsuario &&
                x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente);

            model.calc = new CalculoHoraMensual(idUsuario, FORM.IdTipoTimeSheet.GetValueOrDefault(1), FORM.Fecha.GetValueOrDefault(DateTime.Now));
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
        [Authorize(Roles = "Admin")]
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
            model.calc = new CalculoHoraMensual(UsuarioActual.IdUsuario, FORM.IdTipoTimeSheet.Value, FORM.Fecha.GetValueOrDefault(DateTime.Now));
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
                                Mensaje = "Se han guardado los cambios exitosamente.";
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

        //nuevo desarrollo rimesheet EWP
        [HttpGet]
        public ActionResult CrearEditarTimeSheetEWP(CrearEditarTimeSheetEWPFormModel Form, DateTime? fecha)
        {
            //  esto está muy al lote...
            if (db.DiaEspecials.Any(x => x.Fecha == fecha))
            {
                TempData["Mensaje"] = "El día seleccionado está marcado como feriado.";
                return RedirectToAction("CrearEditarTimeSheetEWP", new { fecha = fecha.Value.AddDays(1).ToShortDateString() });
            }

            int idUsuario;
            if (Form.IdUsuario.HasValue) idUsuario = Form.IdUsuario.Value;
            else idUsuario = UsuarioActual.IdUsuario;
            CrearEditarTimeSheetEWPViewModel model = new CrearEditarTimeSheetEWPViewModel();            
            model.Form.Fecha = Form.Fecha.GetValueOrDefault(DateTime.Today);
            if (fecha.HasValue) { model.Form.Fecha = fecha; }
            model.Form.validar = true;
            IQueryable<UsuarioCategoriaProyecto> usuarioCategoriaProyectos = db.UsuarioCategoriaProyectos.Where(x =>
               x.IdUsuario == idUsuario &&
               x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                   TipoEstadoUsuarioCategoriaProyecto.NoVigente);
            model.UsuarioCategoriaProyectos = usuarioCategoriaProyectos;
            if (!usuarioCategoriaProyectos.Any()) return View("SinAsignacionODS");
            model.Form.IdUsuarioCategoriaProyecto = usuarioCategoriaProyectos.First().IdUsuarioCategoriaProyecto;
            if(db.Turnos.Any(x => x.IdUsuarioCategoriaProyecto == model.Form.IdUsuarioCategoriaProyecto))
            {
                model.turnoHelper.HorasLaborablesSemana = 12;
                model.turnoHelper.HorasLaborablesViernes = 12;
                model.turnoHelper.TrabajaFinesDeSemana = true;
            }

            model.Form.HorasSemana = model.turnoHelper.HorasLaborablesSemana;
            model.Form.HorasViernes = model.turnoHelper.HorasLaborablesViernes;

            model.Actividades = usuarioCategoriaProyectos.First().Categoria.CategoriaActividads.Select(x => x.Actividad);
            model.TimeSheetsEWP = db.TimeSheetEWPs.Where(x => x.Fecha == model.Form.Fecha
              && x.IdUsuarioCategoriaProyecto == model.Form.IdUsuarioCategoriaProyecto)
              .OrderByDescending(x => x.Fecha).ThenBy(x => x.SubEWP.Codigo);

            foreach(var actividad in model.Actividades.OrderBy(x => x.TipoActividad.Orden).ThenBy(x => x.Nombre))
            {
                int horas = 0;
                IEnumerable<TimeSheet> timeSheets = db.TimeSheets
                    .Where(x => x.Fecha == model.Form.Fecha
                        && x.IdUsuarioCategoriaProyecto == model.Form.IdUsuarioCategoriaProyecto
                        && x.IdActividad == actividad.IdActividad);
                if(timeSheets.Any()) horas = timeSheets
                    .Sum(x => x.HorasReportadas.GetValueOrDefault(0));
                model.Form.DTOvalues.Add(horas);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CrearEditarTimeSheetEWP(CrearEditarTimeSheetEWPFormModel Form)
        {
            int idUsuario = UsuarioActual.IdUsuario;
            IQueryable<UsuarioCategoriaProyecto> usuarioCategoriaProyectos = db.UsuarioCategoriaProyectos.Where(x =>
                    x.IdUsuario == idUsuario &&
                    x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente);
            if (ModelState.IsValid)
            {
                if (Form.HorasReportadasEWP > 0)
                {
                    TimeSheetEWP tsEWP = db.TimeSheetEWPs.SingleOrDefault(x => 
                        x.IdUsuarioCategoriaProyecto == Form.IdUsuarioCategoriaProyecto
                            && x.IdActividad == 130 //Form.HorasReportadasEWP.IdActividad 
                            && x.Fecha == Form.Fecha.Value
                            && x.IdSubEWP == Form.IdSubEWP
                            && x.IdTipoDocumento == Form.IdTipoDocumento
                            && x.IdRevision == Form.IdRevision
                            && x.IdNumeroDocumento == Form.IdNumeroDocumento);
                    if(tsEWP != null)
                    {
                        tsEWP.HorasReportadas = Form.HorasReportadasEWP;
                    }
                    else
                    {
                        TimeSheetEWP timeSheet = new TimeSheetEWP()
                        {
                            IdActividad = 130, // Form.IdActividad.GetValueOrDefault(),  ACTIVIDAD EWP
                            IdUsuarioCategoriaProyecto = Form.IdUsuarioCategoriaProyecto.GetValueOrDefault(),
                            IdNumeroDocumento = Form.IdNumeroDocumento.GetValueOrDefault(),
                            IdRevision = Form.IdRevision.GetValueOrDefault(),
                            IdSubEWP = Form.IdSubEWP.GetValueOrDefault(),
                            IdTipoDocumento = Form.IdTipoDocumento.GetValueOrDefault(),
                            HorasReportadas = Form.HorasReportadasEWP,
                            Fecha = Form.Fecha.Value
                        };

                        db.TimeSheetEWPs.InsertOnSubmit(timeSheet);
                    }
                                        
                    db.SubmitChanges();
                    TempData["Mensaje"] = "Se guardaron los cambios exitosamente.";
                }

                if(Form.DTO.Any())
                {
                    foreach (var dto in Form.DTO)
                    {
                        TimeSheet _ts = db.TimeSheets.SingleOrDefault(x => 
                            x.IdUsuarioCategoriaProyecto == Form.IdUsuarioCategoriaProyecto 
                                && x.IdActividad == dto.IdActividad 
                                && x.Fecha == Form.Fecha.Value);
                        if (_ts != null)
                        {
                            _ts.HorasReportadas = dto.HorasReportadas;
                        }
                        else if(dto.HorasReportadas > 0)
                        {
                            TimeSheet ts = new TimeSheet()
                            {
                                IdUsuarioCategoriaProyecto = Form.IdUsuarioCategoriaProyecto.Value,
                                IdActividad = dto.IdActividad,
                                HorasReportadas = dto.HorasReportadas,
                                Fecha = Form.Fecha.Value
                            };
                            db.TimeSheets.InsertOnSubmit(ts);
                            TempData["Mensaje"] = "Se guardaron los cambios exitosamente.";
                        }                        
                    }
                    db.SubmitChanges();
                }
                var fecha = Form.Fecha;
                Form = new CrearEditarTimeSheetEWPFormModel();
                Form.Fecha = fecha;
            }
            CrearEditarTimeSheetEWPViewModel model = new CrearEditarTimeSheetEWPViewModel(Form);
            model.UsuarioCategoriaProyectos = usuarioCategoriaProyectos;
            model.Form.IdUsuarioCategoriaProyecto = usuarioCategoriaProyectos.First().IdUsuarioCategoriaProyecto;
            model.Actividades = usuarioCategoriaProyectos.First().Categoria.CategoriaActividads.Select(x => x.Actividad);
            
            foreach (var actividad in model.Actividades.OrderBy(x => x.TipoActividad.Orden).ThenBy(x => x.Nombre))
            {
                int horas = 0;
                IEnumerable<TimeSheet> timeSheets = db.TimeSheets
                    .Where(x => x.Fecha == model.Form.Fecha
                        && x.IdUsuarioCategoriaProyecto == model.Form.IdUsuarioCategoriaProyecto
                        && x.IdActividad == actividad.IdActividad);
                if (timeSheets.Any()) horas = timeSheets
                     .Sum(x => x.HorasReportadas.GetValueOrDefault(0));
                model.Form.DTOvalues.Add(horas);
            }

            model.Form.Fecha = Form.Fecha.GetValueOrDefault(DateTime.Today);
            model.TimeSheetsEWP = db.TimeSheetEWPs.Where(x => x.Fecha == model.Form.Fecha
               && x.IdUsuarioCategoriaProyecto == model.Form.IdUsuarioCategoriaProyecto)
               .OrderByDescending(x => x.Fecha).ThenBy(x => x.SubEWP.Codigo);
            return View(model);
        }

        public PartialViewResult VerTimeSheetEWP(int IdUsuarioCategoriaProyecto, DateTime Fecha)
        {
            VerTimeSheetEWPViewModel model = new VerTimeSheetEWPViewModel();
            model.TimeSheets = db.TimeSheetEWPs.Where(x => x.Fecha == Fecha
                && x.IdUsuarioCategoriaProyecto == IdUsuarioCategoriaProyecto)
                .OrderByDescending(x => x.Fecha).ThenBy(x => x.SubEWP.Codigo);
            return PartialView(model);
        }

        public ActionResult getSubEWPFromEWP(FormCollection formCollection)
        {
            var formValue = formCollection.GetValues(0)[0];
            int IdEWP = Int16.Parse(formValue);
            SelectList result = null;
            result = new SelectList(db.SubEWPs.Where(x => x.IdEWP == IdEWP), "IdSubEWP", "Codigo");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNumeroDocumentoFromEWP(FormCollection formCollection)
        {
            var formValue = formCollection.GetValues(0)[0];
            int IdEWP = Int16.Parse(formValue);
            SelectList result = null;
            if(db.NumeroDocumentos.Where(x => x.IdEWP == IdEWP).Any())
            {
                result = new SelectList(db.NumeroDocumentos.Where(x => x.IdEWP == IdEWP && x.Vigente), "IdNumeroDocumento", "Codigo");
            }
            else
            {
                result = new SelectList(db.NumeroDocumentos.Where(x => x.Vigente), "IdNumeroDocumento", "Codigo");
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarEWP(int IdTimeSheetEWP, DateTime fecha)
        {
            TimeSheetEWP del = db.TimeSheetEWPs.Single(x => x.IdTimeSheetEWP == IdTimeSheetEWP);
            db.TimeSheetEWPs.DeleteOnSubmit(del);
            db.SubmitChanges();
            TempData["Mensaje"] = "El registro fue eliminado exitosamente.";
            return RedirectToAction("CrearEditarTimeSheetEWP", new { fecha = fecha.ToShortDateString() });
        }
    }
}