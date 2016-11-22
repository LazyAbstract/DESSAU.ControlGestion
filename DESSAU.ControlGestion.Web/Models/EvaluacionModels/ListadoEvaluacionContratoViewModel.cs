using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class ListadoEvaluacionContratoViewModel
    {
        public IEnumerable<EvaluacionContrato> Evaluaciones { get; set; }
        public IEnumerable<Evaluacion> Desempeno { get; set; }

        public IEnumerable<Proyecto> Proyectos { get; set; }

        public List<DateTime> Periodos { get; set; }


    }
}