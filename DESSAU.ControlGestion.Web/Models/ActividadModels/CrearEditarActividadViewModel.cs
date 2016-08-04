using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ActividadModels
{
    public class CrearEditarActividadViewModel
    {
        public DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public CrearEditarActividadFormModel Form { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; }

        public CrearEditarActividadViewModel()
        {
            Form = new CrearEditarActividadFormModel();
            Categorias = db.Categorias.OrderBy(x => x.Nombre);
        }

        public CrearEditarActividadViewModel(CrearEditarActividadFormModel form) : this()
        {
            Form = form;
        }
    }
}