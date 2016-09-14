using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.EmisorCorreo
{
    public partial class DESSAUControlGestionCorreoDataContext
    {
        ////  para pruebas locales
        const string CONNECTIONSTRING_KEY = "AppDb";
        public DESSAUControlGestionCorreoDataContext WithConnectionStringFromConfiguration(string key)
        {
            var settingsCollection = ConfigurationManager.ConnectionStrings[key];
            if (settingsCollection == null) throw new ConfigurationErrorsException(String.Format("No existe el ConnectionString con la llave \"{0}\" en el archivo de configuracion.", key));

            string connectionString = settingsCollection.ConnectionString;
            if (connectionString == null && String.IsNullOrEmpty(connectionString)) throw new ConfigurationErrorsException(String.Format("El ConnectionString especificado para la llave \"{0}\" está vacío.", key));

            this.Connection.ConnectionString = settingsCollection.ConnectionString;
            this.CommandTimeout = 0;
            return this;
        }

        public DESSAUControlGestionCorreoDataContext WithConnectionStringFromConfiguration()
        {
            return WithConnectionStringFromConfiguration(CONNECTIONSTRING_KEY);
        }


        //para job en producción
        //public DESSAUControlGestionCorreoDataContext WithConnectionStringFromConfiguration()
        //{
        //    this.Connection.ConnectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AppDb");
        //    this.CommandTimeout = 1200;
        //    return this;
        //}
    }
}
