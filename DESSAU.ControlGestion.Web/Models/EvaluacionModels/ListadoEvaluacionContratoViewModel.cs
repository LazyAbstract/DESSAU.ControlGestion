﻿using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class ListadoEvaluacionContratoViewModel
    {
        public IEnumerable<EvaluacionContrato> Evaluaciones { get; set; }
    }
}