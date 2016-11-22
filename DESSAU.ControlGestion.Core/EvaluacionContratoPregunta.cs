using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class EvaluacionContratoPregunta
    {
        public double ValorObtenidoPorcentual
        {
            get
            {
                return (double)this.ValorObtenido / 5;
            }
        }
    }
}
