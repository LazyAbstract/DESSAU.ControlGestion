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
            int? IdProyecto = null;
            if(UsuarioActual.IdTipoUsuario == TipoUsuario.DirectorProyecto)
            {
                IdProyecto = db.Proyectos.Single(x => x.IdUsuarioDirector == UsuarioActual.IdUsuario).IdProyecto;
            }
            else if(db.Proyectos.Any(x => x.IdUsuarioDirector == UsuarioActual.IdUsuario))
            {
                IdProyecto = db.Proyectos.Single(x => x.IdUsuarioDirector == UsuarioActual.IdUsuario).IdProyecto;
            }
            return RedirectToAction("EvaluationSheet", new { IdProyecto  = IdProyecto });
        }

        [HttpGet]
        public ActionResult EvaluationSheet(EvaluationSheetFormModel FORM)
        {
            EvaluationSheetViewModel model = new EvaluationSheetViewModel(FORM, db);
            if (FORM.IdProyecto.HasValue)
            {
                model.Categorias = new SelectList(db.UsuarioCategoriaProyectos
                    .Where(x => x.IdProyecto == FORM.IdProyecto && x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                        TipoEstadoUsuarioCategoriaProyecto.NoVigente)
                        .Select(x => x.Categoria).Distinct(), "IdCategoria", "Nombre");
            }
            if (ModelState.IsValid)
            {
                if (FORM.IdCategoria.HasValue)
                {
                    IEnumerable<int> usuariosDistintos = 
                        model.UsuarioCategoriaProyectos
                        .Select(x => x.IdUsuarioCategoriaProyecto).Distinct();
                    model.EvaluacionFORMs = db.Evaluacions
                        .Where(x => //x.FechaEvaluacion == FORM.Fecha && 
                            usuariosDistintos.Contains(x.IdUsuarioCategoriaProyecto) 
                            //&& x.IdUsuarioDirector == UsuarioActual.IdUsuario
                            )
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
            EvaluationSheetViewModel model = new EvaluationSheetViewModel(FORM, db);
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
                            FechaEvaluacion = DateTime.Now,//item.Fecha.Value,
                            IdUsuarioCategoriaProyecto = item.IdUsuarioCategoriaProyecto.Value,
                            IdUsuarioDirector = UsuarioActual.IdUsuario,
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
                return RedirectToAction("EvaluationSheet");//, new { Fecha = model.FORM.Fecha.Value.ToShortDateString(), IdCategoria = model.FORM.IdCategoria });
            }
            return View(model);
        }

        public ActionResult getCategoriaFromProyecto(FormCollection formCollection) {
            var formValue = formCollection.GetValues(0)[0];
            int IdProyecto = Int16.Parse(formValue);
            SelectList result = null;
            result = new SelectList(db.UsuarioCategoriaProyectos
                .Where(x => x.IdProyecto == IdProyecto && x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente)
                    .Select(x => x.Categoria).Distinct(), "IdCategoria", "Nombre");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}