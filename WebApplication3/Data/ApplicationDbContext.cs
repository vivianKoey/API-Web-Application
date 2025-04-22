using ApiVersioning.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVersioning.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products {  get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
