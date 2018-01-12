using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.ActividadModels
{
    public class CrearEditarActividadViewModel
    {
        public DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public CrearEditarActividadFormModel Form { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; }
        public IEnumerable<SelectListItem> TipoActividades { get; set; }
        private TipoActividadSelectListProvider taslp = new TipoActividadSelectListProvider();

        public CrearEditarActividadViewModel()
        {
            Form = new CrearEditarActividadFormModel();
            Categorias = db.Categorias.Where(x => x.Vigente).OrderBy(x => x.Nombre);
            TipoActividades = taslp.Provide();
        }

        public CrearEditarActividadViewModel(CrearEditarActividadFormModel form) : this()
        {
            Form = form;
        }
    }
}