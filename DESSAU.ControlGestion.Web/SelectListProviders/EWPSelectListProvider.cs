using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class EWPSelectListProvider : ISelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
              .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> Resultados;

        public SelectList Provide()
        {
            Resultados = db.EWPs
                .OrderBy(x => x.Codigo)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdEWP.ToString(),
                    Text = x.Codigo + " - " + x.Nombre
                });
            return new SelectList(Resultados, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            Resultados = db.EWPs
                .OrderBy(x => x.Codigo)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdEWP.ToString(),
                    Text = x.Codigo + " - " + x.Nombre
                });
            return new SelectList(Resultados, "Value", "Text", selected);
        }
    }
}