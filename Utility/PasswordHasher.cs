using System;
using BCrypt.Net;

namespace api.Utility
{
	public class PasswordHasher
	{
		public static string Encrypt (string password) // encrypt a password, used before storing password to database
		{
			string salt = BCrypt.Net.BCrypt.GenerateSalt();
			return BCrypt.Net.BCrypt.HashPassword(password, salt);
		}

		public static bool Verify (string givenPassword, string hashedPassword) // verify that 2 strings are the same, used when retrieving an account
		{
			return BCrypt.Net.BCrypt.Verify(givenPassword, hashedPassword);
		}
	}
}

