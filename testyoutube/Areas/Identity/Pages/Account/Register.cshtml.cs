using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using testyoutube.Areas.Identity.Data;

namespace testyoutube.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<aspnetusers> _signInManager;
        private readonly UserManager<aspnetusers> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<aspnetusers> userManager,
            SignInManager<aspnetusers> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "L'adresse e-mail est obligatoire")]
            [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Le mot de passe est obligatoire")] // Erreur mot de passe manquant
            [StringLength(100, ErrorMessage = "Le mot de passe doit comporter au moins {2} et au maximum {1} caractères.", MinimumLength = 8)] // Erreur mot de passe < 8 ou > 100
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Le mot de passe doit avoir au moins un caractère spécial, un chiffre et une majuscule.")] 
                                                                        // message pour vérifier si le mot de passe est bien conforme
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Cette ligne utilise l'opérateur de coalescence nulle (??=) pour affecter à returnUrl la valeur de Url.Content("~/") si returnUrl est nul.
            // Cela permet de définir une valeur par défaut pour returnUrl si aucune valeur n'est fournie.

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            // Cette ligne utilise le gestionnaire de connexion (_signInManager) pour obtenir de manière asynchrone les schémas d'authentification externes
            // pris en charge. Les schémas sont ensuite convertis en une liste et assignés à la propriété ExternalLogins.

            if (ModelState.IsValid)
            {
                var user = new aspnetusers { UserName = Input.Email, Email = Input.Email };
                // Cette ligne crée une nouvelle instance de la classe aspnetusers (probablement une entité utilisateur dans le contexte de l'application)
                // et lui assigne les valeurs de UserName et Email à partir des propriétés Input.Email de l'objet Input (qui est une propriété de la classe RegisterModel).

                var result = await _userManager.CreateAsync(user, Input.Password);
                // Cette ligne utilise le gestionnaire d'utilisateurs (_userManager) pour créer de manière asynchrone un nouvel utilisateur (user) avec le mot de passe fourni (Input.Password).
                // Le résultat de cette opération est stocké dans la variable result.

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // Cette ligne utilise le gestionnaire d'utilisateurs (_userManager) pour générer de manière asynchrone un jeton de confirmation d'e-mail pour l'utilisateur créé (user).
                    // Le jeton est stocké dans la variable code.

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    // Cette ligne encode le jeton de confirmation d'e-mail (code) en utilisant l'encodage Base64 URL.
                    // Cela permet de le représenter sous forme de chaîne de caractères sécurisée qui peut être utilisée dans une URL.
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    // Cette ligne envoie de manière asynchrone un e-mail de confirmation à l'adresse e-mail fournie (Input.Email).
                    // L'e-mail contient un message qui demande à l'utilisateur de confirmer son compte en cliquant sur un lien généré à partir de callbackUrl.
                    // La méthode HtmlEncoder.Default.Encode est utilisée pour encoder l'URL de rappel dans le corps de l'e-mail.

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        // Cette ligne connecte de manière asynchrone l'utilisateur créé (user) en utilisant le gestionnaire de connexion (_signInManager).
                        // Le paramètre isPersistent est défini sur false, ce qui signifie que l'utilisateur ne sera pas connecté de manière persistante
                        // (la connexion sera perdue après la fermeture du navigateur).
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
