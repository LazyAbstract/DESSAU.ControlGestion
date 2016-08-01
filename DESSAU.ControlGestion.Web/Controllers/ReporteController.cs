using DESSAU.ControlGestion.Web.Helpers;
using DESSAU.ControlGestion.Web.Models.ReporteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult ReporteDedicacionActividad(ReporteDedicacionActividadFormModel Form)
        {
            ReporteDedicacionActividadViewModel model = new ReporteDedicacionActividadViewModel();
            LectorMonthPicker lector = new LectorMonthPicker();
            model.Periodo = lector.GetMonthNameFromInt(DateTime.Now.Month) 
                + " " + DateTime.Now.Year.ToString();
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
                            Rows = db.fn_ReporteDedicacionActividad(Mes, Ano, IdUsuario, IdProyecto)
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
                            .Select(x => new dynamic [] {
                                x.Actividad,
                                x.PorcentajeDedicacionDeclaracion,
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
                .Where(x => x.IdProyecto == IdProyecto)
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