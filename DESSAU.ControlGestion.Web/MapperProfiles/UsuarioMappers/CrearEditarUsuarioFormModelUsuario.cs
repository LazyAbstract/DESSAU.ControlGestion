using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.UsuarioModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.MapperProfiles.UsuarioMappers
{
    public class CrearEditarUsuarioFormModelUsuario : Profile
    {
        public override string ToString()
        {
            return "CrearEditarUsuarioFormModel -> Usuario";
        }

        protected override void Configure()
        {
            //base.Configure();
            Mapper.CreateMap<CrearEditarUsuarioFormModel, Usuario>();
        }
    }
}