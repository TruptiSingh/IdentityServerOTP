using System.Text;

using IdentityServer.Attributes;
using IdentityServer.Data.Identity;
using IdentityServer.DTOs.Enums;
using IdentityServer.Interfaces.AccountServices;
using IdentityServer.ViewModels.Account;

using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityServer.Controllers
{
	[SecurityHeaders]
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IIdentityServerInteractionService _interaction;
		private readonly IClientStore _clientStore;
		private readonly IEventService _events;
		private readonly IBuildLoginViewModelService _buildLoginViewModelService;
		private readonly IBuildLogoutViewModelService _buildLogoutViewModelService;
		private readonly IBuildLoggedOutViewModelService _buildLoggedOutViewModelService;
		private readonly ISendConfirmationEmailService _sendConfirmationEmailService;
		private readonly IConfirmEmailService _confirmEmailService;
		private readonly ISendForgottenPasswordEmailService _sendForgottenPasswordEmailService;

		public AccountController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			IIdentityServerInteractionService interaction,
			IClientStore clientStore,
			IEventService events,
			IBuildLoginViewModelService buildLoginViewModelService,
			IBuildLogoutViewModelService buildLogoutViewModelService,
			IBuildLoggedOutViewModelService buildLoggedOutViewModelService,
			ISendConfirmationEmailService sendConfirmationEmailService,
			IConfirmEmailService confirmEmailService,
			ISendForgottenPasswordEmailService sendForgottenPasswordEmailService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_interaction = interaction;
			_clientStore = clientStore;
			_events = events;
			_buildLoginViewModelService = buildLoginViewModelService;
			_buildLogoutViewModelService = buildLogoutViewModelService;
			_buildLoggedOutViewModelService = buildLoggedOutViewModelService;
			_sendConfirmationEmailService = sendConfirmationEmailService;
			_confirmEmailService = confirmEmailService;
			_sendForgottenPasswordEmailService = sendForgottenPasswordEmailService;
		}

		/// <summary>
		/// Entry point into the login workflow
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> Login(string returnUrl)
		{
			// build a model so we know what to show on the login page
			var vm = await _buildLoginViewModelService.BuildLoginViewModel(returnUrl);

			return View(vm);
		}

		/// <summary>
		/// Handle postback from username/password login
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginInputModel model, string button)
		{
			// check if we are in the context of an authorization request
			var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

			// the user clicked the "cancel" button
			if (button != "login")
			{
				if (context != null)
				{
					// if the user cancels, send a result back into IdentityServer as if they 
					// denied the consent (even if this client does not require consent).
					// this will send back an access denied OIDC error response to the client.
					await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

					return Redirect(model.ReturnUrl);
				}
				else
				{
					// since we don't have a valid context, then we just go back to the home page
					return Redirect("~/");
				}
			}

			if (ModelState.IsValid)
			{
				// validate username/password
				var user = await _userManager.FindByNameAsync(model.Username);

				if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
				{
					if (!user.EmailConfirmed)
					{
						var loginViewModel = await _buildLoginViewModelService.BuildLoginViewModel(model.ReturnUrl);

						ModelState.AddModelError("Username", "Email has not yet been confirmed");

						return View(loginViewModel);
					}

					var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);

					if (result.Succeeded)
					{
						await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, string.Concat(user.FirstName, ' ', user.LastName)));

						if (context != null)
						{
							// we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
							return Redirect(model.ReturnUrl);
						}

						// request for a local page
						if (Url.IsLocalUrl(model.ReturnUrl))
						{
							return Redirect(model.ReturnUrl);
						}
						else if (string.IsNullOrEmpty(model.ReturnUrl))
						{
							return Redirect("~/");
						}
						else
						{
							// user might have clicked on a malicious link - should be logged
							throw new Exception("invalid return URL");
						}
					}
				}

				await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));

				ModelState.AddModelError("Username", "Invalid username or password");
			}

			// something went wrong, show form with error
			var vm = await _buildLoginViewModelService.BuildLoginViewModel(model);

			return View(vm);
		}

		/// <summary>
		/// Show logout page
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> Logout(string logoutId)
		{
			// build a model so the logout page knows what to display
			var vm = await _buildLogoutViewModelService.BuildLogoutViewModel(User, logoutId);

			if (vm.ShowLogoutPrompt == false)
			{
				// if the request for logout was properly authenticated from IdentityServer, then
				// we don't need to show the prompt and can just log the user out directly.
				return await Logout(vm);
			}

			return View(vm);
		}

		/// <summary>
		/// Handle logout page postback
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout(LogoutInputModel model)
		{
			// build a model so the logged out page knows what to display
			var vm = await _buildLoggedOutViewModelService.BuildLoggedOutViewModel(User, HttpContext, model.LogoutId);

			if (User?.Identity.IsAuthenticated == true)
			{
				// delete local authentication cookie
				await _signInManager.SignOutAsync();

				// raise the logout event
				await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
			}

			// check if we need to trigger sign-out at an upstream identity provider
			if (vm.TriggerExternalSignout)
			{
				// build a return URL so the upstream provider will redirect back
				// to us after the user has logged out. this allows us to then
				// complete our single sign-out processing.
				string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

				// this triggers a redirect to the external provider for sign-out
				return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
			}

			return View("LoggedOut", vm);
		}

		[HttpGet]
		public IActionResult Register(string returnUrl)
		{
			return View(new RegisterRequestViewModel
			{
				ReturnUrl = returnUrl
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterRequestViewModel model)
		{
			//var aVal = 0; var blowUp = 1 / aVal;
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = new AppUser
			{
				Address1 = model.Address1,
				Address2 = model.Address2,
				Address3 = model.Address3,
				City = model.City,
				Country = model.Country,
				County = model.County,
				DateOfBirth = model.DateOfBirth,
				GenderId = model.GenderId,
				PhoneNumber = model.ContactNumber,
				UserName = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				RegistrationTimestamp = DateTime.UtcNow
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				AddEmailPasswordErrors(result);
				return View(model);
			}

			var roleName = Enum.GetName(typeof(Roles), model.RoleId);
			model.RoleName = roleName;

			await _userManager.AddToRoleAsync(user, model.RoleName);
			await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
			await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("name", string.Concat(user.FirstName, ' ', user.LastName)));
			await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
			await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", model.RoleName));
			await _sendConfirmationEmailService.SendConfirmationEmail(user, model.ReturnUrl);

			return RedirectToAction(nameof(RegisterConfirmation), "Account",
				new { returnUrl = model.ReturnUrl });
		}

		[HttpGet]
		public IActionResult RegisterConfirmation(string returnUrl = null)
		{
			return returnUrl == null ? View("Error") : View(new ResetPasswordConfirmationViewModel
			{
				ReturnUrl = returnUrl
			});
		}

		public IActionResult ConfirmEmail(string userid, string token, string returnUrl)
		{
			return View(_confirmEmailService.ConfirmEmail(userid, token, returnUrl));
		}

		[HttpGet]
		public IActionResult ForgottenPassword(string returnUrl)
		{
			return View(new ForgotPasswordViewModel
			{
				ReturnUrl = returnUrl
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgottenPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _sendForgottenPasswordEmailService.SendForgottenPasswordEmail(model.Email, model.ReturnUrl);

				return View("ForgottenPasswordConfirmation");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpGet]
		public IActionResult ForgottenPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ResetPassword(string code = null, string returnUrl = null)
		{
			return (code == null && returnUrl == null) ? View("Error") : View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByNameAsync(model.Email);

			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
			}

			var codeDecodedBytes = WebEncoders.Base64UrlDecode(model.Code);
			var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

			var result = await _userManager.ResetPasswordAsync(user, codeDecoded, model.Password);

			if (result.Succeeded)
			{
				return RedirectToAction(nameof(ResetPasswordConfirmation), "Account",
					new { returnUrl = model.ReturnUrl });
			}

			AddEmailPasswordErrors(result);

			return View();
		}

		[HttpGet]
		public IActionResult ResetPasswordConfirmation(string returnUrl = null)
		{
			return returnUrl == null ? View("Error") : View(new ResetPasswordConfirmationViewModel
			{
				ReturnUrl = returnUrl
			});
		}

		[HttpGet]
		public IActionResult CreatePassword(string code = null, string returnUrl = null)
		{
			return (code == null && returnUrl == null) ? View("Error") : View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByNameAsync(model.Email);

			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToAction(nameof(AccountController.CreatePasswordConfirmation), "Account");
			}

			var codeDecodedBytes = WebEncoders.Base64UrlDecode(model.Code);
			var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

			user.EmailConfirmed = true;

			await _userManager.UpdateAsync(user);

			var result = await _userManager.ResetPasswordAsync(user, codeDecoded, model.Password);

			if (result.Succeeded)
			{
				return RedirectToAction(nameof(AccountController.CreatePasswordConfirmation), "Account",
					new { returnUrl = model.ReturnUrl });
			}

			AddEmailPasswordErrors(result);

			return View();
		}

		[HttpGet]
		public IActionResult CreatePasswordConfirmation(string returnUrl = null)
		{
			return returnUrl == null ? View("Error") : View(new ResetPasswordConfirmationViewModel
			{
				ReturnUrl = returnUrl
			});
		}

		private void AddEmailPasswordErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				if (error.Code.StartsWith("Password"))
				{
					ModelState.AddModelError("Password", error.Description);
				}
				else
				{
					ModelState.AddModelError("Email", error.Description);
				}
			}
		}
	}
}
