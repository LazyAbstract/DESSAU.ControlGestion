using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.RevisionModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RevisionController : BaseController
    {
        public ActionResult ListarRevision(int? pagina, string filtro)
        {
            ListarRevisionViewModel model = new ListarRevisionViewModel();
            IQueryable<Revision> items = db.Revisions;
            if (!String.IsNullOrEmpty(filtro))
            {
                items = items.Where(x => x.Nombre.Contains(filtro));
            }
            model.filtro = filtro;
            model.Revisiones = items.ToPagedList(pagina ?? 1, 10);
            return View(model);
        }

        public ActionResult CrearEditarRevision(int? IdRevision)
        {
            CrearEditarRevisionViewModel model = new CrearEditarRevisionViewModel();
            if (IdRevision.HasValue)
            {
                Revision numDoc = db.Revisions
                    .Single(x => x.IdRevision == IdRevision);
                model.Form.IdRevision = numDoc.IdRevision;
                model.Form.Nombre = numDoc.Nombre;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CrearEditarRevision(CrearEditarRevisionFormModel Form)
        {
            if (ModelState.IsValid)
            {
                if (Form.IdRevision.HasValue)
                {
                    Revision rev = db.Revisions
                        .Single(x => x.IdRevision == Form.IdRevision);
                    rev.Nombre = Form.Nombre;
                }
                else
                {
                    Revision rev = new Revision()
                    {
                        Nombre = Form.Nombre
                    };
                    db.Revisions.InsertOnSubmit(rev);
                }
                db.SubmitChanges();
                TempData["Mensaje"] = "La revisión se ha registrado correctamente.";
                return RedirectToAction("ListarRevision");
            }
            CrearEditarRevisionViewModel model = new CrearEditarRevisionViewModel(Form);
            return View(model);
        }
    }
}