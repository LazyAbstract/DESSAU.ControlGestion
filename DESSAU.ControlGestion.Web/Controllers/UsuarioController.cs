using AutoMapper;
using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models;
using DESSAU.ControlGestion.Web.Models.UsuarioModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuarioController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UsuarioController()
        {
        }

        public UsuarioController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult ListarUsuario(int? pagina, string filtro)
        {
            ListarUsuarioViewModel Model = new ListarUsuarioViewModel();
            Model.filtro = filtro;
            IEnumerable<Usuario> Users = db.Usuarios.OrderByDescending(x => x.Vigente)
                .ThenBy(x => x.ApellidoPaterno);
            if (!String.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                Users = db.Usuarios
                    .Where(x => x.Nombre.ToLower().Contains(filtro)
                        || x.Nombre.ToLower().Contains(filtro)
                        || x.ApellidoPaterno.ToLower().Contains(filtro)
                        || x.Correo.ToLower().Contains(filtro));
            }
            Model.Usuarios = Users.ToPagedList(pagina ?? 1, 10);
            return View(Model);
        }

        public ActionResult CrearEditarUsuario(int? IdUsuario)
        {
            CrearEditarUsuarioViewModel Model = new CrearEditarUsuarioViewModel();
            Model.TiposUsuario = db.TipoUsuarios.OrderBy(x => x.Nombre);
            if (IdUsuario.HasValue)
            {
                Model.Form.CreacionUsuario = false;
                Usuario user = db.Usuarios.Single(x => x.IdUsuario == IdUsuario.Value);
                Mapper.Map<Usuario, CrearEditarUsuarioFormModel>(user, Model.Form);
                Model.Form.IdTipoUsuario = user.IdTipoUsuario;
            }
            return View(Model);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearEditarUsuario(CrearEditarUsuarioFormModel Form)
        {
            if (ModelState.IsValid)
            {
                if (Form.IdUsuario.HasValue)
                {
                    Usuario _user = db.Usuarios.Single(x => x.IdUsuario == Form.IdUsuario);
                    int AntigupoTipoUsuario = _user.IdTipoUsuario;
                    _user.Nombre = Form.Nombre;
                    _user.ApellidoPaterno = Form.ApellidoPaterno;
                    _user.IdTipoUsuario = Form.IdTipoUsuario;
                    _user.Contacto = Form.Contacto;

                    if (AntigupoTipoUsuario != Form.IdTipoUsuario)
                    {
                        var user = UserManager.FindByName(_user.Correo);
                        var roles = UserManager.GetRoles(user.Id);

                        foreach (var rol in db.TipoUsuarioPermisos.Where(x => x.IdTipoUsuario == AntigupoTipoUsuario).Select(x => x.Permiso.Nombre))
                        {
                            UserManager.RemoveFromRole(user.Id, rol);
                        }

                        foreach (var permiso in _user.TipoUsuario.TipoUsuarioPermisos.Select(x => x.Permiso.Nombre))
                        {
                            var roleresult = UserManager.AddToRole(user.Id, permiso);
                        }
                    }

                    db.SubmitChanges();
                    Mensaje = "El usuario fue editado exitosamente";
                    return RedirectToAction("ListarUsuario");
                }
                else
                {
                    string Password = Form.ApellidoPaterno.ToLower().Substring(0, 4) + Form.Nombre.ToLower().Substring(0, 2);
                    var user = new ApplicationUser { UserName = Form.Correo, Email = Form.Correo };
                    var result = await UserManager.CreateAsync(user, Password);
                    if (result.Succeeded)
                    {
                        foreach (var permiso in db.TipoUsuarioPermisos.Where(x => x.IdTipoUsuario == Form.IdTipoUsuario).Select(x => x.Permiso.Nombre))
                        {
                            var roleresult = UserManager.AddToRole(user.Id, permiso);
                        }

                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        Usuario _user = new Usuario()
                        {
                            IdTipoUsuario = Form.IdTipoUsuario,
                            Correo = Form.Correo,
                            Nombre = Form.Nombre,
                            ApellidoPaterno = Form.ApellidoPaterno,
                            Contacto = Form.Contacto
                        };

                        db.Usuarios.InsertOnSubmit(_user);
                        db.SubmitChanges();
                        Mensaje = "El Usuario fue creado exitosamente.";
                    }
                    //AddErrors(result);
                    return RedirectToAction("ListarUsuario");
                }
            }
            CrearEditarUsuarioViewModel Model = new CrearEditarUsuarioViewModel(Form);
            Model.TiposUsuario = db.TipoUsuarios.OrderBy(x => x.Nombre);
            if (Form.IdUsuario.HasValue) Model.Form.CreacionUsuario = false;
            return View(Model);
        }

        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetContrasena(int IdUsuario)
        {
            Usuario _user = db.Usuarios.Single(x => x.IdUsuario == IdUsuario);
            var user = await UserManager.FindByNameAsync(_user.Correo);
            string Password = _user.Contrasena;
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, code, Password);
            if(result.Succeeded) Mensaje = "La contraseña fue reseteada exitosamente";
            return RedirectToAction("ListarUsuario");
        }

        public ActionResult CambiarEstadoUsuario(int IdUsuario)
        {
            Usuario _user = db.Usuarios.Single(x => x.IdUsuario == IdUsuario);
            _user.Vigente = !_user.Vigente;
            UsuarioCategoriaProyecto ucp = db.UsuarioCategoriaProyectos
                .SingleOrDefault(x => x.IdUsuario == IdUsuario 
                && x.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto != TipoEstadoUsuarioCategoriaProyecto.NoVigente);
            if(ucp != null) ucp.EstadoUsuarioCategoriaProyecto.IdTipoEstadoUsuarioCategoriaProyecto = TipoEstadoUsuarioCategoriaProyecto.NoVigente;

            var user = UserManager.FindByName(_user.Correo);
            if (!_user.Vigente)
            {                
                var roles = UserManager.GetRoles(user.Id);
                foreach (var rol in db.TipoUsuarioPermisos.Where(x => x.IdTipoUsuario == _user.IdTipoUsuario).Select(x => x.Permiso.Nombre))
                {
                    UserManager.RemoveFromRole(user.Id, rol);
                }
            }
            else
            {
                foreach (var permiso in db.TipoUsuarioPermisos.Where(x => x.IdTipoUsuario == _user.IdTipoUsuario).Select(x => x.Permiso.Nombre))
                {
                    var roleresult = UserManager.AddToRole(user.Id, permiso);
                }
            }
            
            db.SubmitChanges();
            Mensaje = "El usuario ha cambiado de estado exitosamente";
            return RedirectToAction("ListarUsuario");
        }

        //Método Champilistico de Creación de Usuarios
        //public async Task<ActionResult> CargaOriginalUsuarios()
        //{
        //    IEnumerable<Usuario> Usuarios = db.Usuarios.Where(x => x.IdTipoUsuario == 3);
        //    foreach (var _user in Usuarios)
        //    {
        //        var buffer = await UserManager.FindByNameAsync(_user.Correo);
        //        if(buffer == null)
        //        {
        //            string Password = _user.Contrasena;
        //            var user = new ApplicationUser { UserName = _user.Correo, Email = _user.Correo };
        //            var result = await UserManager.CreateAsync(user, Password);
        //            if (result.Succeeded)
        //            {
        //                foreach (var permiso in db.TipoUsuarioPermisos.Where(x => x.IdTipoUsuario == _user.IdTipoUsuario).Select(x => x.Permiso.Nombre))
        //                {
        //                    var roleresult = UserManager.AddToRole(user.Id, permiso);
        //                }
        //            }
        //        }                
        //    }
        //    return new EmptyResult();
        //}
    }
}