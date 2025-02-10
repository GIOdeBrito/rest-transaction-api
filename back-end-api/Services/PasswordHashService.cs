using System;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace BackEndApi.Services
{
	public static class PasswordHash
	{
		public static string HashPassword (string password)
		{
			PasswordHasher<object> passwordHasher = new();
			return passwordHasher.HashPassword(null, password);
		}

		public static bool BalancePasswords (string userPassword, string hashedPassword)
		{
			PasswordHasher<object> passwordHasher = new();

			var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, userPassword);
			return result == PasswordVerificationResult.Success;
		}
	}
}