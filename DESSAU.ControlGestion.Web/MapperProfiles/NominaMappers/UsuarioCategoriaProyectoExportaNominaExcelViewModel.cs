using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.NominaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.MapperProfiles.NominaMappers
{
    public class UsuarioCategoriaProyectoExportaNominaExcelViewModel : Profile
    {
      
        public override string ToString()
        {
            return "UsuarioCategoriaProyecto -> ExportaNominaExcelViewModel";
        }

        protected override void Configure()
        {
            //base.Configure();
            Mapper.CreateMap<UsuarioCategoriaProyecto, ExportaNominaExcelViewModel>()
                .ForMember(d => d.Nombre, prop => prop.MapFrom(x => x.Usuario.Nombre))
                .ForMember(d => d.Apellido, prop => prop.MapFrom(x => x.Usuario.ApellidoPaterno))
                .ForMember(d => d.Categoria, prop => prop.MapFrom(x => x.Categoria.Nombre))
                .ForMember(d => d.ODS, prop => prop.MapFrom(x => x.Proyecto.Nombre));
                //.ForMember(d => d.EstadoPlanificacion, prop => prop.MapFrom(x => x.EstadoPlanificacionDeclaracions));
        }

    }
}