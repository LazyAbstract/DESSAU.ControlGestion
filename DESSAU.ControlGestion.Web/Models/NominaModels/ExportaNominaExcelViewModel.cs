using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.NominaModels
{
    public class ExportaNominaExcelViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [DisplayName("Categoría")]
        public string Categoria { get; set; }
        [DisplayName("Orde de Servicio")]
        public string ODS { get; set; }
        //[DisplayName("Estado Planificación")]
        //public string EstadoPlanificacion { get; set; }
        //[DisplayName("Estado Declaración")]
        //public string EstadoDeclaracion { get; set; }
    }
}