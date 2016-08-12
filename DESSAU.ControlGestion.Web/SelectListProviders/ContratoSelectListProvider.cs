using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class ContratoSelectListProvider :ISelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
               .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> Contratos;

        public SelectList Provide()
        {
            Contratos = db.Contratos
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdContrato.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(Contratos, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            Contratos = db.Contratos
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdContrato.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(Contratos, "Value", "Text", selected);
        }
    }
}