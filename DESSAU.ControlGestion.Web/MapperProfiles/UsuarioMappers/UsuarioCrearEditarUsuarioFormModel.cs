using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.UsuarioModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.MapperProfiles.UsuarioMappers
{
    public class UsuarioCrearEditarUsuarioFormModel : Profile
    {
        public override string ToString()
        {
            return "Usuario -> CrearEditarUsuarioFormModel";
        }

        protected override void Configure()
        {
            //base.Configure();
            Mapper.CreateMap<Usuario, CrearEditarUsuarioFormModel>();
        }
    }
}