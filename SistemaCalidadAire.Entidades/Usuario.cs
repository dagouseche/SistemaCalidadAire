namespace SistemaCalidadAire.Entidades
{
    public class Usuario
    {
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        public string idtype { get; set; }

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Email del usuario
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Numero de celular
        /// </summary>
        public string cel { get; set; }

        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        public string psw { get; set; }

        /// <summary>
        /// Tipo de usuario
        /// </summary>
        public int type { get; set; }


        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public string bdate { get; set; }

        /// <summary>
        /// Dispositivo al que esta asociado el usuario
        /// </summary>
        public int device { get; set; }
    }
}
