using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.NominaModels
{
    public class VerNominaViewModel
    {
        public VerNominaFormModel Form { get; set; }
        public IPagedList<UsuarioCategoriaProyecto> Nominas { get; set; }
        public IEnumerable<SelectListItem> Proyectos { get; set; }
        private ProyectoSelectListProvider pslp = new ProyectoSelectListProvider();

        public VerNominaViewModel()
        {
            Form = new VerNominaFormModel();
            Proyectos = pslp.Provide();
        }

        public VerNominaViewModel(VerNominaFormModel F) : this()
        {
            Form = F;
        }
    }
}