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

		[HttpPost]
		[Route("users/add")]
		public async Task<ActionResult<User>> AddUser([FromBody] User user)
		{
			if (user.Name == string.Empty || user.Surname == string.Empty)
			{
				return new BadRequestResult();
			}

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
