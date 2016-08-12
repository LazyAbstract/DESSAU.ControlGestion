using DESSAU.ControlGestion.Core;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ProyectoModels
{
    public class ListarProyectoViewModel
    {
        public IPagedList<Proyecto> OrdenesServicio { get; set; }
        public string filtro { get; set; }
    }
}