using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class ListadoEvaluacionesDesempenoViewModel
    {
        public ListadoEvaluacionesDesempenoFormModel Form { get; set; }
        public IEnumerable<SelectListItem> Proyectos { get; set; }
        private ProyectoSelectListProvider pslp = new ProyectoSelectListProvider();
        public IEnumerable<Evaluacion> Evaluaciones { get; set; }
        public IEnumerable<UsuarioCategoriaProyecto> NoEvaluados { get; set; }

        public ListadoEvaluacionesDesempenoViewModel()
        {
            Form = new ListadoEvaluacionesDesempenoFormModel();
            Proyectos = pslp.Provide();
        }
    }
}