using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class UsuarioCategoriaProyecto
    {
        private CalculoHoraMensual calc { get; set; }

        public bool PlanificacionOk(DateTime Fecha)
        {
            bool response = false;
            calc = new CalculoHoraMensual(this.IdUsuario, TipoTimeSheet.Planificacion, Fecha);
            int? horasPlanificadas = this.TimeSheets
                .Where(x => x.Fecha.Month == Fecha.Month && x.Fecha.Year == Fecha.Year).Sum(x => x.HorasPlanificadas);
            if (horasPlanificadas.HasValue && horasPlanificadas.Value >= calc.CalculoHoraTotal) response = true;
            return response;
        }

        public bool DeclaracionOk(DateTime Fecha)
        {
            bool response = false;
            calc = new CalculoHoraMensual(this.IdUsuario, TipoTimeSheet.Reportado, Fecha);
            int? horasReportadas = this.TimeSheets
                .Where(x => x.Fecha.Month == Fecha.Month && x.Fecha.Year == Fecha.Year).Sum(x => x.HorasReportadas.GetValueOrDefault(0));
            if (horasReportadas.HasValue && horasReportadas.Value >= calc.CalculoHoraTotal) response = true;
            return response;
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
