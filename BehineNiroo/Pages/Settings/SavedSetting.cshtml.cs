using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace BehineNiroo
{
    public class SavedSettingModel : PageModel
    {

        string connectionstring = Globals.SavingsConnection;

        public class Setting
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string items { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }

        public string title { get; set; }

        [BindProperty]
        public List<Setting> setting { get; set; } = new List<Setting>();

        public void OnGet(string type)
        {
            string query = "select * from savings.savings where type='" + type + "';";
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            setting.Add(new Setting()
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                items = Convert.ToString(reader["items"]),
                                Name = Convert.ToString(reader["name"]),
                                FromDate = Convert.ToString(reader["fromdate"]),
                                ToDate = Convert.ToString(reader["todate"])
                            });
                        }
                    }
                    connection.Close();
                }
            }
            switch (type)
            {
                case "energy":
                    title = "انرژی";
                    break;
                case "event":
                    title = "رخداد";
                    break;
                case "online":
                    title = "آنلاین";
                    break;
                case "logger":
                    title = "لاگر";
                    break;
                default:
                    break;
            }
        }
    }
}