using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace BehineNiroo.Pages.Online
{
    public class OnlineModel : PageModel
    {
        private readonly BehineNirooContext context;

        public OnlineModel(BehineNirooContext _context)
        {
            context = _context;
        }


        public class InputModel     // For Input Attribute
        {

            //[Required]
            public List<string> City { get; set; }

            //[Required]
            public List<string> Post { get; set; }

            //[Required]
            public List<string> Feeder { get; set; }

            public List<string> FieldGroup { get; set; }

            [Required(ErrorMessage = ".حداقل یک مورد را انتخاب نمایید")]
            public List<string> Parameters { get; set; }

            public string SettingName { get; set; }

            public bool SettingCheckbox { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();


        public List<String> CityList { get; set; } = new List<string>();

        public List<String> PostList { get; set; } = new List<string>();

        public List<String> FeederList { get; set; } = new List<string>();

        string cs = Globals.PocketSwitchConnection;

        public string CityPostFeederListItems { get; set; }


        //public static List<string>[] FieldGroup;

        public void OnGet()
        {
            
            if (Globals.OnlinePageLoadCount == 0)
            {
                CityPostFeederListItems = Citynames();
                Globals.OnlinePageLoadCount++;
                Globals.OnlineTree = CityPostFeederListItems;
            }
            else
            {
                CityPostFeederListItems = Globals.OnlineTree;
            }
        }

        public string Citynames()
        {
            string txt = "";

            string query = "select distinct CityName from pocketswitch.deviceinfo";
            //CityList.Clear();
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        CityList.Add(Convert.ToString(reader["CityName"]));
                    }

                    reader.Close();
                    connection.Close();
                }
            }

            foreach (var item in CityList)
            {
                txt += "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox class=Input.City id=Input_City name=Input.City value=" + item + ">" +
                       "<lable asp-for=Input.City> " + item + "</lable><ul class=nested id=id-" + item + "-id>" +
                       Postnames(item) + "</ul></li>";
            }

            return txt;
        }

        public string Postnames(string cityname)
        {
            string txt = "";

            string query = "select distinct PostName from pocketswitch.deviceinfo where CityName = '" + cityname + "'";
            PostList.Clear();
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        PostList.Add(Convert.ToString(reader["PostName"]));
                    }

                    reader.Close();
                    connection.Close();
                }
            }

            foreach (var item in PostList)
            {
                txt += "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                    "<input type=checkbox class=Input.Post id=Input_Post name=Input.Post value=" + item + ">" +
                    "<lable asp-for=Input.Post> " + item + "</lable><ul class=nested id=id-" + cityname + "-id-id-" + item + ">" +
                    Feedernames(cityname, item) + "</ul></li >";
            }

            return txt;
        }

        public string Feedernames(string cityname, string postname)
        {
            string txt = "";

            string query = "select distinct FeederName from pocketswitch.deviceinfo where CityName = '" + cityname + "' and PostName = '" + postname + "'";
            FeederList.Clear();
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        FeederList.Add(Convert.ToString(reader["FeederName"]));
                    }

                    reader.Close();
                    connection.Close();
                }
            }
            foreach (var item in FeederList)
            {
                txt += "<li><i class=\"fas fa-angle-right rotate mr-1\"></i><input type=checkbox class=Input.Feeder id=Input_Feeder name=Input.Feeder value=" + cityname + "_" + postname + "_" + item + ">" +
                    "<lable asp-for=Input.Feeder> " + item + "</lable><ul class=nested>" + 
                    SerialNumber(cityname, postname, item) + "</ul></li>";
            }
            return txt;
        }

        public string SerialNumber(string cityname, string postname, string feedername)
        {
            string txt = "";
            string SerialNum = "";
            int NumberOfFields = 0;
            int NumberOfFielGroups = 0;
            int tmp = 0;
            List<string> P = new List<string>();
            List<string> U = new List<string>();
            List<string>[] FieldGroup;
            

            string query = "select SerialNumber from PocketSwitch.DeviceInfo where CityName='" + cityname + "' and PostName='" + postname + "' and FeederName='" + feedername + "'";

            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();
                    reader.Read();
                    SerialNum = Convert.ToString(reader["SerialNumber"]);

                    reader.Close();
                    
                }
                query = "SELECT * FROM Manager.NameAndUnit WHERE SerialNumber='" + SerialNum + "'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(command))
                    {
                        da.Fill(dataTable);

                        for (int i = 1; i < dataTable.Columns.Count; i = i+2)
                        {
                            if (dataTable.Rows[0][i].ToString() == "")
                            {
                                break;
                            }
                            P.Add(Convert.ToString(dataTable.Rows[0][i]));
                            U.Add(Convert.ToString(dataTable.Rows[0][i + 1]));
                            NumberOfFields++;
                        }
                    }
                }
                connection.Close();
                query = "SELECT FieldGroup FROM PocketSwitch.FieldGrouping GROUP BY FieldGroup";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        NumberOfFielGroups++;
                    }
                    reader.Close();
                    connection.Close();

                    connection.Open();
                    reader = command.ExecuteReader();

                    FieldGroup = new List<string>[NumberOfFielGroups];

                    int i = 0;
                    while (reader.Read())
                    {
                        FieldGroup[i] = new List<string>();
                        FieldGroup[i].Add(Convert.ToString(reader["FieldGroup"]));
                        i++;
                    }
                    reader.Close();
                }
                connection.Close();
                for (int i = 0; i < FieldGroup.Length; i++)
                {
                    foreach (var item in P)
                    {
                        query = "Select FieldName, FieldGroup FROM PocketSwitch.FieldGrouping WHERE FieldGroup='" + FieldGroup[i][0] + "' AND FieldName='" + item + "'";
                        using (MySqlCommand mySqlCommand = new MySqlCommand(query,connection))
                        {
                            connection.Open();
                            MySqlDataReader reader = mySqlCommand.ExecuteReader();
                            reader.Read();
                            if (reader.HasRows)
                            {
                                FieldGroup[i].Add(item + "(" + U[P.IndexOf(item)] + ")");
                            }
                            reader.Close();
                        }
                        connection.Close();
                    }
                }
                connection.Close();
            }
            
            for (int i = 0; i < NumberOfFielGroups; i++)
            {
                txt += "<li><i class=\"fas fa-angle-right rotate mr-1\"></i><input type=checkbox id=Input_FieldGroup class=Input.FieldGroup name=Input.FieldGroup>" +
                       "<lable asp-for=Input.FieldGroup> " + FieldGroup[i][0] + "</lable><ul class=nested>";

                for (int j = 1; j < FieldGroup[i].Count; j++)
                {
                    tmp = P.IndexOf(FieldGroup[i][j].Substring(0, FieldGroup[i][j].LastIndexOf("("))) + 1;
                    txt += "<li><input type=checkbox class=Input.Parameters id=Input_Parameters name=Input.Parameters value=" + cityname + "_" + postname + "_" + feedername + "_" + FieldGroup[i][j] +
                            "_P" + tmp +"><lable asp-for=Input.Parameters> " + FieldGroup[i][j] + 
                            "</lable></li>";
                }

                txt += "</ul></li>";
            }

            txt += "<li><input type=checkbox class=Input.Parameters id=Input_Parameters name=Input.Parameters class=Input.Parameters><lable asp-for=Input.Parameters> Other</li>";

            return txt;
        }

        public IActionResult OnGetSettingNameExist(string SettingName)
        {
            bool result;

            string connectionstring = Globals.SavingsConnection;
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
            {
                string query = "select * from savings.savings where name = '" + SettingName + "'";
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    mySqlConnection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    result = reader.HasRows;
                }
                mySqlConnection.Close();
            }
            return new JsonResult(result);
        }

        public void OnPost(DateTime SDate, DateTime EDate)
        {

            if (ModelState.IsValid)
            {
                string parameters = "";

                foreach (var item in Input.Parameters)
                {
                    parameters = parameters + item + ",";
                }

                parameters = parameters.Remove(parameters.Length - 1);

                if (Input.SettingCheckbox && Input.SettingName != "")
                {
                    string connectionstring = Globals.SavingsConnection;
                    string Qry = "INSERT into savings.savings(name,type,items) VALUES ('" + Input.SettingName + "','online','" + parameters + "');";
                    using (MySqlConnection connection = new MySqlConnection(connectionstring))
                    {
                        using (MySqlCommand mySqlCommand = new MySqlCommand(Qry, connection))
                        {
                            connection.Open();
                            mySqlCommand.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
                Globals.OnlineParameters = parameters;
                Response.Redirect("/Online/ShowOnline/");
                Input.Parameters = null;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Fill the Required Field");

                OnGet();
            }
            return ;
        }
    }
}