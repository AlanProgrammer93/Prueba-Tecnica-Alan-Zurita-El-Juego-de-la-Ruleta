using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.Data
{
    public class OfficeDb : DbContext
    {
        public OfficeDb(DbContextOptions<OfficeDb> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
