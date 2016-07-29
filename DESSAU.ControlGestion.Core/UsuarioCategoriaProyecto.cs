using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class UsuarioCategoriaProyecto
    {
        public bool PlanificacionOk
        {
            get
            {
                bool response = true;
                EstadoPlanificacionDeclaracion epd = this.EstadoPlanificacionDeclaracions.OrderByDescending(x => x.Ano).ThenBy(x => x.Mes).FirstOrDefault();
                if (epd != null) response = epd.PlanificacionOk;
                return response;
            }
        }

        public bool DeclaracionOk
        {
            get
            {
                bool response = true;
                EstadoPlanificacionDeclaracion epd = this.EstadoPlanificacionDeclaracions.OrderByDescending(x => x.Ano).ThenBy(x => x.Mes).FirstOrDefault();
                if (epd != null) response = epd.DeclaracionOk;
                return response;
            }
        }
    }
}
