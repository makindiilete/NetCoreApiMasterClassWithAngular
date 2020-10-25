using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data //ds class is inside API/Data
{
    public class DataContext : DbContext
    {
        //create a constructor for the DataContext class dt call the constructor of DbContext class which requires an argument
        public DataContext( DbContextOptions options) :base(options)
        {

        }

        //Using our entity (model) AppUser, we create a Users table in the database
        public DbSet<AppUser> Users { get; set; }
    }
}
