using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                .WithConnectionStringFromConfiguration();
            //DateTime FechaCalculo = DateTime.Now.AddMonths(-1);

            //int response = 0;
            //DateTime Primero = new DateTime(FechaCalculo.Year, FechaCalculo.Month, 1);
            //DateTime FinMes = (new DateTime(FechaCalculo.Year, FechaCalculo.Month, 1)).AddMonths(1).AddDays(-1);
            //int Dias = (FinMes - Primero).Days;
            //DateTime contador = Primero;
            //for (int i = 0; i <= Dias; i++)
            //{
            //    DiaEspecial DiaEspecial = db.DiaEspecials.SingleOrDefault(x => x.Fecha == contador);
            //    if (DiaEspecial != null && DiaEspecial.Fecha == contador)
            //    {
            //        response += DiaEspecial.Horas;
            //    }
            //    else if (contador.DayOfWeek == DayOfWeek.Monday || contador.DayOfWeek == DayOfWeek.Tuesday || contador.DayOfWeek == DayOfWeek.Wednesday || contador.DayOfWeek == DayOfWeek.Thursday)
            //    {
            //        response += 10;

            //    }
            //    else if (contador.DayOfWeek == DayOfWeek.Friday)
            //    {
            //        response += 5;
            //    }
            //    contador = contador.AddDays(1);
            //}
            ////return response;

            //Console.WriteLine(response);
            //Console.ReadLine();

            Tmp_PruebasWebJob hola = new Tmp_PruebasWebJob()
            {
                Fecha = DateTime.Now,
                Texto = "Primera Prueba",
            };

            db.Tmp_PruebasWebJobs.InsertOnSubmit(hola);
            db.SubmitChanges();
        }
    }
}
