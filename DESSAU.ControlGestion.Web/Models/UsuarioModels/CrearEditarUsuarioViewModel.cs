﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DESSAU.ControlGestion.Core;

namespace DESSAU.ControlGestion.Web.Models.UsuarioModels
{
    public class CrearEditarUsuarioViewModel
    {
        public CrearEditarUsuarioFormModel Form { get; set; }
        public IEnumerable<TipoUsuario> TiposUsuario { get; set; }

        public CrearEditarUsuarioViewModel()
        {
            Form = new CrearEditarUsuarioFormModel();
            Form.CreacionUsuario = true;

        }

        public CrearEditarUsuarioViewModel(CrearEditarUsuarioFormModel F)
            : this()
        {
            Form = F;
            Form.CreacionUsuario = true;
        }
    }
}