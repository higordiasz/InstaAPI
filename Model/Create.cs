using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Model.Create
{
    public class ICreate
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public ICreate (string username, string password, string email)
        {
            this.Username = username;
            this.Password = password;
            this.Email = email;
        }
    }
}
