using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemaCalidadAire.Entidades;

namespace SistemaCalidadAire.Cliente.Models
{
    public class DashboardModel
    {
        /// <summary>
        /// Indica si el usuario cuenta con privilegios de administrador
        /// </summary>
        public bool Privilegios { get; set; }

        /// <summary>
        /// Listado de dispositivos
        /// </summary>
        public List<Device> ListaDispositivos { get; set; }

        /// <summary>
        /// Listado de temperaturas
        /// </summary>
        public List<DataGrafica> ListaTemperaturas { get; set; }

        /// <summary>
        /// Listado de material particulado
        /// </summary>
        public List<DataGrafica> ListaPM2 { get; set; }
    }
}