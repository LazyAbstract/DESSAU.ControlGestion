﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.EmisorCorreo
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-cl");
            DESSAUControlGestionCorreoDataContext db = new DESSAUControlGestionCorreoDataContext()
                .WithConnectionStringFromConfiguration();

            var fecha = DateTime.Now;
            //if (fecha.DayOfWeek == DayOfWeek.Friday && fecha.Hour < 12)
            //{
            //    db.SpAcusete();
            //}

            //if (fecha.DayOfWeek == DayOfWeek.Monday && fecha.Hour < 12)
            //{
            //    db.SpNotificaciones();
            //}

            //  para cada notificación tipo correo = 2 y estado creda = 1
            foreach (var noti in db.Notificacions.Where(x => x.IdTipoNotificacion == 2 && x.IdTipoEstadoNotificacion == 1))
            {
                Usuario usuario = db.Usuarios.SingleOrDefault(x => x.IdUsuario == noti.IdUsuario);
                string accion = String.IsNullOrEmpty(noti.Accion) ? String.Empty : "Favor diríjase a " + noti.Accion;
                if (usuario != null)
                {
                    Correo Correo = new Correo()
                    {
                        Remitente = "soporte@metricarts.com",
                        Destinatario = usuario.Correo,
                        Asunto = "Notificación de MetricNET",
                        CuerpoTexto = "Estimado(a) " + usuario.Nombre + ", " + noti.Mensaje + " " + accion,
                        CuerpoHTML = "<p>Estimado(a) " + usuario.Nombre + ", " + noti.Mensaje + " " + accion + "</p>",
                        FechaProgramadaEnvio = noti.FechaPostpuesto.GetValueOrDefault(DateTime.Now),
                        FechaCreacion = DateTime.Now,
                        Pendiente = true,
                    };

                    //  marco la notificación ciomo leída = 3
                    noti.IdTipoEstadoNotificacion = 3;

                    db.Correos.InsertOnSubmit(Correo);
                    db.SubmitChanges();
                }
            }

            IQueryable<Correo> correos = db.Correos.Where(x => x.FechaProgramadaEnvio <= DateTime.Today.AddMinutes(1)
                && x.Pendiente == true);
            foreach (Correo correo in correos)
            {
                EmisorCorreo.EnviarCorreo(correo);
                correo.Pendiente = false;
                correo.FechaEnvio = DateTime.Now;
                db.SubmitChanges();
            }
        }
    }
}