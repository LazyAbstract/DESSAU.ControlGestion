using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class EvaluationSheetViewModel
    {
        public EvaluationSheetFormModel FORM { get; set; }

        public IEnumerable<EvaluacionFormModel> EvaluacionFORMs { get; set; }
        public SelectList Categorias { get; set; }
        public SelectList Proyectos{ get; set; }
        public SelectList PlantillaEvaluacions { get; set; }

        public IEnumerable<UsuarioCategoriaProyecto> UsuarioCategoriaProyectos { get; set; }

        public PlantillaEvaluacion PlantillaEvaluacion { get; set; }

        public IEnumerable<Pregunta> Preguntas { get; set; }

        public EvaluationSheetViewModel()
        {
            FORM = new EvaluationSheetFormModel();
            EvaluacionFORMs = new List<EvaluacionFormModel>();
        }
        public EvaluationSheetViewModel(DESSAUControlGestionDataContext db):this()
        {
            Categorias = new SelectList(db.Categorias, "IdCategoria", "Nombre");
            Proyectos = new SelectList(db.Proyectos, "IdProyecto", "Nombre");
            PlantillaEvaluacions = new SelectList(db.PlantillaEvaluacions, "IdPlantillaEvaluacion", "Nombre");
        }

        public EvaluationSheetViewModel(EvaluationSheetFormModel form, DESSAUControlGestionDataContext db) :this(db)
        {
            FORM = form;
            UsuarioCategoriaProyectos = new List<UsuarioCategoriaProyecto>();
        }

        public EvaluationSheetViewModel(EvaluationSheetFormModel form, DESSAUControlGestionDataContext db, Usuario usuarioActual) : this(form,db)
        {
            if (form.IdCategoria.HasValue)
            {
                PlantillaEvaluacion = db.PlantillaEvaluacions.SingleOrDefault(x => x.IdCategoria == form.IdCategoria.Value);
                Preguntas = PlantillaEvaluacion.PlantillaEvaluacionPreguntas.Select(x=>x.Pregunta);
                UsuarioCategoriaProyectos = usuarioActual
                    .UsuarioSupervisors1
                    .SelectMany(x => x.Usuario.UsuarioCategoriaProyectos)
                    .Where(x => x.IdCategoria == form.IdCategoria.Value && 
                    x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto == TipoEstadoUsuarioCategoriaProyecto.Creado);
               
            }
        }
    }
}