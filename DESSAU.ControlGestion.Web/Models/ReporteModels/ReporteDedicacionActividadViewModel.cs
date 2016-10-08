using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.ReporteModels
{
    public class ReporteDedicacionActividadViewModel
    {
        public ReporteDedicacionActividadFormModel Form { get; set; }
        //public string Periodo { get; set; }
        public string claseBootstrap { get; set; }
        public double PorcentajeDesviacion { get; set; }
        public UsuarioCategoriaProyecto UCP { get; set; }
        public IEnumerable<SelectListItem> Proyectos { get; set; }
        public IEnumerable<SelectListItem> Usuarios { get; set; }
        private ProyectoSelectListProvider pslp = new ProyectoSelectListProvider();
        private UsuarioSelectListProvider uslp = new UsuarioSelectListProvider();

        public ReporteDedicacionActividadViewModel()
        {
            Form = new ReporteDedicacionActividadFormModel();
            Proyectos = pslp.Provide();
            Usuarios = uslp.Provide();
            claseBootstrap = "success";
        }

        public ReporteDedicacionActividadViewModel(ReporteDedicacionActividadFormModel F) : this()
        {
            Form = F;
        }
    }
}