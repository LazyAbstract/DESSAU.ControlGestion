using DESSAU.ControlGestion.Core;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ActividadModels
{
    public class ListarActividadViewModel
    {
        public IPagedList<Actividad> Actividades { get; set; }
        public string filtro { get; set; }
    }
}