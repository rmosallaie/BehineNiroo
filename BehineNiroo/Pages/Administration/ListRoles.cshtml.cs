using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
    public class ListRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public ListRolesModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        string cs = Globals.SecurityConnection;

        //public IEnumerable<IdentityRole> roles { get; set; }
        public List<IdentityRole> roles { get; set; } = new List<IdentityRole>();

        public IActionResult OnGet()
        {
            MySqlConnection connection = new MySqlConnection(cs);
            connection.Open();

            string query = "Select Id, Name from aspnetroles";
            MySqlCommand mySqlCommand = new MySqlCommand(query,connection);
            

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while(reader.Read())
            {
                roles.Add(new IdentityRole()
                { 
                    Id = Convert.ToString(reader["Id"]),
                    Name = Convert.ToString(reader["Name"])
                });
            }
            reader.Close();
            connection.Close();
            //roles = roleManager.Roles;

            return Page();
        }
    }
}