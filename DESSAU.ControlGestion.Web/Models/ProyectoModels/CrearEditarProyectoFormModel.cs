using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ProyectoModels
{
    public class CrearEditarProyectoFormModel : IDataErrorInfo
    {
        public int? IdProyecto { get; set; }
        [Required]
        [DisplayName("Contrato")]
        public int IdContrato { get; set; }
        [Required]
        [DisplayName("Nombre Orden de Servicio")]
        [StringLength(255, ErrorMessage = "El nombre de la ODS debe tener como mínimo 4 y como máximo 255 caracteres.", MinimumLength = 4)]
        public string Nombre { get; set; }
        [Required]
        [DisplayName("Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DisplayName("Fecha de Término")]
        public DateTime FechaFin { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.Proyectos.Any(x => x.Nombre == Nombre) && !IdProyecto.HasValue)
                {
                    return "El nombre de la ODS ya está en uso.";
                }
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get { return string.Empty; }
        }

        #endregion

    }
}