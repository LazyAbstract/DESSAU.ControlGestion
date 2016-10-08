using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.ReporteModels
{
    public class DashboardViewModel
    {
        public DashboardFormModel Form { get; set; }
        public IEnumerable<SelectListItem> Proyectos { get; set; }
        public IPagedList<UsuarioCategoriaProyecto> Nominas { get; set; }
        public CalculoHoraMensual calc { get; set; }
        public IEnumerable<SelectListItem> Usuarios { get; set; }
        private ProyectoSelectListProvider pslp = new ProyectoSelectListProvider();
        private UsuarioSelectListProvider uslp = new UsuarioSelectListProvider();
        public string claseBootstrap { get; set; }
        public double PorcentajeDesviacion { get; set; }

        public DashboardViewModel()
        {
            Form = new DashboardFormModel();
            Proyectos = pslp.Provide();
            Usuarios = uslp.Provide();
        }

        public DashboardViewModel(DashboardFormModel form) : this()
        {
            Form = form;
        }
    }
}