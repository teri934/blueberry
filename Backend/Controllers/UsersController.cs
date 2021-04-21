using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTO;

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
		public async Task<List<DTOUser>> GetUsers()
		{
			List<DTOUser> dtoUsers = new List<DTOUser>();
			var users = await context.User.ToListAsync();

			for (int i = 0; i < users.Count; i++)
			{
				dtoUsers.Add(new DTOUser { Username = users[i].Username, Name = users[i].Name, Surname = users[i].Surname, Password = users[i].Password });
			}

			return dtoUsers;

		}

		/// <summary>
		/// checks the pairing of username and password
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns>null if thu user with the provided username and password doesn't exist</returns>
		[HttpPost]
		[Route("users/getExisting")]
		public async Task<ActionResult<DTORelevantData>> GetExistingUser([FromBody] DTOSignIn user)
		{
			string username = user.Username;
			string password = user.Password;

			///same username is not allowed
			var foundEntity = await context.User.FirstOrDefaultAsync(user => user.Username == username);

			if (foundEntity == null || !Hash.VerifyPassword(foundEntity.Password, password))
				return null;

			return new DTORelevantData {Name = foundEntity.Name, Surname = foundEntity.Surname, Username = foundEntity.Username };
		}

		/// <summary>
		/// checks if the username exists in the database
		/// </summary>
		/// <param name="username"></param>
		/// <returns>true if the username doesn't exist in the database</returns>
		[HttpPost]
		[Route("users/checkPotential")]
		public async Task<ActionResult<bool>> CheckPotentialUser([FromBody] DTOSignUp user)
		{
			string username = user.Username;

			///same username is not allowed
			var foundEntity = await context.User.FirstOrDefaultAsync(user => user.Username == username);

			if (foundEntity == null)
				return true;

			return false;
		}

		[HttpPost]
		[Route("users/add")]
		public async Task<ActionResult<DTORelevantData>> AddUser([FromBody] User user)
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
			return new CreatedAtRouteResult(null, new DTORelevantData { Name = user.Name, Surname = user.Surname, Username = user.Username });
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
