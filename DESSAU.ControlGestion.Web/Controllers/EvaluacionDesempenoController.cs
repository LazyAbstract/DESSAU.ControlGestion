using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.EvaluacionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class EvaluacionDesempenoController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("EvaluationSheet");
        }

        [HttpGet]
        public ActionResult EvaluationSheet(EvaluationSheetFormModel FORM)
        {
            EvaluationSheetViewModel model = new EvaluationSheetViewModel(FORM, db, UsuarioActual);
            if (ModelState.IsValid)
            {
                if (FORM.IdCategoria.HasValue)
                {
                    IEnumerable<int> usuariosDistintos = model.UsuarioCategoriaProyectos.Select(x => x.Usuario).Select(x=> x.IdUsuario).Distinct();
                    model.EvaluacionFORMs = db.Evaluacions
                        .Where(x => x.FechaEvaluacion == FORM.Fecha && 
                            usuariosDistintos.Contains(x.IdUsuario) && 
                            x.IdSupervisor == UsuarioActual.IdUsuario)
                        .Select(x => new EvaluacionFormModel()
                        {
                            IdEvaluacion = x.IdEvaluacion,
                            Fecha = x.FechaEvaluacion,
                            IdUsuario = x.IdUsuario,
                             EvaluacionPreguntaDTOs = x.EvaluacionPreguntas.Select(y=> new EvaluacionPreguntaDTO()
                             {
                                 IdEvaluacionPregunta = y.IdEvaluacionPregunta,
                                 IdPregunta = y.IdPregunta,
                                 ValorObtenido = y.ValorObtenido
                             })
                        });
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EvaluationSheet(EvaluationSheetFormModel FORM, IEnumerable<EvaluacionFormModel> EvaluacionForms)
        {
            EvaluationSheetViewModel model = new EvaluationSheetViewModel(FORM, db, UsuarioActual);
            if (ModelState.IsValid)
            {
                foreach (var item in EvaluacionForms)
                {
                    Evaluacion evaluacion = db.Evaluacions
                        .SingleOrDefault(x => x.IdEvaluacion == item.IdEvaluacion);
                    if (evaluacion == null)
                    {
                        evaluacion = new Evaluacion()
                        {
                            FechaEvaluacion = item.Fecha.Value,
                            IdUsuario = item.IdUsuario.Value, IdSupervisor = UsuarioActual.IdUsuario
                        };
                    }
                    else
                    {
                        evaluacion.EstadoEvaluacion.IdEstadoEvaluacion = TipoEstadoEvaluacion.Editada;
                    }
                    if (!item.IdEvaluacion.HasValue)
                    {
                        db.Evaluacions.InsertOnSubmit(evaluacion);
                    }
                    foreach (var subitem in item.EvaluacionPreguntaDTOs)
                    {
                        EvaluacionPregunta evaluacionPregunta = evaluacion.EvaluacionPreguntas
                            .SingleOrDefault(x => x.IdEvaluacionPregunta == subitem.IdEvaluacionPregunta);
                        if (evaluacionPregunta == null)
                        {
                            evaluacionPregunta = new EvaluacionPregunta()
                            {
                                IdPregunta = subitem.IdPregunta.Value
                            };
                        }
                        evaluacionPregunta.ValorObtenido = subitem.ValorObtenido.Value;
                        if (!subitem.IdEvaluacionPregunta.HasValue)
                        {
                            evaluacion.EvaluacionPreguntas.Add(evaluacionPregunta);
                        }
                    }
                }
                db.SubmitChanges();
                Mensaje = "Se han guardado las evaluaciones con éxito.";
                return RedirectToAction("EvaluationSheet", new { Fecha= model.FORM.Fecha.Value.ToShortDateString(), IdCategoria = model.FORM.IdCategoria });
            }
            return View(model);
        }
    }
}