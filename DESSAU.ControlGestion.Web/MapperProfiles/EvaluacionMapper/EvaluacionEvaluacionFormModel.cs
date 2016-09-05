using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.EvaluacionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.MapperProfiles.EvaluacionMapper
{
    public class EvaluacionEvaluacionFormModel:Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<EvaluacionFormModel,Evaluacion>()
                .ForMember(x=>x.EvaluacionPreguntas, prop=> prop.MapFrom(y => y.EvaluacionPreguntaDTOs) );
        }
    }
}