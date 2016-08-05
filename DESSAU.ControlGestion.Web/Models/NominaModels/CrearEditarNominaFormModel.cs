using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.NominaModels
{
    public class CrearEditarNominaFormModel
    {
        [DisplayName("Profesional")]
        public int IdUsuario { get; set; }
        [Required]
        [DisplayName("Orden de Servicio")]
        public int IdProyecto { get; set; }
        [Required]
        [DisplayName("Categoría")]
        public int IdCategoria { get; set; }
    }
}