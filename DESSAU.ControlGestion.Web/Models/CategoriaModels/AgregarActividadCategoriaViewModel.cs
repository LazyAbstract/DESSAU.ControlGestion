using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.CategoriaModels
{
    public class AgregarActividadCategoriaViewModel
    {
        public AgregarActividadCategoriaFormModel Form { get; set; }
        ActividadSelectListProvider aslp = new ActividadSelectListProvider();
        public IEnumerable<SelectListItem> Actividades { get; set; }
        public Categoria Categoria { get; set; }

        public AgregarActividadCategoriaViewModel()
        {
            Form = new AgregarActividadCategoriaFormModel();
            Actividades = aslp.Provide();
        }

        public AgregarActividadCategoriaViewModel(AgregarActividadCategoriaFormModel F) : this()
        {
            Form = F;
        }
    }
}