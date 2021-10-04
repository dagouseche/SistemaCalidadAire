using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SistemaCalidadAire.Cliente.Models;
using SistemaCalidadAire.Cliente.Utilities;
using SistemaCalidadAire.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SistemaCalidadAire.Cliente.Controllers
{
    [ValidadorSesion]
    public class HomeController : Controller
    {
        private readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://ragnarok000.pythonanywhere.com/calidadaire/")
        };

        /// <summary>
        /// Metodo que se encarga de cargar la vista principal del sitio de calidad
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            System.Globalization.DateTimeFormatInfo mfi = CultureInfo.GetCultureInfo("es-CO").DateTimeFormat;

            DashboardModel model = new DashboardModel
            {
                Privilegios = true,
                ListaDispositivos = new List<Device>(),
                ListaTemperaturas = new List<DataGrafica>()
            };
            string usuario = Session["Usuario"].ToString();

            HttpResponseMessage response = await client.GetAsync("device");
            if (response.IsSuccessStatusCode)
            {
                model.ListaDispositivos = await response.Content.ReadAsAsync<List<Device>>();
            }

            HttpResponseMessage responseQualitydata = await client.GetAsync("qualitydata");
            if (responseQualitydata.IsSuccessStatusCode)
            {
                var listaDatos = await responseQualitydata.Content.ReadAsAsync<List<Qualitydata>>();

                model.ListaTemperaturas = (from item in listaDatos
                                           group item by DateTime.Parse(item.date).Month into newItem
                                           select new DataGrafica { Id = newItem.Key, Label = mfi.GetMonthName(newItem.Key), Valor = newItem.Average(s => s.temp).ToString() }).ToList();

                model.ListaPM2 = (from item in listaDatos
                                           group item by DateTime.Parse(item.date).Month into newItem
                                           select new DataGrafica { Id = newItem.Key, Label = mfi.GetMonthName(newItem.Key), Valor = newItem.Average(s => s.value_particle2).ToString() }).ToList();
            }

            return View(model);
        }

        /// <summary>
		/// Descargar reporte de quality data
		/// </summary>
		/// <param name="regional">id regional</param>
		/// <param name="municipio">id municipio</param>
		/// <returns></returns>
		[HttpGet]
        public async System.Threading.Tasks.Task<FileResult> GenerarExcelReporteAsync()
        {
            try
            {
                List<Qualitydata> listadoDatos = new List<Qualitydata>();

                byte[] excel = Convert.FromBase64String("");
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Reporte qualitydata");
                sheet.DefaultColumnWidth = 20;
                int rowIndex = 0;
                IRow row = sheet.CreateRow(0);

                string[] columnas = new string[] { "Device", "Pressure", "Rh", "Temp", "Particle 1", "Particle 2", "Date" };

                for (int i = 0; i < columnas.Length; i++)
                {
                    row.CreateCell(i).SetCellValue(columnas[i]);
                    sheet.AutoSizeColumn(i);
                }

                HttpResponseMessage responseQualitydata = await client.GetAsync("qualitydata");
                if (responseQualitydata.IsSuccessStatusCode)
                {
                    listadoDatos = await responseQualitydata.Content.ReadAsAsync<List<Qualitydata>>();
                }

                foreach (Qualitydata item in listadoDatos)
                {
                    rowIndex++;
                    row = sheet.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.device);
                    row.CreateCell(1).SetCellValue(item.pressure);
                    row.CreateCell(2).SetCellValue(item.rh);
                    row.CreateCell(3).SetCellValue(item.temp);
                    row.CreateCell(4).SetCellValue(item.value_particle1);
                    row.CreateCell(5).SetCellValue(item.value_particle2);
                    row.CreateCell(6).SetCellValue(item.date);
                }

                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    excel = exportData.GetBuffer();
                    return File(excel, System.Net.Mime.MediaTypeNames.Application.Octet, "Reporte.xls");
                }
            }
            catch
            {
                return File(Convert.FromBase64String(""), "application/vnd.ms-excel", "ErrorGenerardoArchivo.xls");
            }
        }

        /// <summary>
        /// Metodo que consulta la informacion de calidad de aire de un dispositivo en especifico
        /// </summary>
        /// <param name="idDevice">ID del dispositivo</param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> ConsultarInfoDeviceAsync(int idDevice)
        {
            Qualitydata model = null;

            HttpResponseMessage response = await client.GetAsync($"qualitydata/selectbydevice/{idDevice}");
            if (response.IsSuccessStatusCode)
            {
                List<Qualitydata> listado = await response.Content.ReadAsAsync<List<Qualitydata>>();

                model = listado.FirstOrDefault();
            }

            if (model != null)
            {
                return Json(new { temp = model.temp, particle1 = model.value_particle1, particle2 = model.value_particle2, pressure = model.pressure, rh = model.rh });
            }
            else
            {
                return Json(new { temp = 0, particle1 = 0, particle2 = 0, pressure = 0, rh = 0 });
            }
        }

        /// <summary>
        /// Consultar el detalle de calidad de un dispositivo
        /// </summary>
        /// <param name="idDevice">ID del dispositivo</param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<PartialViewResult> DetalleQualityDataAsync(int idDevice)
        {
            HttpResponseMessage response = await client.GetAsync($"qualitydata/selectbydevice/{idDevice}");
            if (response.IsSuccessStatusCode)
            {
                List<Qualitydata> listado = await response.Content.ReadAsAsync<List<Qualitydata>>();

                return PartialView("_DetalleQualityData", listado);
            }

            return PartialView("_DetalleQualityData", new List<Qualitydata>());
        }
    }
}