using DESSAU.ControlGestion.Core;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.CategoriaModels
{
    public class ListarCategoriaViewModel
    {
        public IPagedList<Categoria> Categorias { get; set; }
        public string filtro { get; set; }
    }
}