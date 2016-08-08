using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class ProyectoSelectListProvider : ISelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> Proyectos;

        public SelectList Provide()
        {
            Proyectos = db.Proyectos
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdProyecto.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(Proyectos, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            Proyectos = db.Proyectos
               .OrderBy(x => x.Nombre)
               .Select(x => new SelectListItem()
               {
                   Value = x.IdProyecto.ToString(),
                   Text = x.Nombre
               });
            return new SelectList(Proyectos, "Value", "Text", selected);
        }
    }
}