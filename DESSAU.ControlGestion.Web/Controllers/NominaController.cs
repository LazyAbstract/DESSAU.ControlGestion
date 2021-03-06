﻿using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.NominaModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
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
            IQueryable<fn_ReportePorUsuarioResult> Nominas = db.fn_ReportePorUsuario(IdProyecto, DateTime.Now)
                .OrderBy(x => x.ApellidoPaterno);
            model.Nominas = Nominas.ToPagedList(pagina ?? 1, 10);
            model.NominaNoAsignados = db.Usuarios
                .Where(x => !x.UsuarioCategoriaProyectos.Any(y => y.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto
                    != TipoEstadoUsuarioCategoriaProyecto.NoVigente
                    ) && x.Vigente).OrderBy(x => x.ApellidoPaterno);
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

        public ActionResult ExportarNominaExcel(int? IdProyecto)
        {
            IEnumerable<UsuarioCategoriaProyecto> Nomina = db.UsuarioCategoriaProyectos
                .Where(x => x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto != TipoEstadoUsuarioCategoriaProyecto.NoVigente)
                .OrderBy(x => x.Proyecto.Nombre).ThenBy(x => x.Usuario.ApellidoPaterno);
            if (IdProyecto.HasValue) Nomina = Nomina.Where(x => x.IdProyecto == IdProyecto);
            IEnumerable<ExportaNominaExcelViewModel> NominaExportar =
                Mapper.Map<IEnumerable<UsuarioCategoriaProyecto>, IEnumerable<ExportaNominaExcelViewModel>>(Nomina);
            using (var Lista = new MemoryStream())
            {
                ExportadorExcel.GeneraExcel<ExportaNominaExcelViewModel>(NominaExportar, "Lista", Lista, true);
                return File(Lista.ToArray(), "applications/excel", "Nómina.xls");
            }
        }
    }
}