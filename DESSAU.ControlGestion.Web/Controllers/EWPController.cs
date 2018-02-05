using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.EWPModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class EWPController : BaseController
    {
        //  CRU EWP
        public ActionResult ListarEWP(int? pagina, string filtro)
        {
            ListarEWPViewModel model = new ListarEWPViewModel();
            IQueryable<EWP> items = db.EWPs.OrderBy(x => x.Codigo)
                .ThenBy(x => x.Nombre);
            if(!String.IsNullOrEmpty(filtro))
            {
                items = items.Where(x => x.Codigo.Contains(filtro)
                    || x.Nombre.Contains(filtro));
            }
            model.filtro = filtro;
            model.EWPs = items.ToPagedList(pagina ?? 1, 10);
            return View(model);
        }

        public ActionResult CrearEditarEWP(int? IdEWP)
        {
            CrearEditarEWPViewModel model = new CrearEditarEWPViewModel();
            if(IdEWP.HasValue)
            {
                EWP ewp = db.EWPs.Single(x => x.IdEWP == IdEWP);
                model.Form.IdEWP = ewp.IdEWP;
                model.Form.Codigo = ewp.Codigo;
                model.Form.Nombre = ewp.Nombre;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CrearEditarEWP(CrearEditarEWPFormModel Form)
        {
            if(ModelState.IsValid)
            {
                if(Form.IdEWP.HasValue)
                {
                    EWP ewp = db.EWPs.Single(x => x.IdEWP == Form.IdEWP);
                    ewp.Codigo = Form.Codigo;
                    ewp.Nombre = Form.Nombre;
                }
                else
                {
                    EWP ewp = new EWP()
                    {
                        Codigo = Form.Codigo,
                        Nombre = Form.Nombre
                    };
                    db.EWPs.InsertOnSubmit(ewp);
                }
                db.SubmitChanges();
                TempData["Mensaje"] = "La EWP se a agregado con éxito.";
                return RedirectToAction("ListarEWP");
            }
            CrearEditarEWPViewModel model = new CrearEditarEWPViewModel(Form);
            return View(model);
        }

        //  CRU Sub EWP

        public ActionResult ListarSubEWP(int? pagina, string filtro)
        {
            ListarSubEWPViewModel model = new ListarSubEWPViewModel();
            IQueryable<SubEWP> items = db.SubEWPs.OrderBy(x => x.Codigo);
            if (!String.IsNullOrEmpty(filtro))
            {
                items = items.Where(x => x.Codigo.Contains(filtro));
            }
            model.SubEWPs = items.ToPagedList(pagina ?? 1, 10);
            model.filtro = filtro;
            return View(model);
        }

        public ActionResult CrearEditarSubEWP(int? IdSubEWP)
        {
            CrearEditarSubEWPViewModel model = new CrearEditarSubEWPViewModel();
            if (IdSubEWP.HasValue)
            {
                SubEWP subewp = db.SubEWPs.Single(x => x.IdSubEWP == IdSubEWP);
                model.Form.IdEWP = subewp.IdEWP;
                model.Form.Codigo = subewp.Codigo;
                model.Form.IdSubEWP = IdSubEWP;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CrearEditarSubEWP(CrearEditarSubEWPFormModel Form)
        {
            if (ModelState.IsValid)
            {
                if (Form.IdSubEWP.HasValue)
                {
                    SubEWP subewp = db.SubEWPs.Single(x => x.IdSubEWP == Form.IdSubEWP);
                    subewp.Codigo = Form.Codigo;
                    subewp.IdEWP = Form.IdEWP;
                    subewp.Nombre = Form.Nombre;
                }
                else
                {
                    SubEWP subewp = new SubEWP()
                    {
                        Codigo = Form.Codigo,
                        IdEWP = Form.IdEWP,
                        Nombre = Form.Nombre
                    };
                    db.SubEWPs.InsertOnSubmit(subewp);
                }
                db.SubmitChanges();
                TempData["Mensaje"] = "La Sub EWP se a agregado con éxito.";
                return RedirectToAction("ListarEWP");
            }
            CrearEditarSubEWPViewModel model = new CrearEditarSubEWPViewModel(Form);
            return View(model);
        }

        
    }
}