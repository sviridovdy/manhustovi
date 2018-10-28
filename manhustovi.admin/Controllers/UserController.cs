using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using OtpNet;

namespace manhustovi.admin.Controllers
{
	[Route("api")]
	public class UserController : Controller
	{
		[HttpPost("signin"), AllowAnonymous]
		public async Task<IActionResult> PostSignIn([FromBody] JObject jSignInData, [FromServices] IConfiguration config)
		{
			try
			{
				var clientTotp = jSignInData["code"].Value<string>();
				var success = ValidateTotp(config, clientTotp);
				if (success)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, "manhust")
					};

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
				}

				var jResponse = new JObject
				{
					["success"] = success
				};
				return Json(jResponse);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPost("signout"), AllowAnonymous]
		public async Task<IActionResult> PostSignOut()
		{
			await HttpContext.SignOutAsync();
			var jResponse = new JObject
			{
				["success"] = true
			};
			return Json(jResponse);
		}

		private static bool ValidateTotp(IConfiguration config, string clientTotp)
		{
			var secretHexString = config["totpSecretKey"];
			var secretBytes = new byte[secretHexString.Length / 2];
			for (var i = 0; i < secretBytes.Length; i++)
			{
				secretBytes[i] = byte.Parse(secretHexString.Substring(i * 2, 2), NumberStyles.HexNumber);
			}

			var totp = new Totp(secretBytes);
			var serverTotp = totp.ComputeTotp();
			return serverTotp == clientTotp;
		}
	}
}