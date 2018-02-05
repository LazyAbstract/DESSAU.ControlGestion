using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EWPModels
{
    public class CrearEditarSubEWPFormModel : IDataErrorInfo
    {
        public int? IdSubEWP { get; set; }
        [Required]
        [DisplayName("EWP")]
        public int IdEWP { get; set; }
        [Required]
        [DisplayName("Código")]
        [StringLength(15, ErrorMessage = "El código de la Sub EWP debe tener como máximo 255 carcateres.", MinimumLength = 2)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "El Nombre de la Sub EWP debe tener como máximo 255 carcateres.", MinimumLength = 2)]
        public string Nombre { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.SubEWPs.Any(x => x.Codigo == Codigo && !IdSubEWP.HasValue))
                {
                    return "El Código ingresado ya está en uso.";
                }

                if (db.SubEWPs.Any(x => x.Nombre == Nombre && !IdSubEWP.HasValue))
                {
                    return "El Código ingresado ya está en uso.";
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