﻿using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels.Account
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }

		public string ReturnUrl { get; set; }
	}
}
