using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.RevisionModels
{
    public class CrearEditarRevisionFormModel : IDataErrorInfo
    {
        public int? IdRevision { get; set; }
        [Required]
        [DisplayName("Nombre Revisión")]
        [StringLength(63, ErrorMessage = "El Nombre de la Revión debe tener cómo máximo 63 caractéres y 2 caractéres cómo mínimo.", MinimumLength = 2)]
        public string Nombre { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.Revisions.Any(x => x.Nombre == Nombre && !IdRevision.HasValue))
                {
                    return "El nombre ingresado ya está en uso.";
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