using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
	public class UserDatabaseContext : DbContext
	{
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "server=localhost;database=users;user=root;password=mojairenka";
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
