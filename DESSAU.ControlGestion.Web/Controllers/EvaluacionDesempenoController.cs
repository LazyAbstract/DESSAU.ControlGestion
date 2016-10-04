using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Helpers;
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
            int Mes;
            int Ano;
            EvaluationSheetViewModel model = new EvaluationSheetViewModel(FORM, db);
            if (String.IsNullOrWhiteSpace(FORM.Periodo))
            {
                LectorMonthPicker lector = new LectorMonthPicker();
                var fecha = DateTime.Now.AddMonths(-1);
                Mes = fecha.Month;
                Ano = fecha.Year;
                model.FORM.Periodo = lector.GetMonthNameFromInt(fecha.Month)
                        + " " + fecha.Year.ToString();
            }
            else
            {
                LectorMonthPicker lector = new LectorMonthPicker(FORM.Periodo);
                Mes = lector.GetMes;
                Ano = lector.GetAnno;
            }
            
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
                    IEnumerable<int> usuariosDistintos = model.UsuarioCategoriaProyectos
                        .Select(x => x.IdUsuarioCategoriaProyecto).Distinct();
                    if (FORM.IdUsuarioCategoriaProyecto.HasValue) usuariosDistintos.Where(x => x == FORM.IdUsuarioCategoriaProyecto);
                    model.EvaluacionFORMs = db.Evaluacions.Where(x => x.FechaEvaluacion == new DateTime(Ano, Mes, 1) 
                        && usuariosDistintos.Contains(x.IdUsuarioCategoriaProyecto) 
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
            int Mes;
            int Ano;
            if (String.IsNullOrWhiteSpace(FORM.Periodo))
            {
                var fecha = DateTime.Now.AddMonths(-1);
                Mes = fecha.Month;
                Ano = fecha.Year;
            }
            else
            {
                LectorMonthPicker lector = new LectorMonthPicker(FORM.Periodo);
                Mes = lector.GetMes;
                Ano = lector.GetAnno;
            }
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
                            FechaEvaluacion = new DateTime(Ano, Mes, 1),
                            FechaCreacion = DateTime.Now,
                            IdUsuarioCategoriaProyecto = item.IdUsuarioCategoriaProyecto.Value,
                            IdUsuarioDirector = UsuarioActual.IdUsuario,
                        };
                        
                    }
                    else
                    {
                        evaluacion.EstadoEvaluacion.IdTipoEstadoEvaluacion = TipoEstadoEvaluacion.Editada;
                        evaluacion.EstadoEvaluacion.CreadoPor = User.Identity.Name;
                        evaluacion.FechaCreacion = DateTime.Now;
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

                    db.SubmitChanges();
                    evaluacion.Promedio = evaluacion.EvaluacionPreguntas.Average(X => X.ValorObtenido);
                }
                db.SubmitChanges();

                Mensaje = "Se han guardado las evaluaciones con éxito.";
                return RedirectToAction("ListadoEvaluaciones", new { Periodo = FORM.Periodo, IdProyecto = FORM.IdProyecto });
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

        public ActionResult getProfesionalFromCategoria(FormCollection formCollection)
        {
            var formValue = formCollection.GetValues(0)[0];
            int IdCategoria = Int16.Parse(formValue);
            SelectList result = null;
            result = new SelectList(db.UsuarioCategoriaProyectos
                .Where(x => x.IdCategoria == IdCategoria && x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente)
                    .Distinct(), "IdUsuarioCategoriaProyecto", "Usuario.ApellidoNombre");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListadoEvaluaciones(ListadoEvaluacionesDesempenoFormModel Form)
        {
            int Mes;
            int Ano;
            ListadoEvaluacionesDesempenoViewModel model = new ListadoEvaluacionesDesempenoViewModel();
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
            if (UsuarioActual.IdTipoUsuario == TipoUsuario.DirectorProyecto)
            {
                Form.IdProyecto = db.Proyectos.Single(x => x.IdUsuarioDirector == UsuarioActual.IdUsuario).IdProyecto;
            }
            //else if (db.Proyectos.Any(x => x.IdUsuarioDirector == UsuarioActual.IdUsuario))
            //{
            //    Form.IdProyecto = db.Proyectos.Single(x => x.IdUsuarioDirector == UsuarioActual.IdUsuario).IdProyecto;
            //}
            model.Evaluaciones = db.Evaluacions
                .Where(x => x.FechaEvaluacion == new DateTime(Ano, Mes, 1))
                .OrderBy(x => x.UsuarioCategoriaProyecto.Usuario.ApellidoPaterno);

            model.NoEvaluados = db.UsuarioCategoriaProyectos
                .Where(x => !x.Evaluacions.Any(y => y.FechaEvaluacion == new DateTime(Ano, Mes, 1) 
                && x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                        TipoEstadoUsuarioCategoriaProyecto.NoVigente))
                        .OrderBy(x => x.Usuario.ApellidoPaterno);

            if(Form.IdProyecto.HasValue)
            {
                model.Evaluaciones = model.Evaluaciones.Where(x => x.UsuarioCategoriaProyecto.IdProyecto == Form.IdProyecto);
                model.NoEvaluados = model.NoEvaluados.Where(x => x.IdProyecto == Form.IdProyecto);
            }
            
            return View(model);
        }

        public ActionResult DetalleEvaluacionDesempeno(int IdEvaluacion)
        {
            return View(db.Evaluacions.Single(x => x.IdEvaluacion == IdEvaluacion));
        }

        public ActionResult ExportarEvaluacionDesempenoPDF(int IdEvaluacion)
        {
            throw new NotImplementedException("TODO: Xampi, la magia del PDF plz!");
        }
    }
}