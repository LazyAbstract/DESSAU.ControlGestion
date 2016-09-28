using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class ListadoEvaluacionesDesempenoViewModel
    {
        public string Periodo { get; set; }

        public IEnumerable<Evaluacion> Evaluaciones { get; set; }

        public IEnumerable<UsuarioCategoriaProyecto> UCPs { get; set; }
    }
}