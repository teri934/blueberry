using System;

namespace DTO
{
	public class DTOSignIn
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class DTOSignUp
	{
		public string Username { get; set; }
	}

	public class DTORelevantData
	{
		public string Username { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }
	}

	public class DTOUser
	{
		public string Username { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public string Password { get; set; }
	}
}
