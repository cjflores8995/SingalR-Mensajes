using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class MainEntity
    {
        public ApplicationUser Usuario { get; set; }
        public IEnumerable<ApplicationUser> Usuarios { get; set; }
        public Mensaje Mensaje { get; set; }
        public List<Mensaje> Mensajes { get; set; }
    }
}
