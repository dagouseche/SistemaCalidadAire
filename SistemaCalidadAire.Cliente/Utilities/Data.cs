using SistemaCalidadAire.Entidades;
using System.Collections.Generic;
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
                return (string)HttpContext.Current.Session["Usuario"];
            }
        }

        /// <summary>
        /// Cookie de la Session del usuario que esta logueado en el sistema
        /// </summary>
        public static string CookieSession
        {
            get
            {
                return (string)HttpContext.Current.Session["CookieSession"];
            }
        }

        /// <summary>
        /// Indica si el usuario es externo
        /// </summary>
        public static bool UsuarioExterno
        {
            get
            {
                return bool.Parse(HttpContext.Current.Session["UsuarioExterno"].ToString());
            }
        }
    }
}