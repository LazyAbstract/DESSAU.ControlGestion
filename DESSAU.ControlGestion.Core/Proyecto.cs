using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class Proyecto
    {
        public string NombreCompleto
        {
            get
            {
                return String.Format("{0}, {1}, {2}",Nombre, Contrato.Nombre, Contrato.Cliente.Nombre);
            }
        }
    }
}
