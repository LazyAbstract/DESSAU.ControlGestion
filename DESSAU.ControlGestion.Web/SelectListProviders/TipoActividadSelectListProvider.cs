using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class TipoActividadSelectListProvider : ISelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> TipoActividades;

        public SelectList Provide()
        {
            TipoActividades = db.TipoActividads
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdTipoActividad.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(TipoActividades, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            TipoActividades = db.TipoActividads
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdTipoActividad.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(TipoActividades, "Value", "Text", selected);
        }
    }
}