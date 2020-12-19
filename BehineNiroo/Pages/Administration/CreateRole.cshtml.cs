using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BehineNiroo
{
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class CreatRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public CreatRoleModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [BindProperty]
        public Role NewRole { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = NewRole.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Administration/ListRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Page();
        }
    }
}