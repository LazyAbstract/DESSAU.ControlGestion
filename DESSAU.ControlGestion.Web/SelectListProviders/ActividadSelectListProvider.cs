using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class ActividadSelectListProvider : ISelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
               .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> resultados;

        public SelectList Provide()
        {
            resultados = db.Actividads
                .OrderBy(x => x.Nombre)
                .Where(x => x.Vigente)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdActividad.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(resultados, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            resultados = db.Actividads
                .OrderBy(x => x.Nombre)
                .Where(x => x.Vigente)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdActividad.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(resultados, "Value", "Text", selected);
        }
    }
}