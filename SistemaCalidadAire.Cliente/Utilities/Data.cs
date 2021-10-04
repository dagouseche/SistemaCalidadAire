using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaCalidadAire.Cliente.Utilities
{
    public static class Data
    {
        /// <summary>
        /// Usuario que esta logueado en el sistema
        /// </summary>
        public static string UsuarioLogeado
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session["Usuario"];
            }
        }
    }
}