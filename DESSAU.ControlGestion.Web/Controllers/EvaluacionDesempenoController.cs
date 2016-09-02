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
            return View();
        }

        [HttpGet]
        public ActionResult EvaluationSheet(EvaluationSheetFormModel FORM)
        {
            EvaluationSheetViewModel model = new EvaluationSheetViewModel(FORM, db);
            if (ModelState.IsValid)
            {
                if (FORM.IdPlantillaEvaluacion.HasValue)
                {
                    model.UsuarioCategoriaProyectos = db.UsuarioCategoriaProyectos
                        .Where(x => x.EstadoUsuarioCategoriaProyecto
                            .IdTipoEstadoUsuarioCategoriaProyecto == 
                            TipoEstadoUsuarioCategoriaProyecto.Creado &&
                            x.IdCategoria == model.PlantillaEvaluacion.IdCategoria);
                    model.PlantillaEvaluacion = db.PlantillaEvaluacions
                        .SingleOrDefault(x => x.IdPlantillaEvaluacion == FORM.IdPlantillaEvaluacion);
                    model.Preguntas = model.PlantillaEvaluacion.PlantillaEvaluacionPreguntas.Select(x => x.Pregunta);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EvaluationSheet(EvaluationSheetFormModel FORM, FormCollection formCollection)
        {
            EvaluationSheetViewModel model = new EvaluationSheetViewModel(FORM,db);
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
    }
}