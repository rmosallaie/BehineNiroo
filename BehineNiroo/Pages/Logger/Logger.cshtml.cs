using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using BehineNiroo.Models;
using BehineNiroo.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace BehineNiroo
{
    public class LoggerModel : PageModel
    {
        private readonly BehineNirooContext context;

        public LoggerModel(BehineNirooContext _context)
        {
            context = _context;
        }

        public List<String> CityList { get; set; } = new List<string>();

        public List<String> PostList { get; set; } = new List<string>();

        public List<String> FeederList { get; set; } = new List<string>();

        public List<String> LogTableList { get; set; } = new List<string>();

        //public List<String> CategoriesList { get; set; } = new List<string>() { "Current", "Frequency" , "Harmonics" , "PF" ,
        //                                                                       "Power", "THD","Voltage","Other"};
        public List<String> CurrentList { get; set; } = new List<string>() { "I1min","I1","I1max", "I2min", "I2", "I2max",
                                                                               "I3min","I3","I3max",};

        public List<String> FrequencyList { get; set; } = new List<string>() { "Freqmin", "Freq", "Freqmax" };

        public List<String> HarmonicsList { get; set; } = new List<string>() { "Harm3V1", "Harm5V1", "Harm7V1", "Harm9V1", "Harm3V2", "Harm5V2", "Harm7V2", "Harm9V2",
                                                                                "Harm3V3", "Harm5V3", "Harm7V3", "Harm9V3", "Harm3I1", "Harm5I1", "Harm7I1", "Harm9I1",
                                                                                  "Harm3I2", "Harm5I2", "Harm7I2", "Harm9I2", "Harm3I3", "Harm5I3", "Harm7I3", "Harm9I3"};

        public List<String> PFList { get; set; } = new List<string>() { "PFtotmin", "PFtot", "PFtotmax" };

        public List<String> PowerList { get; set; } = new List<string>() { "Ptotmin", "Ptot", "Ptotmax", "Qtotmin", "Qtot", "Qtotmax", "Stotmin", "Stot", "Stotmax" };

        public List<String> THDList { get; set; } = new List<string>() { "THDV1min", "THDV1", "THDV2min", "THDV2", "THDV2max", "THDV3min", "THDV3", "THDV3max", "THDI1min",
                                                                        "THDI1", "THDI1max", "THDI2min", "THDI2", "THDI2max", "THDI3min", "THDI3", "THDI3max"};

        public List<String> VoltagetList { get; set; } = new List<string>() { "V1min", "V1", "V1max", "V12min", "V12", "V12max",
                                                                                "V2min", "V2", "V2max","V23min", "V23", "V23max", "V3min",
                                                                                    "V3", "V3max","V31min", "V31", "V31max" };

        public List<String> OtherList { get; set; } = new List<string>();

        public DateTime FromDate { get; set; } = new DateTime();
        public DateTime ToDate { get; set; } = new DateTime();

        public PersianDateTime StartDate { get; set; }
        public PersianDateTime EndDate { get; set; }

        public class InputModel     // For Input Attribute
        {

            //[Required]
            public List<string> City { get; set; }

            //[Required]
            public List<string> Post { get; set; }

            //[Required]
            public List<string> Feeder { get; set; }

            //[Required]
            public List<string> Logtable { get; set; }

            [Required(ErrorMessage =".حداقل یک مورد را انتخاب نمایید")]
            public List<string> Categories { get; set; }

            public string SettingName { get; set; }

            public bool SettingCheckbox { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string CityPostFeederListItems { get; set; }

        string cs = Globals.PocketSwitchConnection;

        public void OnGet()
        {
            CityPostFeederListItems = Citynames();

            FromDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss");
            FromDate = DateTime.Today.AddDays(-5);
            ToDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss");
            ToDate = DateTime.Today;

            StartDate = FromDate.ToPersianDateTime();
            EndDate = ToDate.ToPersianDateTime();

            return;
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
                       "<input type=checkbox id=Input_City name=Input.City value=" + item + ">" +
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

                    while(reader.Read())
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
                    "<input type=checkbox id=Input_Post name=Input.Post value=" + item + ">" +
                    "<lable asp-for=Input.Post> " + item + "</lable><ul class=nested id=id-" + cityname + "-id-id-" + item + ">" +
                    Feedernames(cityname, item) + "</ul></li >";
            }

            return txt;
        }

        public string Feedernames(string cityname,string postname)
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
                txt += "<li><i class=\"fas fa-angle-right rotate mr-1\"></i><input type=checkbox id=Input_Feeder name=Input.Feeder value=" + cityname + "_" + postname + "_" + item + ">" +
                    "<lable asp-for=Input.Feeder> " + item + "</lable><ul class=nested id=" + cityname + "-id-" + postname + "-id-" + item + ">" +
                    LogTables(cityname,postname,item) + "</ul></li>";
            }

            return txt;
        }


        public string LogTables(string cityname, string postname, string feedername)
        {
            string txt = "";

            string query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'logger' and "+
                            "table_name like 'log" + feedername +"$%'";
            LogTableList.Clear();
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        LogTableList.Add(Convert.ToString(reader["TABLE_NAME"]));
                    }

                    reader.Close();
                    connection.Close();
                }
            }

            int i;

            string Current_txt = "";
            string Frequency_txt = "";
            string Harmonics_txt = "";
            string PF_txt = "";
            string Power_txt = "";
            string THD_txt = "";
            string Voltage_txt = "";
            string Other_txt = "";

            foreach (var LogTableitem in LogTableList)
            {
                i = LogTableList.IndexOf(LogTableitem);

                foreach (var CurrentItem in CurrentList)
                {
                    Current_txt += "<li><input type=checkbox id=Input_Categories name=Input.Categories value=" +
                                    CurrentItem + "_" + cityname + "_" + postname + "_" + LogTableitem + " /> " +
                                    "<lable asp-for=Input.Categories>" + CurrentItem + "</lable></li>";
                }

                foreach (var FrequencyItem in FrequencyList)
                {
                    Frequency_txt += "<li><input type=checkbox id=Input_Categories name=Input.Categories value=" +
                                    FrequencyItem + "_" + cityname + "_" + postname + "_" + LogTableitem + " /> " +
                                    "<lable asp-for=Input.Categories>" + FrequencyItem + "</lable></li>";
                }

                foreach (var HarmonicsItem in HarmonicsList)
                {
                    Harmonics_txt += "<li><input type=checkbox id=Input_Categories name=Input.Categories value=" +
                                    HarmonicsItem + "_" + cityname + "_" + postname + "_" + LogTableitem + " /> " +
                                    "<lable asp-for=Input.Categories>" + HarmonicsItem + "</lable></li>";
                }

                foreach (var PFItem in PFList)
                {
                    PF_txt += "<li><input type=checkbox id=Input_Categories name=Input.Categories value=" +
                                    PFItem + "_" + cityname + "_" + postname + "_" + LogTableitem + " /> " +
                                    "<lable asp-for=Input.Categories>" + PFItem + "</lable></li>";
                }

                foreach (var PowerItem in PowerList)
                {
                    Power_txt += "<li><input type=checkbox id=Input_Categories name=Input.Categories value=" +
                                    PowerItem + "_" + cityname + "_" + postname + "_" + LogTableitem + " /> " +
                                    "<lable asp-for=Input.Categories>" + PowerItem + "</lable></li>";
                }

                foreach (var THDItem in THDList)
                {
                    THD_txt += "<li><input type=checkbox id=Input_Categories name=Input.Categories value=" +
                                    THDItem + "_" + cityname + "_" + postname + "_" + LogTableitem + " /> " +
                                    "<lable asp-for=Input.Categories>" + THDItem + "</lable></li>";
                }

                foreach (var VoltageItem in VoltagetList)
                {
                    Voltage_txt += "<li><input type=checkbox id=Input_Categories name=Input.Categories value=" +
                                    VoltageItem + "_" + cityname + "_" + postname + "_" + LogTableitem + " /> " +
                                    "<lable asp-for=Input.Categories>" + VoltageItem + "</lable></li>";
                }

                foreach (var OtherItem in OtherList)
                {
                    Other_txt += "<li><input type=checkbox id=Input_Categories name=Input.Categories value=" +
                                    OtherItem + "_" + cityname + "_" + postname + "_" + LogTableitem + " /> " +
                                    "<lable asp-for=Input.Categories>" + OtherItem + "</lable></li>";
                }

                txt += "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox id=Input_Logtable name=Input.Logtable value=" + LogTableitem + ">" +
                       "<lable asp-for=Input.Logtable> " + LogTableitem + "</lable><ul class=nested>" +

                       "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox value=Current><lable>Current</lable>" +
                       "<ul class=nested>" + Current_txt + "</ul></li>" +

                       "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox value=Frequency><lable>Frequency</lable>" +
                       "<ul class=nested>" + Frequency_txt + "</ul></li>" +

                       "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox value=Harmonics><lable>Harmonics</lable>" +
                       "<ul class=nested>" + Harmonics_txt + "</ul></li>" +

                       "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox value=PF><lable>PF</lable>" +
                       "<ul class=nested>" + PF_txt + "</ul></li>" +

                       "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox value=THD><lable>THD</lable>" +
                       "<ul class=nested>" + THD_txt + "</ul></li>" +

                       "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox value=Voltage><lable>Voltage</lable>" +
                       "<ul class=nested>" + Voltage_txt + "</ul></li>" +

                       "<li><i class=\"fas fa-angle-right rotate mr-1\"></i>" +
                       "<input type=checkbox value=Other><lable>Other</lable>" +
                       "<ul class=nested>" + Other_txt + "</ul></li>" +

                       "</ul></li>";
            }

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
            PersianDateTime persianDate1 = PersianDateTime.Parse(SDate.ToString("yyyy/MM/dd"));
            DateTime miladiDate1 = persianDate1.ToDateTime();
            string fdate = miladiDate1.ToString("yyyy-MM-dd");

            PersianDateTime persianDate2 = PersianDateTime.Parse(EDate.ToString("yyyy/MM/dd"));
            DateTime miladiDate2 = persianDate2.ToDateTime();
            string tdate = miladiDate2.ToString("yyyy-MM-dd");

            if (ModelState.IsValid)
            {
                string cat = "";

                foreach (var item in Input.Categories)
                {
                    cat = cat + item + ",";
                }

                cat = cat.Remove(cat.Length - 1);

                if (Input.SettingCheckbox && Input.SettingName != "")
                {
                    string connectionstring = Globals.SavingsConnection;
                    string Qry = "INSERT into savings.savings(name,type,items,fromdate,todate) VALUES ('" + Input.SettingName + "','logger','" + cat + "'" +
                        ",'" + fdate + "','" + tdate + "');";
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

                Response.Redirect("/Logger/ShowLogs/fdate=" + fdate + "/tdate=" + tdate +
                    /*"/city=" + city + "/post=" + post + "/feeder=" + feeder + "/logtable=" + logtbl +*/
                    "/categories=" + cat);

                ModelState.AddModelError(string.Empty, "Fill the Required Field");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Fill the Required Field");

                FromDate = DateTime.Now.AddMonths(-1).Date;
                ToDate = DateTime.Now.Date;

                OnGet();
            }
        }

    }
}