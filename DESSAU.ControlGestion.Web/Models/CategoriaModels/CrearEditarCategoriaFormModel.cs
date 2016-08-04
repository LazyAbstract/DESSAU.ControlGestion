using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.CategoriaModels
{
    public class CrearEditarCategoriaFormModel : IDataErrorInfo
    {
        public int? IdCategoria { get; set; }
        [Required]
        [DisplayName("Nombre Categoría")]
        [MaxLength(255, ErrorMessage = "El nombre de la Categoría no puede superar los 255 caracteres.")]
        public string NombreCategoria { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.Actividads.Any(x => x.Nombre == NombreCategoria) && !IdCategoria.HasValue)
                {
                    return "El nombre de la Categoría ya está en uso.";
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