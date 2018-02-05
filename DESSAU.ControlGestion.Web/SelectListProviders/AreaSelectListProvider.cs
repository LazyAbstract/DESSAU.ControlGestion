using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class AreaSelectListProvider : ISelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
              .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> Areas;

        public SelectList Provide()
        {
            Areas = db.Areas
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdArea.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(Areas, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            Areas = db.Areas
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdArea.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(Areas, "Value", "Text", selected);
        }
    }
}