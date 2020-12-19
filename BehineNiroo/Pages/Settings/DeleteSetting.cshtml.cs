using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Models;
using BehineNiroo.Pages;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;

namespace BehineNiroo
{
    public class DeleteSettingModel : PageModel
    {
        public Setting setting { get; set; } = new Setting();


        string cs = Globals.SavingsConnection;

        public void OnGet(string id)
        {
            string query = "select * from savings.savings where Id='" + id + "';";
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        reader.Read();
                        setting.Id = int.Parse(reader["Id"].ToString());
                        setting.Items = Convert.ToString(reader["items"]);
                        setting.Name = Convert.ToString(reader["name"]);
                        setting.Type = Convert.ToString(reader["type"]);
                    }
                    connection.Close();
                }
            }
        }

        public void OnPost(string id,string type)
        {
            int result;
            string query = "delete from savings.savings where Id='" + id + "';";
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    result = mySqlCommand.ExecuteNonQuery();

                    connection.Close();
                }
            }
            if (result == 1)
            {
                Response.Redirect("/Settings/SavedSetting/type=" + type);
                //return RedirectToPage("/Settings/SavedSetting/");
                //Response.Redirect("", true);
                //return Page();
            }
        }
    }

}