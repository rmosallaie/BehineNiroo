using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BehineNiroo
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
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

            [Required(ErrorMessage =".کلمه عبور وارد نشده است")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "به خاطر سپردن")]
            public bool RememberMe { get; set; }
        }

        public void OnGet()
        {
            //return Page();
        }

        public IActionResult OnPost(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(
                    Input.Username, Input.Password, Input.RememberMe, false).GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToPage("/Index");
                    }
                }
                ModelState.AddModelError(string.Empty, ".نام کاربری یا کلمه عبور اشتباه است");
            }
            return Page();
        }

    }
}