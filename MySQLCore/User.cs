using System;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace MySQLCore
{
	public class User
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class AccountContext : DbContext
	{
		public DbSet<User> User { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}

	}
}
