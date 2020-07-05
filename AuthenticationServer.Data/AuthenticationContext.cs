using AuthenticationServer.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthenticationServer.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
