using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.CategoriaModels
{
    public class AgregarActividadCategoriaFormModel : IDataErrorInfo
    {
        public int IdCategoria { get; set; }
        [Required]
        [DisplayName("Actividad")]
        public int IdActividad { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.CategoriaActividads.Any(x => x.IdCategoria == IdCategoria && x.IdActividad == IdActividad && x.Vigente))
                {
                    return "La Actividad ya está asociada a la Disciplina.";
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