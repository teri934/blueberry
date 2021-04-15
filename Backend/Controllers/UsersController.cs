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
