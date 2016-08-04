using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.CategoriaModels
{
    public class CrearEditarCategoriaViewModel
    {
        public CrearEditarCategoriaFormModel Form { get; set; }

        public CrearEditarCategoriaViewModel()
        {
            Form = new CrearEditarCategoriaFormModel();
        }

        public CrearEditarCategoriaViewModel(CrearEditarCategoriaFormModel form) : this()
        {
            Form = form;
        }
    }
}