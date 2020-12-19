using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Models;
using BehineNiroo.Pages;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace BehineNiroo
{
    public class LoadSettingModel : PageModel
    {

        public static string ITEMS;

        [BindProperty]
        public Setting setting { get; set; } = new Setting();

        string cs = Globals.SavingsConnection;

        public void OnGet(int id)
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
                        setting.FromDate = Convert.ToString(reader["fromdate"]);
                        setting.ToDate = Convert.ToString(reader["todate"]);
                    }
                    connection.Close();
                }
            }
            ITEMS = setting.Items;
        }

        public void OnPost(string type,string fromdate, string todate)
        {
            switch (type)
            {
                case "event":
                    Response.Redirect("/Event/ShowEventTable/fdate=" + fromdate + "/tdate=" + todate + "/feeder=" + ITEMS + "/sie=2");
                    break;
                case "energy":
                    Response.Redirect("/Energy/ShowEnergyTable/fdate=" + fromdate + "/tdate=" + todate + "/feeder=" + ITEMS);
                    break;
                case "logger":
                    Response.Redirect("/Logger/ShowLogs/fdate=" + fromdate + "/tdate=" + todate + "/categories=" + ITEMS);
                    break;
                case "online":
                    Globals.OnlineParameters = ITEMS;
                    Response.Redirect("/Online/ShowOnline/");
                    break;
                default:
                    break;
            }
        }
    }
}