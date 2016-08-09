using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ReporteModels
{
    public class DashboardFormModel
    {
        public int? IdProyecto { get; set; }
        public string Periodo { get; set; }
        public DateTime Fecha { get; set; }
    }
}