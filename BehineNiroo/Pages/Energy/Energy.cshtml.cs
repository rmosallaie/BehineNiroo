﻿using System;
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
    public class EnergyModel : PageModel
    {
        private readonly BehineNirooContext context;

        public EnergyModel(BehineNirooContext context)
        {
            this.context = context;
        }


        public DateTime FromDate { get; set; } = new DateTime();
        public DateTime ToDate { get; set; } = new DateTime();

        public PersianDateTime StartDate { get; set; }
        public PersianDateTime EndDate { get; set; }

        public List<String> CityList { get; set; } = new List<string>();

        public List<String> PostList { get; set; } = new List<string>();

        public List<String> FeederList { get; set; } = new List<string>();

        public class InputModel     // For Input Attribute
        {

            //[Required]
            public List<string> City { get; set; }

            //[Required]
            public List<string> Post { get; set; }

            [Required(ErrorMessage = ".حداقل یک مورد را انتخاب نمایید")]
            public List<string> Feeder { get; set; }

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
            FromDate = DateTime.Today.AddMonths(-1);
            ToDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss");
            ToDate = DateTime.Today;

            StartDate = FromDate.ToPersianDateTime();
            EndDate = ToDate.ToPersianDateTime();
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
                    "<input type=checkbox id=Input_Post name=Input.Post value=" + item + ">" +
                    "<lable asp-for=Input.Post> " + item + "</lable><ul class=nested id=id-" + cityname + "-id-id-" + item + ">" +
                    Feedernames(cityname, item) + "</ul></li >";
            }

            //JsonResult json = new JsonResult(PostList);

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
                txt += "<li><input type=checkbox id=Input_Feeder name=Input.Feeder value=" + cityname + "_" + postname + "_" + item + ">" +
                    "<lable asp-for=Input.Feeder> " + item + "</lable></li>";
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
                string feeder = "";

                foreach (var item in Input.Feeder)
                {
                    feeder = feeder + item + ",";
                }

                feeder = feeder.Remove(feeder.Length - 1);

                if (Input.SettingCheckbox && Input.SettingName != "")
                {
                    string connectionstring = Globals.SavingsConnection;
                    string Qry = "INSERT into savings.savings(name,type,items,fromdate,todate) VALUES ('" + Input.SettingName + "','energy','" + feeder + "'" +
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

                Response.Redirect("/Energy/ShowEnergyTable/fdate=" + fdate + "/tdate=" + tdate + "/feeder=" + feeder);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Fill the Required Field");

                FromDate = DateTime.Now.AddMonths(-1).Date;
                ToDate = DateTime.Now.Date;
                
                OnGet();
            }
            return;
        }

    }
}