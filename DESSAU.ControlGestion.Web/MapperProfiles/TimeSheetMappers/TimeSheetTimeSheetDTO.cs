using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.TimeSheetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.MapperProfiles.TimeSheetMappers
{
    public class TimeSheetTimeSheetDTO : Profile
    {
        public override string ToString()
        {
            return "TimeSheet -> TimeSheetDTO";
        }
        protected override void Configure()
        {
            Mapper.CreateMap<TimeSheet, TimeSheetDTO>();
            Mapper.CreateMap<IEnumerable<TimeSheet>, IEnumerable<TimeSheetDTO>>();
            Mapper.CreateMap<IQueryable<TimeSheet>, IEnumerable<TimeSheetDTO>>();
        }
    }
}