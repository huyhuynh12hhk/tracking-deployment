using MediaAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaAPI.Data
{
    public class MediaStorageContext : DbContext
    {
        public MediaStorageContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MediaObject> MediaItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
