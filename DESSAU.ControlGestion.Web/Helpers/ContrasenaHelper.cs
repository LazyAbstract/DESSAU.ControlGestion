using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Helpers
{
    public static class ContrasenaHelper
    {
        public static string getContrasena(string Nombre, string ApellidoPaterno)
        {
            string apellidoMinuscula = ApellidoPaterno.ToLower().Trim();
            string nombreMinuscula = Nombre.ToLower().Trim();
            string ApellidoLimpio = apellidoMinuscula.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace('ñ', 'n').Replace('ö', 'o');
            string NombreLimpio = nombreMinuscula.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace('ñ', 'n').Replace('ö', 'o');
            return ApellidoLimpio.Substring(0, 4) + NombreLimpio.Substring(0, 2);
        }
    }
}