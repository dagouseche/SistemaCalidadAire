using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCalidadAire.Entidades
{
    public class Menu
    {
        public int RowID { get; set; }
        public string Nombre { get; set; }
        public string Controlador { get; set; }
        public string Metodo { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
    }
}
