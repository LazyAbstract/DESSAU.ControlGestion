using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ActividadModels
{
    public class CrearEditarActividadFormModel : IDataErrorInfo
    {
        public int? IdActividad { get; set; }
        [Required]
        [DisplayName("Nombre Actividad")]
        [MaxLength(255, ErrorMessage = "El nombre de la Actividad no puede superar los 255 caracteres.")]
        public string NombreActividad { get; set; }
        [Required(ErrorMessage = "Debe elegir al menos una Categoría.")]
        public List<int> IdCategorias { get; set; }

        public CrearEditarActividadFormModel()
        {
            IdCategorias = new List<int>();
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.Actividads.Any(x => x.Nombre == NombreActividad) && !IdActividad.HasValue)
                {
                    return "El nombre de la Actividad ya está en uso.";
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