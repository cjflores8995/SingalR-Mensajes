using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Mensaje : Common
    {
        public int Id { get; set; }

        [ForeignKey("ApplicationUserFrom")]
        public string FromUserId { get; set; }
        public virtual ApplicationUser ApplicationUserFrom { get; set; }

        [ForeignKey("ApplicationUserTo")]
        public string ToUserId { get; set; }
        public virtual ApplicationUser ApplicationUserTo { get; set; }

        public string Texto { get; set; }
    }
}
