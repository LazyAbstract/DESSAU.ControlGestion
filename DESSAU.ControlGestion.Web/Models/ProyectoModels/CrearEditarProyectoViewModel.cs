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

        public CrearEditarProyectoViewModel()
        {
            Form = new CrearEditarProyectoFormModel();
            Form.FechaInicio = DateTime.Now;
            Form.FechaFin = DateTime.Now;
            Contratos = cslp.Provide();
        }

        public CrearEditarProyectoViewModel(CrearEditarProyectoFormModel form) : this()
        {
            Form = form;
        }
    }
}