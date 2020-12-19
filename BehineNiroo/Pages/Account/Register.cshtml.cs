using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using BehineNiroo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BehineNiroo
{
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly BehineNirooContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public RegisterModel(BehineNirooContext context, UserManager<IdentityUser> userManager,
                            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User NewUser { get; set; }


        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> IsUsernameInUse(string Username)
        //{
        //    var user = await userManager.FindByNameAsync(Username);

        //    if (user == null)
        //    {
        //        return Json(true);
        //    }
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = NewUser.Username
            };
            var result = await userManager.CreateAsync(user, NewUser.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}