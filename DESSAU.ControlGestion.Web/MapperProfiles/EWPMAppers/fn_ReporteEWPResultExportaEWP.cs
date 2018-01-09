using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.ReporteModels.ReporteEWPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.MapperProfiles.EWPMAppers
{
    public class fn_ReporteEWPResultExportaEWP : Profile
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public override string ToString()
        {
            return "fn_ReportePorEWPResult -> ExportaEWPViewModel";
        }

        protected override void Configure()
        {
            //base.Configure();
            Mapper.CreateMap<fn_ReportePorEWPResult, ExportaEWPViewModel>()
                .ForMember(d => d.HorasDeclaradas, prop => prop.MapFrom(x => x.HorasReportadas))               
                .ForMember(d => d.NombreEWP, prop => prop.MapFrom(x => x.EWP))
                .ForMember(d => d.NombreProfesional, prop => prop.MapFrom(x => db.Usuarios.Single(y => y.IdUsuario == x.IdUsuario).ApellidoNombre));
        }
    }
}