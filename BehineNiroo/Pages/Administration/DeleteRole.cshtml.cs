using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BehineNiroo.Pages
{
    public class DeleteRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public DeleteRoleModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public class DeleteRole
        {
            public string Id { get; set; }

            [Required(ErrorMessage = "Role Name is required")]
            public string RoleName { get; set; }

        }

        string cs = Globals.SecurityConnection;

        [BindProperty]
        public DeleteRole deleteRole { get; set; } = new DeleteRole();

        public IActionResult OnGet(string id)
        {
            var role = roleManager.FindByIdAsync(id).GetAwaiter().GetResult();

            if (role == null)
            {
                return LocalRedirect("/NotFound");
            }

            deleteRole.Id = role.Id;
            deleteRole.RoleName = role.Name;

            return Page();
        }

        public async Task<IActionResult> OnPost(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Administration/ListRoles");
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