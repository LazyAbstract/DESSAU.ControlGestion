using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.CategoriaModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class CategoriaController : BaseController
    {
        // GET: Categoria
        public ActionResult Index()
        {
            return RedirectToAction("ListarCategoria");
        }

        public ActionResult ListarCategoria(int? pagina, string filtro)
        {
            ListarCategoriaViewModel model = new ListarCategoriaViewModel();
            model.filtro = filtro;
            IEnumerable<Categoria> Cats = db.Categorias
                .Where(x => x.Vigente)
                .OrderBy(x => x.Nombre);
            if (!String.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                Cats = db.Categorias
                    .Where(x => x.Vigente && x.Nombre.ToLower().Contains(filtro));
            }
            model.Categorias= Cats.ToPagedList(pagina ?? 1, 10);
            return View(model);
        }

        public ActionResult CrearEditarCategoria(int? IdCategoria)
        {
            CrearEditarCategoriaViewModel model = new CrearEditarCategoriaViewModel();
            if(IdCategoria.HasValue)
            {
                Categoria Cat = db.Categorias.Single(x => x.IdCategoria == IdCategoria);
                model.Form.IdCategoria = IdCategoria;
                model.Form.NombreCategoria = Cat.Nombre;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CrearEditarCategoria(CrearEditarCategoriaFormModel Form)
        {
            if(ModelState.IsValid)
            {
                if (Form.IdCategoria.HasValue)
                {
                    Categoria Cat = db.Categorias.Single(x => x.IdCategoria == Form.IdCategoria);
                    Cat.Nombre = Form.NombreCategoria;
                    Cat.Vigente = true;
                    db.SubmitChanges();
                    Mensaje = "La Categoría fue creada exitosamente.";
                    return RedirectToAction("ListarCategoria");
                }
                else
                {
                    if(db.Categorias.Any(x => x.Nombre == Form.NombreCategoria && !x.Vigente))
                    {
                        Categoria cat2 = db.Categorias.Single(x => x.Nombre == Form.NombreCategoria && !x.Vigente);
                        cat2.Vigente = true;
                    }
                    else
                    {
                        Categoria Cat = new Categoria()
                        {
                            Nombre = Form.NombreCategoria,
                            Vigente = true,
                        };
                        db.Categorias.InsertOnSubmit(Cat);
                    }                    
                    db.SubmitChanges();
                    Mensaje = "La Categoría fue editada exitosamente.";
                    return RedirectToAction("ListarCategoria");

                }
            }
            CrearEditarCategoriaViewModel model = new CrearEditarCategoriaViewModel(Form);
            return View(model);
        }

        public ActionResult AgregarActividadCategoria(int IdCategoria)
        {
            AgregarActividadCategoriaViewModel model = new AgregarActividadCategoriaViewModel();
            model.Form.IdCategoria = IdCategoria;
            model.Categoria = db.Categorias.Single(x => x.IdCategoria == IdCategoria);
            return View(model);
        }

        [HttpPost]
        public ActionResult AgregarActividadCategoria(AgregarActividadCategoriaFormModel Form)
        {
            if (ModelState.IsValid)
            {
                if(db.CategoriaActividads.Any(x => x.IdCategoria == Form.IdCategoria && x.IdActividad == Form.IdActividad && !x.Vigente))
                {
                    var hola = db.CategoriaActividads.Single(x => x.IdCategoria == Form.IdCategoria && x.IdActividad == Form.IdActividad && !x.Vigente);
                    hola.Vigente = false;
                }
                else
                {
                    CategoriaActividad cata = new CategoriaActividad()
                    {
                        IdCategoria = Form.IdCategoria,
                        IdActividad = Form.IdActividad,
                        Vigente = true,
                    };

                    db.CategoriaActividads.InsertOnSubmit(cata);
                }
                
                db.SubmitChanges();

                TempData["Mensaje"] = "Se ha agregado la actividad con éxito a la Disciplina";
                return RedirectToAction("ListarCategoria");
            }
            AgregarActividadCategoriaViewModel model = new AgregarActividadCategoriaViewModel(Form);
            model.Categoria = db.Categorias.Single(x => x.IdCategoria == Form.IdCategoria);
            return View(model);
        }

        public ActionResult AliminarActividadCategoria(int IdCategoriaActividad)
        {
            CategoriaActividad cata = db.CategoriaActividads.Single(x => x.IdCategoriaActividad == IdCategoriaActividad);
            cata.Vigente = false;
            db.SubmitChanges();
            TempData["Mesaje"] = "La Actividad fue desasociada a la Disciplina.";
            return RedirectToAction("AgregarActividadCategoria", new { IdCategoria = cata.IdCategoria });
        }
    }
}