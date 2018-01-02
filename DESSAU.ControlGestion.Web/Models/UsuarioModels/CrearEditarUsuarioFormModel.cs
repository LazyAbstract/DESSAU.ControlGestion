using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.UsuarioModels
{
    public class CrearEditarUsuarioFormModel : IDataErrorInfo
    {
        public int? IdUsuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [DisplayName("Apellido Paterno")]
        public string ApellidoPaterno { get; set; }
        [Required]
        [DisplayName("Nombre Usuario")]
        public string Correo { get; set; }
        [Required]
        [DisplayName("Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Correo Inválido")]
        public string Contacto { get; set; }
        [Required(ErrorMessage = "Debe elegir al menos un Tipo de usuario.")]
        public int IdTipoUsuario { get; set; }
        public bool CreacionUsuario { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                    .WithConnectionStringFromConfiguration();
                if (db.Usuarios.Any(x => x.Nombre == Correo && CreacionUsuario))
                {
                    return "El Correo ingresado ya está en uso como nombre de usuario.";
                }
                if (String.IsNullOrEmpty(Correo))
                {
                    return "El Correo no puede ser vacío";
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