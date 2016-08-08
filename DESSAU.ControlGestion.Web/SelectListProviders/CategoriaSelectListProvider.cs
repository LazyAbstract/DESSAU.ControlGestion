using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class CategoriaSelectListProvider : ISelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
               .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> Categorias;

        public SelectList Provide()
        {
            Categorias = db.Categorias
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdCategoria.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(Categorias, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            Categorias = db.Categorias
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdCategoria.ToString(),
                    Text = x.Nombre
                });
            return new SelectList(Categorias, "Value", "Text", selected);
        }
    }
}