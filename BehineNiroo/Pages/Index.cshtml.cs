using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BehineNiroo.Pages
{
    static class Globals
    {
        public static string Idd;
        public static string OnlineTree;
        public static int OnlinePageLoadCount = 0;
        public static string OnlineParameters;

        public static string EventConnection =          "server=localhost;user id=root;database=event;persistsecurityinfo=True;password=root;";
        public static string EnergyConnection =         "server=localhost;user id=root;database=energy;persistsecurityinfo=True;password=root;";
        public static string LoggerConnection =         "server=localhost;user id=root;database=logger;persistsecurityinfo=True;password=root;";
        public static string SavingsConnection =        "server=localhost;user id=root;database=savings;persistsecurityinfo=True;password=root;";
        public static string SecurityConnection =       "server=localhost;user id=root;database=security;persistsecurityinfo=True;password=root;";
        public static string ManagerConnection =        "server=localhost;user id=root;database=manager;persistsecurityinfo=True;password=root;";
        public static string PocketSwitchConnection =   "server=localhost;user id=root;database=pocketswitch;persistsecurityinfo=True;password=root;";
    }

    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(ILogger<IndexModel> logger, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = ".نام کاربری وارد نشده است")]
            public string Username { get; set; }

            [Required(ErrorMessage = ".کلمه عبور وارد نشده است")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "به خاطر سپردن")]
            public bool RememberMe { get; set; }
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(
                    Input.Username, Input.Password, Input.RememberMe, false).GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        Input.Username = "";
                        return RedirectToPage("/Index");
                    }
                }
                ModelState.AddModelError(string.Empty, ".نام کاربری یا کلمه عبور اشتباه است");
            }
            return Page();
        }
    }
}