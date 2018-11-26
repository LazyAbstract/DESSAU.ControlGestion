using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Helpers
{
    public class TurnoHorarioHelper
    {
        public int HorasLaborablesSemana { get; set; }
        public int HorasLaborablesViernes { get; set; }
        public int HorarioDiaEspecial { get; set; }  //  un 24 de disiembre que se trabaje hasta las 12:00 hrs. por ejemplo
        public bool TrabajaFinesDeSemana { get; set; }
        public bool TrbajaFeriados { get; set; }

        public TurnoHorarioHelper()
        {
            HorasLaborablesSemana = 10;
            HorasLaborablesViernes = 5;
            TrabajaFinesDeSemana = false;
            TrbajaFeriados = false;
            HorarioDiaEspecial = 0;
        }
    }
}