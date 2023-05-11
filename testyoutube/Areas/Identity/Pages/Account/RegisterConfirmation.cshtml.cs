using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using testyoutube.Areas.Identity.Data;
using testyoutube.Services;
using IEmailSender = testyoutube.Services.IEmailSender;

namespace testyoutube.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<aspnetusers> _userManager;
        private readonly IEmailSender _emailSender;

        public RegisterConfirmationModel(UserManager<aspnetusers> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            this._emailSender = emailSender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId, code, returnUrl },
                    protocol: Request.Scheme);

                var message = "Bonjour, \n\n Cliquez sur le lien suivant pour confirmer l'adresse mail : \n\n " + EmailConfirmationUrl + " \n\n Ce message est généré automatiquement.";

                await _emailSender.SendMailAsync(email, "Confirmation de mail", message);
            }

            return Page();
        }
    }
}
