using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class Evaluacion
    {
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
    }
}
