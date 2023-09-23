using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels.Account
{
	public class RegisterRequestViewModel
	{
		public string ReturnUrl { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "First name is required")]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Address1 is required")]
		[StringLength(200)]
		public string Address1 { get; set; }

		[StringLength(200)]
		public string Address2 { get; set; }

		[StringLength(200)]
		public string Address3 { get; set; }

		[Required(ErrorMessage = "City is required")]
		[StringLength(100)]
		public string City { get; set; }

		[Required(ErrorMessage = "County is required")]
		[StringLength(100)]
		public string County { get; set; }

		[Required(ErrorMessage = "Country is required")]
		[StringLength(100)]
		public string Country { get; set; }

		[Required(ErrorMessage = "Date of birth is required")]
		public DateTime DateOfBirth { get; set; }

		[Required(ErrorMessage = "Gender is required")]
		public int GenderId { get; set; }

		[Required(ErrorMessage = "Contact number is required")]
		[StringLength(20)]
		public string ContactNumber { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Role is required")]
		public int RoleId { get; set; }

		public string RoleName { get; set; }
	}
}
