using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SistemaCalidadAire.Cliente.Models;
using SistemaCalidadAire.Cliente.Utilities;
using SistemaCalidadAire.Datos;
using SistemaCalidadAire.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SistemaCalidadAire.Cliente.Controllers
{
    [ValidadorSesion]
    public class HomeController : Controller
    {
        /// <summary>
        /// Metodo que se encarga de cargar la vista principal del sitio de calidad
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            DateTimeFormatInfo mfi = CultureInfo.GetCultureInfo("es-CO").DateTimeFormat;

            DashboardModel model = new DashboardModel
            {
                Privilegios = true,
                ListaDispositivos = new List<Device>(),
                ListaTemperaturas = new List<DataGrafica>()
            };

            List<Qualitydata> listaDatosCalidad = new List<Qualitydata>();

            if (Data.UsuarioExterno)
            {
                List<Lastday> listaUltimas24Horas = await DAOQualityData.Instance.ConsultarDatosCalidad24Horas();

                List<Device> listaDispositivos = (from data in listaUltimas24Horas
                                                  select new Device
                                                  {
                                                      id = data.device,
                                                      altitude = data.altitude,
                                                      city = data.city,
                                                      country = data.country,
                                                      district = data.district,
                                                      geo = data.geo,
                                                      name = data.name
                                                  }).ToList();

                listaDatosCalidad = (from data in listaUltimas24Horas
                                     select new Qualitydata
                                     {
                                         device = data.device,
                                         date = data.date,
                                         pressure = data.pressure,
                                         rh = data.rh,
                                         value_particle1 = data.value_particle1,
                                         value_particle2 = data.value_particle2,
                                         temp = data.temp
                                     }).ToList();

                List<Device> listaDispositivosLocalidad = new List<Device>();

                foreach (string localidad in (from item in listaDispositivos
                                              select item.district).Distinct())
                {
                    listaDispositivosLocalidad.AddRange(listaDispositivos.Where(w => w.district == localidad).Select(s => new Device { name = localidad, geo = s.geo, id = s.id }).ToList());
                }

                model.ListaDispositivos = listaDispositivosLocalidad;
            }
            else
            {
                List<Device> listaDispositivos = await DAODevice.Instance.ConsultarDispositivos(Data.CookieSession);

                Usuario datosUsuario = await DAOUser.Instance.ConsultarDatosUsuario(Data.UsuarioLogeado, Data.CookieSession);

                List<UserTypes> listaTiposUsuario = await DAOUser.Instance.ConsultarTiposUsuario();

                if (listaTiposUsuario.Where(w => w.id == datosUsuario.type).First().name == "Investigador")
                {
                    model.ListaDispositivos = listaDispositivos;

                    listaDatosCalidad = await DAOQualityData.Instance.ConsultarDatosCalidad(Data.CookieSession);
                }
                else if (listaTiposUsuario.Where(w => w.id == datosUsuario.type).First().name == "Participante")
                {
                    model.ListaDispositivos = listaDispositivos.Where(w => w.id == datosUsuario.device).ToList();

                    listaDatosCalidad = await DAOQualityData.Instance.ConsultarDatosCalidadXDispositivo(datosUsuario.device, Data.CookieSession);
                }
            }

            ///Datos para la grafica de tempratura
            model.ListaTemperaturas = (from item in listaDatosCalidad
                                       group item by DateTime.Parse(item.date).Month into newItem
                                       select new DataGrafica { Id = newItem.Key, Label = mfi.GetMonthName(newItem.Key), Valor = newItem.Average(s => s.temp).ToString() }).ToList();

            ///Datos para la grafica de datos de calidad
            model.ListaPM2 = (from item in listaDatosCalidad
                              group item by DateTime.Parse(item.date).Month into newItem
                              select new DataGrafica { Id = newItem.Key, Label = mfi.GetMonthName(newItem.Key), Valor = newItem.Average(s => s.value_particle2).ToString() }).ToList();

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

                List<Qualitydata> listadoDatos;

                if (Data.UsuarioExterno)
                {
                    listadoDatos = (from data in await DAOQualityData.Instance.ConsultarDatosCalidad24Horas()
                               select new Qualitydata
                               {
                                   device = data.device,
                                   date = data.date,
                                   pressure = data.pressure,
                                   rh = data.rh,
                                   value_particle1 = data.value_particle1,
                                   value_particle2 = data.value_particle2,
                                   temp = data.temp
                               }).ToList();
                }
                else
                {
                    listadoDatos = await DAOQualityData.Instance.ConsultarDatosCalidad(Data.CookieSession);
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
            List<Qualitydata> listado;

            if (Data.UsuarioExterno)
            {
                listado = (from data in await DAOQualityData.Instance.ConsultarDatosCalidad24Horas()
                           select new Qualitydata
                           {
                               device = data.device,
                               date = data.date,
                               pressure = data.pressure,
                               rh = data.rh,
                               value_particle1 = data.value_particle1,
                               value_particle2 = data.value_particle2,
                               temp = data.temp
                           }).Where(w => w.device == idDevice).ToList();
            }
            else {
                DateTime fechaUltimas24Horas = DateTime.Now.AddHours(-24);
                listado = (from data in await DAOQualityData.Instance.ConsultarDatosCalidadXDispositivo(idDevice, Data.CookieSession)
                           select data).Where(w => DateTime.Parse(w.date) >= fechaUltimas24Horas).ToList();
            }

            Qualitydata model = listado.FirstOrDefault();

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
            List<Qualitydata> listado;

            if (Data.UsuarioExterno)
            {
                listado = (from data in await DAOQualityData.Instance.ConsultarDatosCalidad24Horas()
                           select new Qualitydata
                           {
                               device = data.device,
                               date = data.date,
                               pressure = data.pressure,
                               rh = data.rh,
                               value_particle1 = data.value_particle1,
                               value_particle2 = data.value_particle2,
                               temp = data.temp
                           }).Where(w => w.device == idDevice).ToList();
            }
            else
            {
                listado = await DAOQualityData.Instance.ConsultarDatosCalidadXDispositivo(idDevice, Data.CookieSession);
            }

            return PartialView("_DetalleQualityData", listado);
        }
    }
}