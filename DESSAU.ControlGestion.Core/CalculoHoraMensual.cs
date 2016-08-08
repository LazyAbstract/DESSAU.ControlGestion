using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public class CalculoHoraMensual
    {
        private DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        private DateTime FechaCalculo { get; set; }
        private int _IdTipoTimeSheet { get; set; }
        private int _IdUsuario { get; set; }
        public int HorasPlanificacion
        {
            get
            {
                return HorasLaborales(FechaCalculo);
            }
        }
        public int HorasDeclaracion
        {
            get
            {
                return HorasLaborales(FechaCalculo);
            }
        }
        public CalculoHoraMensual(int IdUsuario, int IdTipoTimeSheet)
        {
            FechaCalculo = DateTime.Now;
            _IdTipoTimeSheet = IdTipoTimeSheet;
            _IdUsuario = IdUsuario;
        }
        public CalculoHoraMensual(int IdUsuario, int IdTipoTimeSheet, DateTime fecha)
        {
            FechaCalculo = fecha;
            _IdTipoTimeSheet = IdTipoTimeSheet;
            _IdUsuario = IdUsuario;
        }

        public int HorasLaborales(DateTime FechaCalculo)
        {
            int response = 0;
            DateTime Primero = new DateTime(FechaCalculo.Year, FechaCalculo.Month, 1);
            DateTime FinMes = (new DateTime(FechaCalculo.Year, FechaCalculo.Month, 1)).AddMonths(1).AddDays(-1);
            int Dias = (FinMes - Primero).Days;
            DateTime contador = Primero;
            for(int i = 0; i <= Dias; i++)
            {
                DiaEspecial DiaEspecial = db.DiaEspecials.SingleOrDefault(x => x.Fecha == contador);
                if(DiaEspecial != null && DiaEspecial.Fecha == contador)
                {
                    response += DiaEspecial.Horas;
                }
                else if(contador.DayOfWeek == DayOfWeek.Monday || contador.DayOfWeek == DayOfWeek.Tuesday || contador.DayOfWeek == DayOfWeek.Wednesday || contador.DayOfWeek == DayOfWeek.Thursday)
                {
                    response += 10;
                }
                else if(contador.DayOfWeek == DayOfWeek.Friday)
                {
                    response += 5;
                }
                contador = contador.AddDays(1);
            }
            return response;
        }

        public int CalculoHoraTotal
        {
            get
            {
                int respose = 0;
                if (this._IdTipoTimeSheet == TipoTimeSheet.Planificacion) respose = HorasPlanificacion;
                else if (this._IdTipoTimeSheet == TipoTimeSheet.Reportado) respose = HorasDeclaracion;
                return respose;
            }
            
        }

        public int HorasIngresadas
        {
            get
            {
                int response = 0;
                IQueryable<TimeSheet> timeSheets = db.TimeSheets
                    .Where(x => x.UsuarioCategoriaProyecto.IdUsuario == _IdUsuario
                        && x.UsuarioCategoriaProyecto.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto
                            != TipoEstadoUsuarioCategoriaProyecto.NoVigente);
                if (_IdTipoTimeSheet == TipoTimeSheet.Planificacion)
                {
                    timeSheets = timeSheets.Where(x => x.Fecha.Month == FechaCalculo.Month && x.Fecha.Year == FechaCalculo.Year);
                    response = timeSheets.Any() ? timeSheets.Sum(x => x.HorasPlanificadas) : 0;
                }
                else
                {
                    timeSheets = timeSheets.Where(x => x.Fecha.Month == FechaCalculo.Month && x.Fecha.Year == FechaCalculo.Year);
                    response = timeSheets.Any() ? timeSheets.Sum(x => x.HorasReportadas.GetValueOrDefault(0)) : 0;
                }
                return response;
            }
           
        }

        public int Porcentaje
        {
            get
            {
                return (100) * this.HorasIngresadas / this.CalculoHoraTotal;
            }            
        }

    }
}
