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
using Microsoft.EntityFrameworkCore;

namespace BehineNiroo
{
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class DeleteModel : PageModel
    {
        private readonly BehineNirooContext _context;
        private readonly UserManager<IdentityUser> userManager;
        public DeleteModel(BehineNirooContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }


        [BindProperty]
        public IdentityUser DeleteUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usr = await userManager.FindByIdAsync(id);

            DeleteUser = usr;

            if (DeleteUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Usermanagement/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToPage(NotFound());
            }
        }
    }
}