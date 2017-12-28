using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.ProyectoModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class ProyectoController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("ListarProyecto");
        }

        public ActionResult ListarProyecto(int? pagina, string filtro)
        {
            ListarProyectoViewModel model = new ListarProyectoViewModel();
            model.filtro = filtro;
            IEnumerable<Proyecto> Proy = db.Proyectos
                .Where(x => x.Contrato.Vigente).OrderBy(x => x.Nombre);
            if (!String.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                Proy = db.Proyectos
                    .Where(x => x.Nombre.ToLower().Contains(filtro)
                    || x.Contrato.Nombre.ToLower().Contains(filtro)
                    || x.Usuario.NombreCompleto.ToLower().Contains(filtro));
            }
            model.OrdenesServicio = Proy.ToPagedList(pagina ?? 1, 10);
            return View(model);
        }

        public ActionResult CrearEditarProyecto(int? IdProyecto)
        {
            CrearEditarProyectoViewModel model = new CrearEditarProyectoViewModel(db, IdProyecto);
            Proyecto proyecto = db.Proyectos.SingleOrDefault(x => x.IdProyecto == IdProyecto);
            if(proyecto != null)
            {
                model.Form.IdProyecto = proyecto.IdProyecto;
                model.Form.Nombre = proyecto.Nombre;
                model.Form.IdContrato = proyecto.IdContrato;
                model.Form.FechaInicio = proyecto.FechaIncio;
                model.Form.FechaFin = proyecto.FechaTermino.GetValueOrDefault(DateTime.Now);
                model.Form.IdUsuarioDirector = proyecto.IdUsuarioDirector;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CrearEditarProyecto(CrearEditarProyectoFormModel Form)
        {
            if(ModelState.IsValid)
            {
                if(Form.IdProyecto.HasValue)
                {
                    Proyecto proyecto = db.Proyectos.Single(x => x.IdProyecto == Form.IdProyecto);
                    proyecto.Nombre = Form.Nombre;
                    proyecto.IdContrato = Form.IdContrato;
                    proyecto.FechaIncio = Form.FechaInicio;
                    proyecto.FechaTermino = Form.FechaFin;
                    proyecto.IdUsuarioDirector = Form.IdUsuarioDirector;

                    Mensaje = "La Orden de Servicio fue editada exitosamente.";
                }
                else
                {
                    Proyecto proyecto = new Proyecto()
                    {
                        Nombre = Form.Nombre,
                        IdContrato = Form.IdContrato,
                        FechaIncio = Form.FechaInicio,
                        FechaTermino = Form.FechaFin,
                        IdUsuarioDirector = Form.IdUsuarioDirector,
                    };

                    db.Proyectos.InsertOnSubmit(proyecto);
                    Mensaje = "La Orden de Servicio fue creada exitosamente.";
                }

                db.SubmitChanges();
                return RedirectToAction("ListarProyecto");
            }
            CrearEditarProyectoViewModel model = new CrearEditarProyectoViewModel(Form, db, Form.IdProyecto);
            return View(model);
        }
    }
}