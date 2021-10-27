using SistemaCalidadAire.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCalidadAire.Datos
{
    public class DAODevice
    {
        #region Singleton

        private static volatile DAODevice instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Instance
        /// </summary>
        public static DAODevice Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DAODevice();
                    }
                }
                return instance;
            }
        }

        #endregion Singleton

        Uri uri = new Uri("http://ragnarok000.pythonanywhere.com/calidadaire/");

        /// <summary>
        /// Consultar los dispositivos
        /// </summary>
        /// <param name="cookieSession">Valor de la cookie de session</param>
        /// <returns></returns>
        public async Task<List<Device>> ConsultarDispositivos(string cookieSession)
        {
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(uri, new Cookie("session", cookieSession));

            using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = true, CookieContainer = cookieContainer }) { BaseAddress = uri })
            {
                HttpResponseMessage response = await client.GetAsync("device");

                return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<List<Device>>() : new List<Device>();
            }
        }
    }
}
