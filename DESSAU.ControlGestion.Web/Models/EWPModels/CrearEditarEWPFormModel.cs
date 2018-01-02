using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EWPModels
{
    public class CrearEditarEWPFormModel : IDataErrorInfo
    {
        public int? IdEWP { get; set; }
        [Required]
        [DisplayName("Código")]
        [StringLengthAttribute(15, ErrorMessage = "El código de la EWP debe tener como máximo 15 carcateres.", MinimumLength = 2)]
        public string Codigo { get; set; }
        [Required]
        [StringLengthAttribute(255, ErrorMessage = "El nombre de la EWP debe tener como máximo 255 carcateres.", MinimumLength = 4)]
        public string Nombre { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.EWPs.Any(x => x.Codigo == Codigo && !IdEWP.HasValue))
                {
                    return "El Código ingresado ya está en uso.";
                }
                if (db.EWPs.Any(x => x.Nombre == Nombre && !IdEWP.HasValue))
                {
                    return "El Nombre ingresado ya está en uso.";
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