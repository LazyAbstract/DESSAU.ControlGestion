using DESSAU.ControlGestion.Core;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.UsuarioModels
{
    public class ListarUsuarioViewModel
    {
        public IPagedList<Usuario> Usuarios { get; set; }
        public string filtro { get; set; }
    }
}