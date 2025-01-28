using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BackEndApi.Models;

namespace BackEndApi.Services
{
	public static class JwtToken
	{
		public static string GetToken (User user)
		{
			JwtSecurityTokenHandler handler = new();

			string jsonHash = Environment.GetEnvironmentVariable("JSON_TOKEN_SECRET");

			byte[] key = Encoding.ASCII.GetBytes(jsonHash);
			SigningCredentials credentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				SigningCredentials = credentials,
				Expires = DateTime.UtcNow.AddHours(1),
				Subject = GetClaims(user)
			};

			SecurityToken token = handler.CreateToken(tokenDescriptor);

			// Creates a string from the token
			string strToken = handler.WriteToken(token);

			return strToken;
		}

		private static ClaimsIdentity GetClaims (User user)
		{
			var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
				new Claim(ClaimTypes.Role, user.Role)
            };

            return new ClaimsIdentity(claims);
		}
	}
}