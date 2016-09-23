using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class Usuario
    {
        public string NombreCompleto
        {
            get
            {
                return this.Nombre + " " + this.ApellidoPaterno;
            }
        }

        public string ApellidoNombre
        {
            get
            {
                return this.ApellidoPaterno + ", " + this.Nombre;
            }
        }

        public string Contrasena
        {
            get
            {
                string apellidoMinuscula = this.ApellidoPaterno.ToLower().Trim();
                string nombreMinuscula = this.Nombre.ToLower().Trim();
                string ApellidoLimpio = apellidoMinuscula.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace('ñ', 'n').Replace('ö', 'o');
                string NombreLimpio = nombreMinuscula.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace('ñ', 'n').Replace('ö', 'o');
                return ApellidoLimpio.Substring(0, 4) + NombreLimpio.Substring(0, 2);
            }
        }
    }
}
