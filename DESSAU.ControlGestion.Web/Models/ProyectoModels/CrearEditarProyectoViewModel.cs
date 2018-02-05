using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.ProyectoModels
{
    public class CrearEditarProyectoViewModel
    {
        public CrearEditarProyectoFormModel Form { get; set; }
        public IEnumerable<SelectListItem> Contratos { get; set; }
        private ContratoSelectListProvider cslp = new ContratoSelectListProvider();
        public IEnumerable<SelectListItem> UsuariosDirectores { get; set; }
        private UsuarioSelectListProvider uslp = new UsuarioSelectListProvider();

        public CrearEditarProyectoViewModel(DESSAUControlGestionDataContext db, int? IdProyecto)
        {
            Form = new CrearEditarProyectoFormModel();
            Form.FechaInicio = DateTime.Now;
            Form.FechaFin = DateTime.Now;
            Contratos = cslp.Provide();
            UsuariosDirectores = new SelectList(db.Usuarios, "IdUsuario", "ApellidoNombre");
        }

        public CrearEditarProyectoViewModel(CrearEditarProyectoFormModel form, DESSAUControlGestionDataContext db, int? IdProyecto) : this(db, IdProyecto)
        {
            Form = form;
        }
    }
}