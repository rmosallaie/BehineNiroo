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
    public class CreateModel : PageModel
    {
        private readonly BehineNirooContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public CreateModel(BehineNirooContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
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
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if (error.Description == "Passwords must be at least 6 characters.")
                    {
                        error.Description = ".کلمه عبور باید حداقل 6 کاراکتر باشد";
                    }
                    else if (error.Description == "Passwords must have at least one digit ('0'-'9').")
                    {
                        error.Description = ".کلمه عبور باید شامل حداقل یک عدد باشد";
                    }
                    else if (error.Description == "Passwords must have at least one non alphanumeric character.")
                    {
                        error.Description = ".کلمه عبور باید شامل حداقل یک کاراکتر غیر الفبایی باشد";
                    }
                    else if (error.Description == "Passwords must have at least one lowercase ('a'-'z').")
                    {
                        error.Description = ".کلمه عبور باید شامل حداقل یک حرف کوچک انگلیسی باشد";
                    }
                    else if (error.Description == "Passwords must have at least one uppercase ('A'-'Z').")
                    {
                        error.Description = ".کلمه عبور باید شامل حداقل یک حرف بزرگ انگلیسی باشد";
                    }
                    else if (error.Description == "The Password field is required.")
                    {
                        error.Description = ".کلمه عبور وارد نشده است";
                    }
                    else if (error.Description == "The Username field is required.")
                    {
                        error.Description = ".نام کاربری وارد نشده است";
                    }
                    else if (error.Description == "User name '" + user.UserName + "' is invalid, can only contain letters or digits.")
                    {
                        error.Description = ".نام کاربری غیرقابل قبول است";
                    }
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}