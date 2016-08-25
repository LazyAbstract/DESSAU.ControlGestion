using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public static class EstadoUsuario
    {
        public static bool PlanificacionOk(DESSAUControlGestionDataContext db, int IdUsuario)
        {
            bool response = true;
            if(db.sp_GetDiasPendientesPlanificacionByIdUsuario(IdUsuario).Any())
            {
                response = false;
            }
            return response;
        }

        public static bool DeclaracionOk(DESSAUControlGestionDataContext db, int IdUsuario)
        {
            bool response = true;
            if (db.sp_GetDiasPendientesDeclaracionByIdUsuario(IdUsuario).Any())
            {
                response = false;
            }
            return response;
        }
    }
}
