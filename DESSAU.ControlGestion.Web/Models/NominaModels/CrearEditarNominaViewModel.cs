using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.NominaModels
{
    public class CrearEditarNominaViewModel
    {
        public CrearEditarNominaFormModel Form { get; set; }
        private ProyectoSelectListProvider pslp = new ProyectoSelectListProvider();
        private CategoriaSelectListProvider cslp = new CategoriaSelectListProvider();
        public IEnumerable<SelectListItem> Proyectos { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }
        public Usuario Usuario { get; set; } 

        public CrearEditarNominaViewModel()
        {
            Form = new CrearEditarNominaFormModel();
            Proyectos = pslp.Provide();
            Categorias = cslp.Provide();
        }

        public CrearEditarNominaViewModel(CrearEditarNominaFormModel form) : this()
        {
            Form = form;
        }
    }
}