using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class MensajeRepository: Repository<Mensaje>
    {
        public int GetCountMessagesFromUserById(string userId)
        {
            int total = 0;
            total = db.Mensajes.Where(x => x.ToUserId.Equals(userId)).Count();
            return total;
        }
    }
}
