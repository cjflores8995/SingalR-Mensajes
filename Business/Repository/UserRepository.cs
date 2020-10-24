using DataAccess;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Business.Repository
{
    public class UserRepository: Repository<ApplicationUser>
    {
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return db.Users;
        }

        public string GetUserIdByEmail(string email)
        {
            if(email == null || string.IsNullOrEmpty(email))
            {
                email = "carlos_jhousef593@hotmail.com";
            }

            return db.Users.FirstOrDefault(x => x.Email.Equals(email)).Id;
        }
    }
}
