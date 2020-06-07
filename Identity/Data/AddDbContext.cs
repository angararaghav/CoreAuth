using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data
{
    //IdentityDbContext contains all the user tables
    public class AddDbContext : IdentityDbContext    {
        public AddDbContext(DbContextOptions<AddDbContext> options) : base (options)
        {

        }
    }
}
