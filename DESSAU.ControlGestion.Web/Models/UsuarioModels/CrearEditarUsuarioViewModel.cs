using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DESSAU.ControlGestion.Core;
using System.Web.Mvc;
using DESSAU.ControlGestion.Web.SelectListProviders;

namespace DESSAU.ControlGestion.Web.Models.UsuarioModels
{
    public class CrearEditarUsuarioViewModel
    {
        public CrearEditarUsuarioFormModel Form { get; set; }
        public IEnumerable<TipoUsuario> TiposUsuario { get; set; }
        public IEnumerable<SelectListItem> Areas { get; set; }
        public IEnumerable<SelectListItem> Disciplinas { get; set; }
        public IEnumerable<SelectListItem> ODSs { get; set; }
        CategoriaSelectListProvider cslp = new CategoriaSelectListProvider();
        ProyectoSelectListProvider pslp = new ProyectoSelectListProvider();
        AreaSelectListProvider aslp = new AreaSelectListProvider();

        public CrearEditarUsuarioViewModel()
        {
            Form = new CrearEditarUsuarioFormModel();
            Form.CreacionUsuario = true;
            Disciplinas = cslp.Provide();
            ODSs = pslp.Provide();
            Areas = aslp.Provide();

        }

        public CrearEditarUsuarioViewModel(CrearEditarUsuarioFormModel F)
            : this()
        {
            Form = F;
            Form.CreacionUsuario = true;
        }
    }
}