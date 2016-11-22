using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class EvaluacionContrato
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public double PromedioDesempeno
        {
            get
            {
                return db.Evaluacions.Where(x => x.FechaEvaluacion == this.FechaEvaluacion).Average(x => x.Promedio).GetValueOrDefault(0) / 5;
            }
        }

        public double PromedioContrato
        {
            get
            {
                return this.EvaluacionContratoPreguntas.Average(x => x.ValorObtenido) / 5;
            }
        }

        public string ApreciacionGlobal
        {
            get
            {
                //= SI(P33 = 0; ""; SI(P33 > 0; SI(P33 < 2; "Deficiente"; SI(P33 <= 2, 5; "Mejorable"; SI(P33 <= 3, 5; "Satisfactorio"; SI(P33 <= 4, 5; "Notable"; SI(P33 <= 5; "Excepcional")))))))
                if (Promedio.GetValueOrDefault(0) > 0 && Promedio.GetValueOrDefault(0) < 2)
                    return "Deficiente";
                else if (Promedio.GetValueOrDefault(0) <= 2.5)
                    return "Mejorable";
                else if (Promedio.GetValueOrDefault(0) <= 3.5)
                    return "Satisfactorio";
                else if (Promedio.GetValueOrDefault(0) <= 4)
                    return "Notable";
                else if (Promedio.GetValueOrDefault(0) <= 5)
                    return "Excepcional";
                return String.Empty;
            }
        }

        public double FactorDesempeno
        {
            get
            {
                if (PromedioPorcentual < 0.5)
                    return 0.9;
                else if (PromedioPorcentual < 0.6)
                    return 0.92;
                else if (PromedioPorcentual < 0.7)
                    return 0.93;
                else if (PromedioPorcentual < 0.75)
                    return 0.95;
                else if (PromedioPorcentual < 0.8)
                    return 0.96;
                else if (PromedioPorcentual < 0.85)
                    return 0.97;
                else if (PromedioPorcentual < 0.9)
                    return 0.98;
                else if (PromedioPorcentual < 0.95)
                    return 0.99;
                else return 1;
            }
        }

        public double PromedioPorcentual
        {
            get
            {
                return this.PromedioDesempeno * 0.7 + this.PromedioContrato * 0.3;
            }
        }
    }
}
