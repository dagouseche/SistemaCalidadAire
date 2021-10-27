using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SistemaCalidadAire.Entidades;

namespace SistemaCalidadAire.Datos
{
    public class DAOUser
    {
        #region Singleton

        private static volatile DAOUser instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Instance
        /// </summary>
        public static DAOUser Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DAOUser();
                    }
                }
                return instance;
            }
        }

        #endregion Singleton

        Uri uri = new Uri("http://ragnarok000.pythonanywhere.com/calidadaire/");

        /// <summary>
        /// Consulta los datos del usuario
        /// </summary>
        /// <param name="nombreUsuario">Nombre de usuario</param>
        /// <param name="cookieSession">Valor de la cookie de session</param>
        /// <returns></returns>
        public async Task<Usuario> ConsultarDatosUsuario(string nombreUsuario, string cookieSession)
        {
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(uri, new Cookie("session", cookieSession));

            using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = true, CookieContainer = cookieContainer }) { BaseAddress = uri })
            {
                HttpResponseMessage responseDataUser = await client.GetAsync($"users/{nombreUsuario}");

                return responseDataUser.IsSuccessStatusCode ? await responseDataUser.Content.ReadAsAsync<Usuario>() : null;
            }
        }

        /// <summary>
        /// Consultar los tipos de usuario
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserTypes>> ConsultarTiposUsuario()
        {
            using (HttpClient client = new HttpClient() { BaseAddress = uri })
            {
                HttpResponseMessage responseDataUser = await client.GetAsync("usertypes");

                return responseDataUser.IsSuccessStatusCode ? await responseDataUser.Content.ReadAsAsync<List<UserTypes>>() : null;
            }
        }

        /// <summary>
        /// Finalizar la session del usuario
        /// </summary>
        /// <returns></returns>
        public async Task<bool> FinalizarSession()
        {
            using (HttpClient client = new HttpClient() { BaseAddress = uri })
            {
                HttpResponseMessage responseDataUser = await client.GetAsync("logout");

                return responseDataUser.IsSuccessStatusCode ? true : false;
            }
        }
    }
}