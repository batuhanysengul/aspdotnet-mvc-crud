using ASPNETMVC_CRUD.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVC_CRUD.Data
{
    public class MVCDemoDbContext : DbContext
    {

        //constructor
        public MVCDemoDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Employee>  Employees { get; set; }

    }
}
