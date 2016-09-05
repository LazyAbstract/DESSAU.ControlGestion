using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.EvaluacionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.MapperProfiles.EvaluacionMapper
{
    public class EvaluacionPreguntaDTOEvaluacionPregunta:Profile
    {
        protected override void Configure()
        {
            CreateMap<EvaluacionPreguntaDTO, EvaluacionPregunta>();
        }
    }
}