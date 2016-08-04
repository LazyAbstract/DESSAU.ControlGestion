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
                string ApellidoLimpio = this.ApellidoPaterno.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace('ñ', 'n');
                string NombreLimpio = this.Nombre.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace('ñ', 'n');
                return ApellidoLimpio.ToLower().Substring(0, 4) + NombreLimpio.ToLower().Substring(0, 2);
            }
        }
    }
}
