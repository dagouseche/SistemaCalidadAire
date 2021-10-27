namespace SistemaCalidadAire.Entidades
{
    public class Lastday
    {
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

        /// <summary>
        /// ID del dispositivo
        /// </summary>
        public int device { get; set; }

        /// <summary>
        /// Presion
        /// </summary>
        public int pressure { get; set; }

        /// <summary>
        /// Temperatura
        /// </summary>
        public int temp { get; set; }

        /// <summary>
        /// Fecha en la que se tomaron los datos
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int rh { get; set; }

        /// <summary>
        /// Identificador de la particula 1
        /// </summary>
        public int id_particle1 { get; set; }

        /// <summary>
        /// Identificador del la particula 2
        /// </summary>
        public int id_particle2 { get; set; }

        /// <summary>
        /// Valor  de la particula 1
        /// </summary>
        public int value_particle1 { get; set; }

        /// <summary>
        /// Valor  de la particula 2
        /// </summary>
        public int value_particle2 { get; set; }
    }
}
