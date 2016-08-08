using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.ActividadModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ActividadController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("ListarActividad");
        }

        public ActionResult ListarActividad(int? pagina, string filtro)
        {
            ListarActividadViewModel model = new ListarActividadViewModel();
            model.filtro = filtro;
            IEnumerable<Actividad> Acts = db.Actividads.OrderBy(x => x.IdTipoActividad)
                .ThenBy(x => x.Nombre);
            if (!String.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                Acts = db.Actividads
                    .Where(x => x.Nombre.ToLower().Contains(filtro));
            }
            model.Actividades = Acts.ToPagedList(pagina ?? 1, 10);
            return View(model);
        }

        public ActionResult CrearEditarActividad(int? IdActividad)
        {
            CrearEditarActividadViewModel model = new CrearEditarActividadViewModel();
            if (IdActividad.HasValue)
            {
                Actividad act = db.Actividads.Single(x => x.IdActividad == IdActividad);
                model.Form.NombreActividad = act.Nombre;
                model.Form.IdActividad = IdActividad;
                model.Form.IdTipoActividad = act.IdTipoActividad;
                model.Form.IdCategorias = act.CategoriaActividads.Where(x => x.Vigente == true)
                    .Select(x => x.IdCategoria).ToList();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CrearEditarActividad(CrearEditarActividadFormModel Form)
        {
            if(!Form.IdCategorias.Any())
            {
                ModelState.AddModelError("", "Debe seleccionar al menos una Categoría.");
            }
            if(ModelState.IsValid)
            {
                if (!Form.IdActividad.HasValue)
                {
                    Actividad act = new Actividad()
                    {
                        Nombre = Form.NombreActividad,
                        IdTipoActividad = Form.IdTipoActividad,
                    };

                    db.Actividads.InsertOnSubmit(act);
                    db.SubmitChanges();

                    foreach (var IdCategoria in Form.IdCategorias)
                    {
                        CategoriaActividad catAct = new CategoriaActividad()
                        {
                            IdActividad = act.IdActividad,
                            IdCategoria = IdCategoria,
                            Vigente = true
                        };

                        db.CategoriaActividads.InsertOnSubmit(catAct);
                    }

                    db.SubmitChanges();
                    Mensaje = "La Actividad fue creada exitosamente.";
                    return RedirectToAction("ListarActividad");
                }
                else
                {
                    Actividad act = db.Actividads.Single(x => x.IdActividad == Form.IdActividad);
                    act.Nombre = Form.NombreActividad;
                    act.IdTipoActividad = Form.IdTipoActividad;

                    foreach(var item in act.CategoriaActividads)
                    {
                        item.Vigente = false;
                    }

                    foreach (var IdCategoria in Form.IdCategorias)
                    {
                        var hola = db.CategoriaActividads
                            .SingleOrDefault(x => x.IdActividad == act.IdActividad 
                            && x.IdCategoria == IdCategoria);
                        if (hola != null)
                        {
                            hola.Vigente = true;
                        }
                        else
                        {
                            CategoriaActividad catAct = new CategoriaActividad()
                            {
                                IdActividad = act.IdActividad,
                                IdCategoria = IdCategoria,
                                Vigente = true,
                            };

                            db.CategoriaActividads.InsertOnSubmit(catAct);
                        }           
                    }

                    db.SubmitChanges();
                    Mensaje = "La Actividad fue editada exitosamente.";
                    return RedirectToAction("ListarActividad");
                }
            }
            CrearEditarActividadViewModel model = new CrearEditarActividadViewModel(Form);
            return View(model);
        }
    }
}