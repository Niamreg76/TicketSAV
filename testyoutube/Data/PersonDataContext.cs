using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testyoutube.Data
{
    public class PersonDataContext : DbContext
    {
        public PersonDataContext(DbContextOptions<PersonDataContext> options) : base(options)
        {

        }
    }
}
