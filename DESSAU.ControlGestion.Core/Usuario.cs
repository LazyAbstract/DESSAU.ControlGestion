using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public partial class Usuario
    {
        public string NombreCompleto
        {
            get
            {
                return this.Nombre + " " + this.ApellidoPaterno;
            }
        }
    }
}
