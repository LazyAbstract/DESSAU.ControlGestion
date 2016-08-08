using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.NominaModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class NominaController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("VerNomina");
        }

        [HttpGet]
        public ActionResult VerNomina(int? pagina, int? IdProyecto)
        {
            VerNominaViewModel model = new VerNominaViewModel();
            IQueryable<UsuarioCategoriaProyecto> Nominas = db.UsuarioCategoriaProyectos
                .Where(x => x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto != TipoEstadoUsuarioCategoriaProyecto.NoVigente)
                .OrderBy(x => x.Usuario.ApellidoPaterno);
            if (IdProyecto.HasValue) Nominas = db.UsuarioCategoriaProyectos
                    .Where(x => x.IdProyecto == IdProyecto);
            model.Nominas = Nominas.ToPagedList(pagina ?? 1, 10);
            model.NominaNoAsignados = db.Usuarios.Where(x => !x.UsuarioCategoriaProyectos.Any() 
                && x.IdTipoUsuario != TipoUsuario.Admisnitrador).OrderBy(x => x.ApellidoPaterno);
            model.IdProyecto = IdProyecto;
            return View(model);
        }

        public ActionResult CrearEditarNomina(int IdUsuario)
        {
            CrearEditarNominaViewModel model = new CrearEditarNominaViewModel();
            model.Usuario = db.Usuarios.Single(x => x.IdUsuario == IdUsuario);
            model.Form.IdUsuario = model.Usuario.IdUsuario;
            return View(model);
        }

        [HttpPost]
        public ActionResult CrearEditarNomina(CrearEditarNominaFormModel Form)
        {
            if(ModelState.IsValid)
            {
                Usuario user = db.Usuarios.Single(x => x.IdUsuario == Form.IdUsuario);
                if (user.UsuarioCategoriaProyectos.Any())
                {
                    foreach(var item in user.UsuarioCategoriaProyectos)
                    {
                        item.EstadoUsuarioCategoriaProyecto
                        .IdTipoEstadoUsuarioCategoriaProyecto = TipoEstadoUsuarioCategoriaProyecto.NoVigente;
                    }                    
                }

                UsuarioCategoriaProyecto UPC = new UsuarioCategoriaProyecto()
                {
                    IdUsuario = Form.IdUsuario,
                    IdCategoria = Form.IdCategoria,
                    IdProyecto = Form.IdProyecto,
                };

                db.UsuarioCategoriaProyectos.InsertOnSubmit(UPC);                
                db.SubmitChanges();
                Mensaje = "La asignación del profesional a la ODS fue procesada correctamente.";
                return RedirectToAction("VerNomina");
            }
            CrearEditarNominaViewModel model = new CrearEditarNominaViewModel(Form);
            return View(model);
        }
    }
}