using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.NumeroDocumentoModels
{
    public class CrearEditarNumeroDocumentoFormModel : IDataErrorInfo
    {
        public int? IdNumeroDocumento { get; set; }
        [Required]
        [DisplayName("Tipo Documento")]
        public int? IdTipoDocumento { get; set; }
        [Required]
        [DisplayName("Código")]
        [StringLength(63, ErrorMessage = "El código debe tener cómo máximo 63 caractéres y 2 caractéres cómo mínimo.", MinimumLength = 2)]
        public string Codigo { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "El nombre debe tener cómo máximo 255 caractéres y 2 caractéres cómo mínimo..", MinimumLength = 2)]
        public string Nombre { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.NumeroDocumentos.Any(x => x.Codigo == Codigo && !IdNumeroDocumento.HasValue))
                {
                    return "El Código ingresado ya está en uso.";
                }
                if (db.NumeroDocumentos.Any(x => x.Nombre == Nombre && !IdNumeroDocumento.HasValue))
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