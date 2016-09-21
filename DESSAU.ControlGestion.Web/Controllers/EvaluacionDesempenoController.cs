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
                    IEnumerable<int> usuariosDistintos = 
                        model.UsuarioCategoriaProyectos
                        .Select(x => x.IdUsuarioCategoriaProyecto).Distinct();
                    model.EvaluacionFORMs = db.Evaluacions
                        .Where(x => x.FechaEvaluacion == FORM.Fecha && 
                            usuariosDistintos.Contains(x.IdUsuarioCategoriaProyecto) && 
                            x.IdUsuarioDirector == UsuarioActual.IdUsuario)
                        .Select(x => new EvaluacionFormModel()
                        {
                            IdEvaluacion = x.IdEvaluacion,
                            Fecha = x.FechaEvaluacion,
                            IdUsuarioCategoriaProyecto = x.IdUsuarioCategoriaProyecto,
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
                            IdUsuarioCategoriaProyecto = item.IdUsuarioCategoriaProyecto.Value,
                            IdUsuarioDirector = UsuarioActual.IdUsuario
                        };
                        
                    }
                    else
                    {
                        evaluacion.EstadoEvaluacion.IdTipoEstadoEvaluacion = TipoEstadoEvaluacion.Editada;
                        evaluacion.EstadoEvaluacion.CreadoPor = User.Identity.Name;
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
        [HttpGet]
        public ActionResult getCategoriaFromProyecto(string idProyecto, FormCollection formCollection) {
            int idProyectoBuffer = 0;
            SelectList result = null;
            if (formCollection.AllKeys.Any(x => x == "Form.IdProyecto") && Int32.TryParse(formCollection.Get("Form.IdProyecto"),out idProyectoBuffer))
            {
                result = new SelectList(db.UsuarioCategoriaProyectos
                    .Where(x => x.IdProyecto == idProyectoBuffer)
                    .Select(x => x.Categoria).Distinct(), "IdCategoria", "Nombre");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}