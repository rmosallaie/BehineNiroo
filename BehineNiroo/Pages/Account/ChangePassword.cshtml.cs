using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BehineNiroo
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public ChangePasswordModel(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public class ChangePassword
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور فعلی")]
            public string CurrentPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور جدید")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار کلمه عبور جدید")]
            [Compare("NewPassword", ErrorMessage =
                ".کلمه عبور جدید و تکرار آن مطابقت ندارند")]
            public string ConfirmPassword { get; set; }
        }

        public ChangePassword changePassword { get; set; } = new ChangePassword();

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(ChangePassword changepassword)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToPage("/Account/Login");
                }

                var result = await userManager.ChangePasswordAsync(user, changepassword.CurrentPassword,
                                                                    changepassword.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Description == "Incorrect password.")
                        {
                            error.Description = ".کلمه عبور فعلی اشتباه است";
                        }
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                await signInManager.RefreshSignInAsync(user);
                return RedirectToPage("/Account/ChangePasswordConfirmation");
            }
            return Page();
        }
    }
}