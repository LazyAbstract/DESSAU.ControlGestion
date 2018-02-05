using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ReporteModels.ReporteEWPModels
{
    public class ExportaEWPViewModel
    {
        public DateTime Fecha { get; set; }
        public string NombreProfesional { get; set; }
        public string NombreEWP { get; set; }
        public string CodigoEWP { get; set; }
        public string CodigoSubEWP { get; set; }
        public string TipoActividad { get; set; }
        public string Actividad { get; set; }
        public int HorasDeclaradas { get; set; }
        public string NumeroDocumento { get; set; }
    }
}