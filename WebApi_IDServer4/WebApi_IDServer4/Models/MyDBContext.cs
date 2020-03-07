using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_IDServer4.Models
{
    public class MyDBContext : DbContext
    {

        public MyDBContext(DbContextOptions<MyDBContext> options):base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
