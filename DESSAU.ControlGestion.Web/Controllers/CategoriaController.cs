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
            IEnumerable<Categoria> Cats = db.Categorias.OrderBy(x => x.Nombre);
            if (!String.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                Cats = db.Categorias
                    .Where(x => x.Nombre.ToLower().Contains(filtro));
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
                    db.SubmitChanges();
                    Mensaje = "La Categoría fue creada exitosamente.";
                    return RedirectToAction("ListarCategoria");
                }
                else
                {
                    Categoria Cat = new Categoria()
                    {
                        Nombre = Form.NombreCategoria,
                    };
                    db.Categorias.InsertOnSubmit(Cat);
                    db.SubmitChanges();
                    Mensaje = "La Categoría fue editada exitosamente.";
                    return RedirectToAction("ListarCategoria");

                }
            }
            CrearEditarCategoriaViewModel model = new CrearEditarCategoriaViewModel(Form);
            return View(model);
        }


    }
}