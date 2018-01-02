using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class SubEWPSelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
             .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> Resultados;

        public SelectList Provide()
        {
            Resultados = db.SubEWPs
                .OrderBy(x => x.EWP.Codigo)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdSubEWP.ToString(),
                    Text = x.Codigo
                });
            return new SelectList(Resultados, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            Resultados = db.SubEWPs
                .OrderBy(x => x.EWP.Codigo)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdSubEWP.ToString(),
                    Text = x.Codigo
                });
            return new SelectList(Resultados, "Value", "Text", selected);
        }
    }
}