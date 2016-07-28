using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.TimeSheetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.MapperProfiles.TimeSheetMappers
{
    public class TimeSheetDTOTimeSheet : Profile
    {
        public override string ToString()
        {
            return "TimeSheetDTO -> TimeSheet";
        }

        protected override void Configure()
        {
            Mapper.CreateMap<TimeSheetDTO, TimeSheet>();
        }
    }
}