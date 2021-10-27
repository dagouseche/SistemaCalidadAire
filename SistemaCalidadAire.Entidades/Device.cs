namespace SistemaCalidadAire.Entidades
{
    public class Device
    {
        /// <summary>
        /// Identificador del dispositivo
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Nombre del dispositivo
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Geolocalizacion del dispositivo
        /// </summary>
        public string geo { get; set; }

        /// <summary>
        /// Altitud del dispositivo
        /// </summary>
        public int altitude { get; set; }

        /// <summary>
        /// Localidad del dispositivo
        /// </summary>
        public string district { get; set; }

        /// <summary>
        /// Ciudad donde se encuentra el dispositivo
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Pais donde se ubica el dispositivo
        /// </summary>
        public string country { get; set; }
    }
}
