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
    }
}
