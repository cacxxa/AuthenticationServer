using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationServer.Data.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public Role[] Roles { get; set; }

        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }

    public enum Role
    {
        Admin,
        Manager,
        User
    }
}
