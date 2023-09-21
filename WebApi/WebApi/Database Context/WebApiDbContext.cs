using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Database_Context
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        public DbSet<Document> Documents { get; set; }

        
    }
}
