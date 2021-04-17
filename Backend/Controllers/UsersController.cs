using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
	[ApiController]
	public class UsersController
	{
		private readonly UserDatabaseContext context;

		public UsersController(UserDatabaseContext context)
		{
			this.context = context;
			context.Database.EnsureCreated(); //TODO: call it somewhere, where it is more appropiate to create all tables and mock data
		}

		[HttpGet]
		[Route("users/getAll")]
		public async Task<List<User>> GetUsers()
		{
			var users = await context.User.ToListAsync();
			return users;

		}

		/// <summary>
		/// checks the pairing of username and password
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns>null if thu user with the provided username and password doesn't exist</returns>
		[HttpGet]
		[Route("users/getExisting/{username}/{password}")]
		public async Task<ActionResult<User>> GetExistingUser(string username, string password)
		{
			///same username is not allowed
			var foundEntity = await context.User.FirstOrDefaultAsync(user => user.Username == username);

			if (foundEntity == null || !Hash.VerifyPassword(foundEntity.Password, password))
				return null;

			return foundEntity;
		}

		/// <summary>
		/// checks if the username exists in the database
		/// </summary>
		/// <param name="username"></param>
		/// <returns>true if the username doesn't exist in the database</returns>
		[HttpGet]
		[Route("users/checkPotential/{username}")]
		public async Task<ActionResult<bool>> CheckPotentialUser(string username)
		{
			///same username is not allowed
			var foundEntity = await context.User.FirstOrDefaultAsync(user => user.Username == username);

			if (foundEntity == null)
				return true;

			return false;
		}

		[HttpPost]
		[Route("users/add")]
		public async Task<ActionResult<User>> AddUser([FromBody] User user)
		{
			if (user.Name == string.Empty || user.Surname == string.Empty || user.Username == string.Empty || user.Password == string.Empty)
			{
				return new BadRequestResult();
			}

			//string username = user.Username;
			//var foundEntity = await context.User.FirstOrDefaultAsync(u => u.Username == username);
			//if(foundEntity != null)
			//	return new BadRequestResult();

			user.Password = Hash.HashPassword(user.Password);
			await context.User.AddAsync(user);
			await context.SaveChangesAsync();
			return new CreatedAtRouteResult(null, user);
			//return new CreatedAtActionResult(nameof(AddUser), nameof(UsersController), null, user);
		}

		/*[HttpDelete]*/
		[Route("users/delete/{id}")]
		public async Task RemoveUser(Guid id)
		{
			/*TODO: try passing more info to the user about the operation*/
			var foundEntity = await context.User.FirstOrDefaultAsync(user => user.Id == id);
			if(foundEntity != null)
			{
				context.User.Remove(foundEntity);
				await context.SaveChangesAsync();
			}
		}
	}
}
