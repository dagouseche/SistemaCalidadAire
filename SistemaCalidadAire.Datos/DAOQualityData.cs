using SistemaCalidadAire.Entidades;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SistemaCalidadAire.Datos
{
    public class DAOQualityData
    {
        #region Singleton

        private static volatile DAOQualityData instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Instance
        /// </summary>
        public static DAOQualityData Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DAOQualityData();
                    }
                }
                return instance;
            }
        }

        #endregion Singleton

        Uri uri = new Uri("http://ragnarok000.pythonanywhere.com/calidadaire/");

        /// <summary>
        /// Consultar los datos de calidad
        /// </summary>
        /// <param name="cookieSession">Valor de la cookie de session</param>
        /// <returns></returns>
        public async Task<List<Qualitydata>> ConsultarDatosCalidad(string cookieSession)
        {
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(uri, new Cookie("session", cookieSession));

            using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = true, CookieContainer = cookieContainer }) { BaseAddress = uri })
            {
                HttpResponseMessage response = await client.GetAsync("qualitydata");

                return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<List<Qualitydata>>() : new List<Qualitydata>();
            }
        }

        /// <summary>
        /// Consultar los datos de calidad de un dispositivo por el ID
        /// </summary>
        /// <param name="idDevice">Identificador del dispositivo</param>
        /// <param name="cookieSession">Valor de la cookie de session</param>
        /// <returns></returns>
        public async Task<List<Qualitydata>> ConsultarDatosCalidadXDispositivo(int idDevice, string cookieSession)
        {
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(uri, new Cookie("session", cookieSession));

            using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = true, CookieContainer = cookieContainer }) { BaseAddress = uri })
            {
                HttpResponseMessage response = await client.GetAsync($"qualitydata/selectbydevice/{idDevice}");

                return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<List<Qualitydata>>() : new List<Qualitydata>();
            }
        }

        /// <summary>
        /// Consultar los datos de calidad de las ultimas 24 horas
        /// </summary>
        /// <returns></returns>
        public async Task<List<Lastday>> ConsultarDatosCalidad24Horas()
        {
            using (HttpClient client = new HttpClient() { BaseAddress = uri })
            {
                HttpResponseMessage response = await client.GetAsync("qualitydata/lastday");

                return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<List<Lastday>>() : new List<Lastday>();
            }
        }
    }
}
