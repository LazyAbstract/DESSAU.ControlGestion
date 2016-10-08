using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ReporteModels
{
    public class ReporteDedicacionActividadFormModel
    {
        public int? IdUsuarioCategoriaProyecto { get; set; }
        public int? IdProyecto { get; set; }
        public int? IdUsuario { get; set; }
        //public int? Ano { get; set; }
        //public int? Mes { get; set; }

        public string Periodo { get; set; }
        public DateTime? Fecha { get; set; }
    }
}