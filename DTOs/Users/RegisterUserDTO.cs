using System.ComponentModel.DataAnnotations;

namespace IdentityServer.DTOs.Users
{
	public class RegisterUserDTO
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "First name is required")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Address1 is required")]
		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string Address3 { get; set; }

		[Required(ErrorMessage = "City is required")]
		public string City { get; set; }

		[Required(ErrorMessage = "County is required")]
		public string County { get; set; }

		[Required(ErrorMessage = "Country is required")]
		public string Country { get; set; }

		[Required(ErrorMessage = "Date of birth is required")]
		public DateTime DateOfBirth { get; set; }

		[Required(ErrorMessage = "Gender is required")]
		public int GenderId { get; set; }

		[Required(ErrorMessage = "Contact number is required")]
		public string ContactNumber { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Role is required")]
		public string RoleName { get; set; }
	}
}
