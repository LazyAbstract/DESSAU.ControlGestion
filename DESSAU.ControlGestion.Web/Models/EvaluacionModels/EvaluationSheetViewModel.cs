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
        public SelectList Proyectos { get; set; }
        public SelectList Usuarios { get; set; }
        //public SelectList PlantillaEvaluacions { get; set; }
        public string Periodo { get; set; }
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
            Usuarios = new SelectList(db.UsuarioCategoriaProyectos
                .Where(x => x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto !=
                    TipoEstadoUsuarioCategoriaProyecto.NoVigente)
                .OrderBy(x => x.Usuario.ApellidoPaterno), "IdUsuarioCategoriaProyecto", "Usuario.ApellidoNombre");
            //PlantillaEvaluacions = new SelectList(db.PlantillaEvaluacions, "IdPlantillaEvaluacion", "Nombre");
        }

        public EvaluationSheetViewModel(EvaluationSheetFormModel form, DESSAUControlGestionDataContext db) :this(db)
        {
            FORM = form;
            UsuarioCategoriaProyectos = new List<UsuarioCategoriaProyecto>();            
            if (form.IdCategoria.HasValue && form.IdProyecto.HasValue)
            {
                Proyecto proyecto = db.Proyectos.Single(x => x.IdProyecto == form.IdProyecto);
                PlantillaEvaluacion = db.PlantillaEvaluacions.SingleOrDefault(x => x.IdCategoria == form.IdCategoria.Value);
                Preguntas = PlantillaEvaluacion.PlantillaEvaluacionPreguntas.Select(x => x.Pregunta);
                UsuarioCategoriaProyectos = proyecto.UsuarioCategoriaProyectos
                    .Where(x => x.IdCategoria == form.IdCategoria.Value &&
                    x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto == TipoEstadoUsuarioCategoriaProyecto.Creado);

            }
        }

        //public EvaluationSheetViewModel(EvaluationSheetFormModel form, DESSAUControlGestionDataContext db, int? IdUsuarioDirector) : this(form,db)
        //{
        //    Usuario usuarioActual = db.Usuarios.Single(x => x.IdUsuario == IdUsuarioDirector);
        //    Proyecto proyecto = db.Proyectos.Single(x => x.IdUsuarioDirector == IdUsuarioDirector);
        //    if (form.IdCategoria.HasValue && IdUsuarioDirector.HasValue)
        //    {
        //        PlantillaEvaluacion = db.PlantillaEvaluacions.SingleOrDefault(x => x.IdCategoria == form.IdCategoria.Value);
        //        Preguntas = PlantillaEvaluacion.PlantillaEvaluacionPreguntas.Select(x=>x.Pregunta);
        //        UsuarioCategoriaProyectos = proyecto.UsuarioCategoriaProyectos
        //            .Where(x => x.IdCategoria == form.IdCategoria.Value && 
        //            x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto == TipoEstadoUsuarioCategoriaProyecto.Creado);
               
        //    }
        //}
    }
}