using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using BehineNiroo.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace BehineNiroo
{
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class EditUsersInRoleModel : PageModel
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly BehineNirooContext _context;

        public EditUsersInRoleModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager
                                    , BehineNirooContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }

        public class UserRole
        {
            public string UserId { get; set; }

            public string UserName { get; set; }

            public bool IsSelected { get; set; }
        }

        public List<UserRole> model = new List<UserRole>();

        string cs = Globals.SecurityConnection;

        public IActionResult OnGet(string roleId)
        {
            var role = roleManager.FindByIdAsync(roleId).GetAwaiter().GetResult();
            if (role == null)
            {
                return NotFound();
            }
            
            //Globals.Idd = role.Id;

            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT Id , UserName, RoleId FROM aspnetusers au " +
                    "left outer join aspnetuserroles aur on au.Id = aur.UserId", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Convert.ToString(reader["RoleId"]) == role.Id)
                        {
                            model.Add(new UserRole()
                            {
                                UserId = Convert.ToString(reader["Id"]),
                                UserName = Convert.ToString(reader["UserName"]),
                                IsSelected = true
                            });
                        }
                        else
                        {
                            model.Add(new UserRole()
                            {
                                UserId = Convert.ToString(reader["Id"]),
                                UserName = Convert.ToString(reader["UserName"]),
                                IsSelected = false
                            });
                        }
                    }
                }
            }

            /*foreach (var user in userManager.Users)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                }

                if (userManager.IsInRoleAsync(user, role.Name) != null)
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }

                model.Add(userRole);
            }*/
            return Page();
        }

        public IActionResult OnPost(List<UserRole> model, string roleId)
        {
            var role = roleManager.FindByIdAsync(roleId).GetAwaiter().GetResult();
            if (role == null)
            {
                return NotFound();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = userManager.FindByIdAsync(model[i].UserId).GetAwaiter().GetResult();

                IdentityResult result = null;

                if (model[i].IsSelected && !(userManager.IsInRoleAsync(user, role.Name).GetAwaiter().GetResult()))
                {
                    result = userManager.AddToRoleAsync(user, role.Name).GetAwaiter().GetResult();
                }
                else if (!(model[i].IsSelected) && userManager.IsInRoleAsync(user, role.Name).GetAwaiter().GetResult())
                {
                    result = userManager.RemoveFromRoleAsync(user, role.Name).GetAwaiter().GetResult();
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToPage("/Administration/EditRole", new { Id = roleId });
                }
            }
            _context.SaveChangesAsync();
            return RedirectToPage("/Administration/EditRole", new { Id = roleId });
        }
    }
}