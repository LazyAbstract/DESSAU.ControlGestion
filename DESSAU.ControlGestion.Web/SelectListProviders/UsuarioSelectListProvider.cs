using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    public class UsuarioSelectListProvider
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                .WithConnectionStringFromConfiguration();
        private IEnumerable<SelectListItem> Usuarios;

        public SelectList Provide()
        {
            Usuarios = db.Usuarios
                .Where(x => x.Vigente)
                .OrderBy(x => x.ApellidoPaterno)                
                .Select(x => new SelectListItem()
                {
                    Value = x.IdUsuario.ToString(),
                    Text = x.ApellidoNombre
                });
            return new SelectList(Usuarios, "Value", "Text");
        }

        public SelectList Provide(object selected)
        {
            Usuarios = db.Usuarios
                .Where(x => x.Vigente)
                .OrderBy(x => x.ApellidoPaterno)
                .Select(x => new SelectListItem()
                {
                    Value = x.IdUsuario.ToString(),
                    Text = x.ApellidoNombre
                });
            return new SelectList(Usuarios, "Value", "Text", selected);
        }
    }
}