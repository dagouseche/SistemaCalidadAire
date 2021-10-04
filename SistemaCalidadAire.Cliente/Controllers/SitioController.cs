using SistemaCalidadAire.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Net.Http.Formatting;

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

            try
            {
                var login = new {
                    id = username,
                    psw = passwd
                };

                HttpResponseMessage response = await client.PostAsync("login", login, new JsonMediaTypeFormatter());
                if (response.IsSuccessStatusCode)
                {
                    usuarioLogeado = await response.Content.ReadAsAsync<Usuario>();
                }

                //usuarioLogeado = new Usuario { id = username, user = new UserTypes { RowID = 1, Nombre = "Administrador" } };/*db.Usuario.FirstOrDefault(f => f.NombreUsuario == username && f.Contraseña == contraseña && f.Activo);*/
            }
            catch (Exception e)
            {
                return false;
            }

            List<Menu> menuUsuario;

            if (usuarioLogeado != null)
            {
                if (usuarioLogeado.type != 1/*!= "Administrador"*/)
                {
                    menuUsuario = new List<Menu>();
                }
                else
                {
                    menuUsuario = new List<Menu>();
                }

                Session["Usuario"] = usuarioLogeado.id;
                Session["ListaMenu"] = menuUsuario.Select(s => new Menu { Nombre = s.Nombre, Controlador = s.Controlador, Metodo = s.Metodo }).ToList();
                Session.Timeout = 180;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo encargado de cerrar la sesion del usuario
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ActionResult> LimpiarSessionAsync()
        {
            string mensaje = "";
            HttpResponseMessage response = await client.GetAsync("device");
            if (response.IsSuccessStatusCode)
            {
                Session.Clear();
                Session.RemoveAll();
                Session["Usuario"] = null;
                mensaje = await response.Content.ReadAsAsync<string>();
            }

            return RedirectToAction("Index", "Sitio");
        }
    }
}