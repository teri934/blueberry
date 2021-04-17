using System;
using Microsoft.AspNetCore.Identity;

namespace Backend
{
	public static class Hash
	{
		/// <summary>
		/// provides at least a little security
		/// desirably work the implementation in the future
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public static string HashPassword(string password)
		{
			var hashed = new PasswordHasher<object>().HashPassword(null, password);
			return hashed;
		}

		public static bool VerifyPassword(string hashedPassword, string providedPassword)
		{
			var passwordVerificationResult = new PasswordHasher<object>().VerifyHashedPassword(null, hashedPassword, providedPassword);

			if (passwordVerificationResult == PasswordVerificationResult.Success)
				return true;

			return false;
		}

	}
}
