using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Common
    {
        public Common()
        {
            this.FechaRegistro = DateTime.Now;
            this.FechaModificacion = DateTime.Now;
            this.Estado = true;
        }

        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Estado { get; set; }
    }
}
