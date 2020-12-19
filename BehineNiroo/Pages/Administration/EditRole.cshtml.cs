using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using BehineNiroo.Models;
using BehineNiroo.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace BehineNiroo
{
    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    public class EditRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly BehineNirooContext _context;

        public EditRoleModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager,
                            BehineNirooContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }


        public class EditRole
        {
            public EditRole()
            {
                Users = new List<string>();
            }
            public string Id { get; set; }

            [Required(ErrorMessage = "Role Name is required")]
            public string RoleName { get; set; }

            public List<string> Users { get; set; }

        }

        string cs = Globals.SecurityConnection;

        [BindProperty]
        public EditRole editRole { get; set; } = new EditRole();

        public IActionResult OnGet(string id)
        {
            var role = roleManager.FindByIdAsync(id).GetAwaiter().GetResult();

            if (role == null)
            {
                return LocalRedirect("/NotFound");
            }

            editRole.Id = role.Id;
            Globals.Idd = role.Id;
            editRole.RoleName = role.Name;

            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand("Select UserId FROM aspnetuserroles " +
                        "where RoleId = '" + role.Id + "'", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();
                    while(reader.Read())
                    {
                        using (MySqlConnection conn = new MySqlConnection(cs))
                        {
                            using (MySqlCommand mySqlCmd = new MySqlCommand("Select UserName FROM aspnetusers " +
                                    "where Id = '" + Convert.ToString(reader["UserId"]) + "'", conn))
                            {
                                conn.Open();
                                MySqlDataReader ReadeR = mySqlCmd.ExecuteReader();
                                while (ReadeR.Read())
                                {
                                    editRole.Users.Add(Convert.ToString(ReadeR["UserName"]));
                                        
                                }
                            }
                        }
                    }
                }
            }

            //foreach (var user in userManager.Users)
            //{
            //    //if (userManager.IsInRoleAsync(user, role.Name).GetAwaiter())
            //    //{
            //    //    editRole.Users.Add(user.UserName);
            //    //}
            //    using(MySqlConnection connection = new MySqlConnection(cs))
            //    {
            //        using(MySqlCommand mySqlCommand = new MySqlCommand("Select UserId FROM event.aspnetuserroles"+
            //                "where RoleId = '" + role.Id + "'", connection))
            //        {

            //        }
            //    }
            //}
            return Page();
        }

        public IActionResult OnPost()
        {
            var role = roleManager.FindByIdAsync(Globals.Idd).GetAwaiter().GetResult();

            if (role == null)
            {
                return NotFound();
            }
            else
            {
                role.Name = editRole.RoleName;
                var result = roleManager.UpdateAsync(role).GetAwaiter().GetResult();
                _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    return RedirectToPage("/Administration/ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();
            }
        }
    }
}