using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace BehineNiroo.Pages.Online
{
    public class ShowOnlineModel : PageModel
    {

        [BindProperty]
        public string[] ParametersSplit { get; set; }

        public static List<string>[] SameItems { get; set; }

        [BindProperty]
        public List<string>[] FinalOutput { get; set; }

        public static int ParameterLenght { get; set; }

        public void OnGet()
        {
            string cs = Globals.PocketSwitchConnection;
            string query = "";
            string[] SameItemsSplit;
            string SerialNum = "";
            string parameters = Globals.OnlineParameters;

            //////// Sparation of input parameters
            ///
            ParametersSplit = parameters.Split(",");


            ParameterLenght = ParametersSplit.Length;
            SameItems = new List<string>[ParametersSplit.Length];

            for (int i = 0; i < ParametersSplit.Length; i++)
            {
                SameItems[i] = new List<string>();
            }

            int len = 0;
                                                            ////////////// Seprate Same Parameters
            while (ParametersSplit.Length != 0)
            {
                SameItems[len].Add(ParametersSplit[0]);
                for (int i = 1; i < ParametersSplit.Length; i++)
                {
                    if (string.Compare(ParametersSplit[0].Substring(0, ParametersSplit[0].Substring(0, ParametersSplit[0].LastIndexOf("_")).LastIndexOf("_")),
                        ParametersSplit[i].Substring(0, ParametersSplit[i].Substring(0, ParametersSplit[i].LastIndexOf("_")).LastIndexOf("_"))) == 0)
                    {
                        SameItems[len].Add(ParametersSplit[i]);
                        ParametersSplit = ParametersSplit.Where(val => val != ParametersSplit[0]).ToArray();
                        i--;
                    }
                }
                ParametersSplit = ParametersSplit.Where(val => val != ParametersSplit[0]).ToArray();
                len++;
            }

                                                    ///////////////// Deleting Extra Array
            foreach (var item in SameItems)
            {
                if (item.Count == 0)
                {
                    SameItems = SameItems.Where(val => val != item).ToArray();
                }
            }

            FinalOutput = new List<string>[SameItems.Length];

            for (int i = 0; i < SameItems.Length; i++)
            {
                FinalOutput[i] = new List<string>();
            }

            for (int i = 0; i < SameItems.Length; i++)
            {
                foreach (var item in SameItems[i])
                {
                    SameItemsSplit = item.Split("_");
                    using (MySqlConnection connection = new MySqlConnection(cs))
                    {
                        query = "Select SerialNumber from PocketSwitch.DeviceInfo where CityName='" + SameItemsSplit[0] +"' and PostName='" + 
                                SameItemsSplit[1] + "' and FeederName = '" + SameItemsSplit[2] + "'";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            connection.Open();
                            MySqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            SerialNum = Convert.ToString(reader["SerialNumber"]);

                            reader.Close();
                            connection.Close();
                        }
                        query = "Select " + SameItemsSplit[4] +",TimeCurrent FROM Manager.Parameter WHERE SerialNumber='" + SerialNum + "' order by TimeCurrent Desc limit 1;";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            connection.Open();
                            MySqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            FinalOutput[i].Add(SameItemsSplit[0] + "_" + SameItemsSplit[1] + "_" + SameItemsSplit[2]);
                            FinalOutput[i].Add(SameItemsSplit[3]);
                            FinalOutput[i].Add(Convert.ToString(reader[SameItemsSplit[4]]));
                            //SameItemsSplit[4] = Convert.ToString(reader[SameItemsSplit[4]]);
                            FinalOutput[i].Add(Convert.ToDateTime(reader["TimeCurrent"]).ToPersianDateTime().ToString());

                            connection.Close();
                        }
                    }
                }
            }
        }


        public IActionResult OnGetUpdateOnlineValue()
        
        {
            string[] SameItemsSplit;
            string query = "";
            string cs = Globals.PocketSwitchConnection;
            string SerialNum = "";

            FinalOutput = new List<string>[SameItems.Length];

            for (int i = 0; i < SameItems.Length; i++)
            {
                FinalOutput[i] = new List<string>();
            }

            for (int i = 0; i < SameItems.Length; i++)
            {
                foreach (var item in SameItems[i])
                {
                    SameItemsSplit = item.Split("_");
                    using (MySqlConnection connection = new MySqlConnection(cs))
                    {
                        query = "Select SerialNumber from PocketSwitch.DeviceInfo where CityName='" + SameItemsSplit[0] + "' and PostName='" +
                                SameItemsSplit[1] + "' and FeederName = '" + SameItemsSplit[2] + "'";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            connection.Open();
                            MySqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            SerialNum = Convert.ToString(reader["SerialNumber"]);

                            reader.Close();
                            connection.Close();
                        }
                        query = "Select " + SameItemsSplit[4] + ",TimeCurrent FROM Manager.Parameter WHERE SerialNumber='" + SerialNum + "' order by TimeCurrent Desc limit 1;";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            connection.Open();
                            MySqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            FinalOutput[i].Add(SameItemsSplit[0] + "_" + SameItemsSplit[1] + "_" + SameItemsSplit[2]);
                            FinalOutput[i].Add(SameItemsSplit[3]);
                            FinalOutput[i].Add(Convert.ToString(reader[SameItemsSplit[4]]));
                            //SameItemsSplit[4] = Convert.ToString(reader[SameItemsSplit[4]]);
                            FinalOutput[i].Add(Convert.ToDateTime(reader["TimeCurrent"]).ToPersianDateTime().ToString());

                            connection.Close();
                        }
                    }
                }
            }
            return new JsonResult(FinalOutput);
        }
    }
}