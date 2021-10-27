using SistemaCalidadAire.Entidades;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Net.Http.Formatting;
using System.Net;
using System.Web;
using SistemaCalidadAire.Cliente.Utilities;
using SistemaCalidadAire.Datos;

namespace SistemaCalidadAire.Cliente.Controllers
{
    public class SitioController : Controller
    {
        private readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://ragnarok000.pythonanywhere.com/calidadaire/")
        };

        /// <summary>
        /// Metodo que carga el sitio web estatico
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Metodo que permite hacer el logueo de usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contraseña"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ActionResult> InicioSesionAsync(string usuario, string contraseña)
        {
            bool result = false;
            try
            {
                result = await ValidateLoginAsync(usuario, contraseña);
            }
            catch
            {
                throw new System.ArgumentException("Usuario o contraseña incorrecto.", "Translanza");
            }

            if (result)
            {
                return JavaScript("window.location = '" + Url.Action("Index", "Home") + "'");
            }
            else
            {
                throw new System.ArgumentException("Usuario o contraseña incorrecto.", "Translanza");
            }
        }

        /// <summary>
        /// Metodo que se encarga de hacer la validacion consultado el servicio de login
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="passwd">Contraseña</param>
        /// <returns></returns>
        private async System.Threading.Tasks.Task<bool> ValidateLoginAsync(string username, string passwd)
        {
            Usuario usuarioLogeado = null;
            string cookieValue = string.Empty;

            try
            {
                var login = new
                {
                    id = username,
                    psw = passwd
                };

                HttpResponseMessage response = await client.PostAsync("login", login, new JsonMediaTypeFormatter());
                if (response.IsSuccessStatusCode)
                {
                    usuarioLogeado = await response.Content.ReadAsAsync<Usuario>();

                    response.Headers.TryGetValues("Set-Cookie", out var setCookie);
                    string setCookieString = setCookie.Single();
                    string[] cookieTokens = setCookieString.Split(';');
                    string firstCookie = cookieTokens.FirstOrDefault();
                    string[] keyValueTokens = firstCookie.Split('=');
                    string valueString = keyValueTokens[1];

                    cookieValue = HttpUtility.UrlDecode(valueString);
                }
            }
            catch
            {
                return false;
            }

            if (usuarioLogeado != null)
            {
                Session["Usuario"] = usuarioLogeado.id;
                Session["CookieSession"] = cookieValue;
                Session["UsuarioExterno"] = false;
                Session.Timeout = 180;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo que permite dar ingreso a un usuario externo
        /// </summary>
        /// <returns></returns>
        public ActionResult IngresoExterno()
        {
            Session["Usuario"] = "Usuario Externo";
            Session["UsuarioExterno"] = true;

            return JavaScript("window.location = '" + Url.Action("Index", "Home") + "'");
        }

        /// <summary>
        /// Metodo encargado de cerrar la sesion del usuario
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ActionResult> LimpiarSessionAsync()
        {
            if (!Data.UsuarioExterno)
            {
                await DAOUser.Instance.FinalizarSession();
            }

            Session.Clear();
            Session.RemoveAll();

            return RedirectToAction("Index", "Sitio");
        }
    }
}