using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.NumeroDocumentoModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    [Authorize]
    public class NumeroDocumentoController : BaseController
    {
        public ActionResult ListarNumeroDocumento(int? pagina, string filtro)
        {
            ListarNumeroDocumentoViewModel model = new ListarNumeroDocumentoViewModel();
            IQueryable<NumeroDocumento> items = db.NumeroDocumentos;
            if (!String.IsNullOrEmpty(filtro))
            {
                items = items.Where(x => x.Codigo.Contains(filtro)
                    || x.Nombre.Contains(filtro));
            }
            model.filtro = filtro;
            model.NumeroDocumentos = items.ToPagedList(pagina ?? 1, 10);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CrearEditarNumeroDocumento(int? IdNumeroDocumento)
        {
            CrearEditarNumeroDocumentoViewModel model = new CrearEditarNumeroDocumentoViewModel();
            if(IdNumeroDocumento.HasValue)
            {
                NumeroDocumento numDoc = db.NumeroDocumentos
                    .Single(x => x.IdNumeroDocumento == IdNumeroDocumento);
                model.Form.IdNumeroDocumento = numDoc.IdNumeroDocumento;
                model.Form.IdEWP = numDoc.IdEWP;
                model.Form.Codigo = numDoc.Codigo;
                model.Form.Nombre = numDoc.Nombre;
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CrearEditarNumeroDocumento(CrearEditarNumeroDocumentoFormModel Form)
        {
            if (ModelState.IsValid)
            {
                if (Form.IdNumeroDocumento.HasValue)
                {
                    NumeroDocumento numDoc = db.NumeroDocumentos
                        .Single(x => x.IdNumeroDocumento == Form.IdNumeroDocumento);
                    numDoc.IdEWP = Form.IdEWP;
                    numDoc.Codigo = Form.Codigo;
                    numDoc.Nombre = Form.Nombre;
                }
                else
                {
                    NumeroDocumento numDoc = new NumeroDocumento()
                    {
                        IdEWP = Form.IdEWP,
                        Codigo = Form.Codigo,
                        Nombre = Form.Nombre
                    };
                    db.NumeroDocumentos.InsertOnSubmit(numDoc);
                }
                db.SubmitChanges();
                TempData["Mensaje"] = "El Número de Documnetos se ha registrado correctamente.";
                return RedirectToAction("ListarNumeroDocumento");
            }
            CrearEditarNumeroDocumentoViewModel model = new CrearEditarNumeroDocumentoViewModel(Form);
            return View(model);
        }
    }
}