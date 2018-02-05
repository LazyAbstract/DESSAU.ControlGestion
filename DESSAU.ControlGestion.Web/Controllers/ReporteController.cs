using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Helpers;
using DESSAU.ControlGestion.Web.Models.ReporteModels;
using DESSAU.ControlGestion.Web.Models.ReporteModels.ReporteEWPModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DESSAU.ControlGestion.Web.Models.ReporteModels.DashboardViewModel;

namespace DESSAU.ControlGestion.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReporteController : BaseController
    {
        // GET: Reporte
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard(int? pagina, DashboardFormModel Form)
        {            
            int Mes;
            int Ano;
            DashboardViewModel model = new DashboardViewModel(Form);
            if (String.IsNullOrWhiteSpace(Form.Periodo))
            {
                LectorMonthPicker lector = new LectorMonthPicker();
                var fecha = DateTime.Now.AddMonths(-1);
                Mes = fecha.Month;
                Ano = fecha.Year;
                model.Form.Periodo = lector.GetMonthNameFromInt(fecha.Month)
                        + " " + fecha.Year.ToString();                
            }
            else
            {
                LectorMonthPicker lector = new LectorMonthPicker(Form.Periodo);
                Mes = lector.GetMes;
                Ano = lector.GetAnno;
            }
            model.Form.Fecha = new DateTime(Ano, Mes, 1);
            model.calc = new CalculoHoraMensual(UsuarioActual.IdUsuario, TipoTimeSheet.Planificacion, new DateTime(Ano, Mes, 1));

            var indice = db.fn_IndiceDesviacion(Mes, Ano, model.Form.IdProyecto, null).FirstOrDefault();
            double desviacion = 0;
            if (indice != null)
            {
                desviacion = indice.IndiceDesviacion.GetValueOrDefault(0);
            }

            model.PorcentajeDesviacion = desviacion;
            if (model.PorcentajeDesviacion > 0.05) model.claseBootstrap = "warning";
            if (model.PorcentajeDesviacion > 0.1) model.claseBootstrap = "danger";

            DateTime FechaConsulta; 
            if(DateTime.Now.Month <= Mes && DateTime.Now.Year <= Ano)
            {
                FechaConsulta = new DateTime(Ano, Mes, DateTime.Now.Day);
            }
            else
            {
                var buffer = new DateTime(Ano, Mes, 1);
                buffer = buffer.AddMonths(1);
                FechaConsulta = buffer.AddDays(-1);
            }

            IQueryable<fn_ReportePorUsuarioResult> Nominas = db.fn_ReportePorUsuario(Form.IdProyecto, FechaConsulta);
            model.Nominas = Nominas.Where(x => x.CantidadPendientesDeclaracion > 0 || x.CantidadPendientesPlanificacion > 0);
            return View(model);
        }

        public ActionResult ReporteDedicacionActividad(ReporteDedicacionActividadFormModel Form, int? IdUsuarioCategoriaProyecto)
        {
            ReporteDedicacionActividadViewModel model = new ReporteDedicacionActividadViewModel();
            if (IdUsuarioCategoriaProyecto.HasValue)
            {
                var upc = db.UsuarioCategoriaProyectos.SingleOrDefault(x => x.IdUsuarioCategoriaProyecto == IdUsuarioCategoriaProyecto
                    && x.EstadoUsuarioCategoriaProyecto.IdEstadoUsuarioCategoriaProyecto !=  TipoEstadoUsuarioCategoriaProyecto.NoVigente);
                if (upc != null)
                {
                    model.Form.IdUsuario = upc.IdUsuario;
                    model.Form.IdProyecto = upc.IdProyecto;
                }                
            }

            int Mes;
            int Ano;
            if (String.IsNullOrWhiteSpace(Form.Periodo))
            {
                LectorMonthPicker lector = new LectorMonthPicker();
                var fecha = DateTime.Now.AddMonths(-1);
                Mes = fecha.Month;
                Ano = fecha.Year;
                model.Form.Periodo = lector.GetMonthNameFromInt(fecha.Month)
                       + " " + fecha.Year.ToString();
            }
            else
            {
                LectorMonthPicker lector = new LectorMonthPicker(Form.Periodo);
                Mes = lector.GetMes;
                Ano = lector.GetAnno;
                model.Form.Periodo = lector.GetMonthNameFromInt(Mes)
                       + " " + Ano.ToString();
            }

            return View(model);
        }

        public JsonResult GetDedicacionActividadData(string Periodo, int? IdProyecto, int? IdUsuario)
        {
            int Mes;
            int Ano;
            if(String.IsNullOrWhiteSpace(Periodo))
            {
                Mes = DateTime.Now.Month;
                Ano = DateTime.Now.Year;
            }
            else
            {
                LectorMonthPicker lector = new LectorMonthPicker(Periodo);
                Mes = lector.GetMes;
                Ano = lector.GetAnno;
            }

            var Data =
               new dynamic[]
               {
                    new {
                        Id = "Data1",
                        Columns = new [] {
                            new { Title = "Actividad", type = "string" },
                            new { Title = "Dedicación Planificada", type = "number" },
                        },
                        Rows = db.fn_ReporteDedicacionActividad2(Mes, Ano, IdUsuario, IdProyecto)
                            .OrderBy(x => x.Actividad)
                            .Select(x => new dynamic [] {
                                x.Actividad,
                                x.PorcentajeDedicacionPlanificacion.GetValueOrDefault(0)
                        }).ToArray()
                    },
                    new {
                        Id = "Data2",
                        Columns = new[] {
                                new { Title = "Actividad", type = "string" },
                                new { Title = "Dedicación Declarada", type = "number" },
                            },
                        Rows = db.fn_ReporteDedicacionActividad2(Mes, Ano, IdUsuario, IdProyecto)
                            .OrderBy(x => x.Actividad)
                            .Select(x => new dynamic [] {
                                x.Actividad,
                                x.PorcentajeDedicacionDeclaracion.GetValueOrDefault(0),
                            }).ToArray()
                    },
                    new {
                        Id = "Data3",
                        Columns = new[] {
                                new { Title = "Actividad", type = "string" },
                                new { Title = "Horas Planificación", type = "number" },
                                new { Title = "Horas Declaración", type = "number" },
                            },
                        Rows = db.fn_HorasPlanificacionDeclaracion(Mes, Ano, IdUsuario, IdProyecto)
                            .OrderByDescending(x => x.HorasPlanificadas)
                            .Select(x => new dynamic [] {
                                x.Actividad,
                                x.HorasPlanificadas.GetValueOrDefault(0),
                                x.HorasReportadas.GetValueOrDefault(0),
                            }).ToArray()
                    },
                    new {
                        Id = "Data4",
                        Columns = new[] {
                                new { Title = "Desviación", type = "number" },
                            },
                        Rows = db.fn_IndiceDesviacion(Mes, Ano, IdProyecto, IdUsuario)
                            .Select(x => new dynamic [] {
                                x.IndiceDesviacion.GetValueOrDefault(0)*100
                            }).ToArray()
                    },
                    new {
                        Id = "Data5",
                        Columns = new[] {
                                new { Title = "Desempeño", type = "number" },
                            },
                        Rows = db.fn_PromedioEvaluacion(Mes, Ano, IdUsuario, IdProyecto)
                            .Select(x => new dynamic [] {
                                x.Promedio*20
                            }).ToArray()
                    }
               };

            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNomina(FormCollection formCollection)
        {
            var formValue = formCollection.GetValues(0)[0];
            int IdProyecto = Int16.Parse(formValue);
            IEnumerable<SelectListItem> items = db.UsuarioCategoriaProyectos
                .Where(x => x.IdProyecto == IdProyecto && x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto != 99)
                .Select(x => x.Usuario)
                .OrderBy(x => x.ApellidoPaterno)
                .Select(x => new SelectListItem()
                {
                    Text = x.ApellidoNombre,
                    Value = x.IdUsuario.ToString()
                });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //  reporte EWP
        [HttpGet]
        public ActionResult VerReporteEWP(VerReporteEWPFormModel Form)
        {
            VerReporteEWPViewModel model = new VerReporteEWPViewModel(Form);
            model.Resultados = db.fn_ReportePorEWP(Form.FechaDesde, Form.FechaHasta, Form.IdUsuario)
                .Where(x => x.HorasReportadas > 0)
                .OrderBy(x => x.Fecha)
                .ThenBy(x => x.TipoActividad)
                .ThenBy(x => x.Actividad);
            model.Total += model.Resultados.Sum(x => x.HorasReportadas);
            if (Form.IdUsuario.HasValue)
            {
                model.Profesional = db.Usuarios.Single(x => x.IdUsuario == Form.IdUsuario).ApellidoNombre;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult VerReporteNumeroDessau(VerReporteEWPFormModel Form)
        {
            VerReporteEWPViewModel model = new VerReporteEWPViewModel(Form);
            model.Resultados = db.fn_ReportePorEWP(Form.FechaDesde, Form.FechaHasta, Form.IdUsuario)
                .Where(x => x.HorasReportadas > 0 && x.CodigoEWP != "No Aplica")
                .OrderBy(x => x.Fecha)
                .ThenBy(x => x.Actividad);
            model.Total += model.Resultados.Sum(x => x.HorasReportadas);
            if (Form.IdUsuario.HasValue)
            {
                model.Profesional = db.Usuarios.Single(x => x.IdUsuario == Form.IdUsuario).ApellidoNombre;
            }
            return View(model);
        }

        public ActionResult ExportarEWPExcel(DateTime? FechaDesde)
        {
            VerReporteEWPViewModel model = new VerReporteEWPViewModel();
            if (FechaDesde.HasValue) model.Form.FechaDesde = FechaDesde.Value;          
            
            IEnumerable<fn_ReportePorEWPResult> Exportar = 
                db.fn_ReportePorEWP(new DateTime(2017, 12, 1), DateTime.Today.AddDays(1), model.Form.IdUsuario)
                .Where(x => x.HorasReportadas > 0)
                .OrderBy(x => x.Fecha)
                .ThenBy(x => x.TipoActividad)
                .ThenBy(x => x.Actividad);
            IEnumerable<ExportaEWPViewModel> NominaExportar =
                Mapper.Map<IEnumerable<fn_ReportePorEWPResult>, IEnumerable<ExportaEWPViewModel>>(Exportar);
            using (var Lista = new MemoryStream())
            {
                ExportadorExcel.GeneraExcel<ExportaEWPViewModel>(NominaExportar, "Lista", Lista, true);
                return File(Lista.ToArray(), "applications/excel", "ReporteEWP.xls");
            }
        }
    }
}