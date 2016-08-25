using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class UsuarioCategoriaProyecto
    {
        private DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();

        public bool PlanificacionOk
        {
            get
            {
                bool response = true;
                if (db.sp_GetDiasPendientesPlanificacionByIdUsuario(this.IdUsuario).Any()) response = false;
                return response;
            }            
        }

        public bool DeclaracionOk
        {
            get
            {
                bool response = true;
                if (db.sp_GetDiasPendientesDeclaracionByIdUsuario(this.IdUsuario).Any()) response = false;
                return response;
            }            
        }

        public int HorasPlanificadas(DateTime Fecha)
        {
            int response = 0;
            int? horasReportadas = this.TimeSheets
                .Where(x => x.Fecha.Month == Fecha.Month && x.Fecha.Year == Fecha.Year).Sum(x => x.HorasPlanificadas);
            if (horasReportadas.HasValue) response = horasReportadas.Value;
            return response;
        }

        public int HorasReportadas(DateTime Fecha)
        {
            int response = 0;
            int? horasReportadas = this.TimeSheets
                .Where(x => x.Fecha.Month == Fecha.Month && x.Fecha.Year == Fecha.Year).Sum(x => x.HorasReportadas.GetValueOrDefault(0));
            if (horasReportadas.HasValue) response = horasReportadas.Value;
            return response;
        }
    }
}
