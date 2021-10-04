namespace SistemaCalidadAire.Entidades
{
    public class Qualitydata
    {
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
