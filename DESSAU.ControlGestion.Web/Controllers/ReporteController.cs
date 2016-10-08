using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Helpers;
using DESSAU.ControlGestion.Web.Models.ReporteModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DESSAU.ControlGestion.Web.Models.ReporteModels.DashboardViewModel;

namespace DESSAU.ControlGestion.Web.Controllers
{
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
                Mes = DateTime.Now.Month;
                Ano = DateTime.Now.Year;
                model.Form.Periodo = lector.GetMonthNameFromInt(DateTime.Now.Month)
                        + " " + DateTime.Now.Year.ToString();                
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

            IQueryable<UsuarioCategoriaProyecto> Nominas = db.UsuarioCategoriaProyectos
                .Where(x => x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto != TipoEstadoUsuarioCategoriaProyecto.NoVigente)
                .OrderBy(x => x.Usuario.ApellidoPaterno);
            if (Form.IdProyecto.HasValue) Nominas = Nominas
                    .Where(x => x.IdProyecto == Form.IdProyecto);
            model.Nominas = Nominas.ToList()
                .Where(x => !x.PlanificacionOk || !x.DeclaracionOk)
                .ToPagedList(pagina ?? 1, 100);
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
                Mes = DateTime.Now.Month;
                Ano = DateTime.Now.Year;
                model.Form.Periodo = lector.GetMonthNameFromInt(DateTime.Now.Month)
                       + " " + DateTime.Now.Year.ToString();
            }
            else
            {
                LectorMonthPicker lector = new LectorMonthPicker(Form.Periodo);
                Mes = lector.GetMes;
                Ano = lector.GetAnno;
                model.Form.Periodo = lector.GetMonthNameFromInt(Mes)
                       + " " + Ano.ToString();
            }

            //var indice = db.fn_IndiceDesviacion(Mes, Ano, model.Form.IdProyecto, model.Form.IdUsuario).FirstOrDefault();
            //double desviacion = 0;
            //if(indice != null)
            //{
            //    desviacion = indice.IndiceDesviacion.GetValueOrDefault(0);
            //}

            //model.PorcentajeDesviacion = desviacion;
            //if (model.PorcentajeDesviacion > 5) model.claseBootstrap = "warning";
            //if (model.PorcentajeDesviacion > 10) model.claseBootstrap = "danger";
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
            //if (!IdProyecto.HasValue) IdProyecto = 1;

            var Data =
               new dynamic[]
               {
                    new {
                        Id = "Data1",
                        Columns = new [] {
                            new { Title = "Actividad", type = "string" },
                            new { Title = "Dedicación Planificada", type = "number" },
                        },
                        Rows = db.fn_ReporteDedicacionActividad(Mes, Ano, IdUsuario, IdProyecto)
                            .OrderBy(x => x.Actividad)
                            .Select(x => new dynamic [] {
                                x.Actividad,
                                x.PorcentajeDedicacionPlanificacion
                        }).ToArray()
                    },
                    new {
                        Id = "Data2",
                        Columns = new[] {
                                new { Title = "Actividad", type = "string" },
                                new { Title = "Dedicación Declarada", type = "number" },
                            },
                        Rows = db.fn_ReporteDedicacionActividad(Mes, Ano, IdUsuario, IdProyecto)
                            .OrderBy(x => x.Actividad)
                            .Select(x => new dynamic [] {
                                x.Actividad,
                                x.PorcentajeDedicacionDeclaracion,
                            }).ToArray()
                    },
                    new {
                        Id = "Data3",
                        Columns = new[] {
                                new { Title = "Actividad", type = "string" },
                                new { Title = "Horas Planificacón", type = "number" },
                                new { Title = "Horas Declaración", type = "number" },
                            },
                        Rows = db.fn_HorasPlanificacionDeclaracion(Mes, Ano, IdUsuario, IdProyecto)
                            .OrderByDescending(x => x.HorasPlanificadas)
                            .Select(x => new dynamic [] {
                                x.Actividad,
                                x.HorasPlanificadas,
                                x.HorasReportadas,
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
    }
}