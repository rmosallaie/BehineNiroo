using System;
using System.Collections.Generic;
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
    public class RealTimeEventsModel : PageModel
    {
        private readonly BehineNirooContext context;

        public RealTimeEventsModel(BehineNirooContext _context)
        {
            context = _context;
        }

        string cs = Globals.EventConnection;

        string query;

        [BindProperty]
        public string[] feedersSplit { get; set; }

        public static List<string>[] SameItems { get; set; }

        public static string Feeder { get; set; }

        [BindProperty]
        public List<Event>[] FinalOutput { get; set; }

        public int RTEC { get; set; }

        public void OnGet(string feeder, int RealTimeEventCount)
        {
            RTEC = RealTimeEventCount;
            string[] ItemsSplit;
            int len = 0;
            Feeder = feeder;

            feedersSplit = feeder.Split(",");

            FinalOutput = new List<Event>[feedersSplit.Length];

            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                connection.Open();

                foreach (var item in feedersSplit)
                {
                    ItemsSplit = item.Split("_");
                    query = "select EventNumber,EventTime,MilliTime,EventValue," +
                            "EventReason,EventType,CityName,PostName,FeederName from event.event ee inner join event.eventsetting es " +
                            "on es.SerialNum = ee.SerialNum Where CityName='" + ItemsSplit[0]
                            + "' and PostName = '" + ItemsSplit[1]
                            + "' and FeederName = '" + ItemsSplit[2]
                            + "' order by EventNumber Desc limit " + RealTimeEventCount;

                    FinalOutput[len] = new List<Event>();

                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                    {
                        MySqlDataReader reader = mySqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            FinalOutput[len].Add(new Event()
                            {
                                EventNumber = Convert.ToUInt32(reader["EventNumber"]),
                                EventTime = Convert.ToDateTime(reader["EventTime"]).ToPersianDateTime().ToString(),
                                MilliTime = int.Parse(reader["MilliTime"].ToString()),
                                EventValue = Convert.ToString(reader["EventValue"]),
                                EventReason = Convert.ToString(reader["EventReason"]),
                                EventType = int.Parse(reader["EventType"].ToString()),
                                CityName = Convert.ToString(reader["CityName"]),
                                PostName = Convert.ToString(reader["PostName"]),
                                FeederName = Convert.ToString(reader["FeederName"])
                            });
                        }
                        reader.Close();
                        FinalOutput[len].OrderByDescending(x => x.EventTime);
                    }
                    len++;
                }
            }
        }


        public IActionResult OnGetNewEvents(int limit)
        {
            string[] ItemsSplit;
            int len = 0;
            feedersSplit = Feeder.Split(",");

            FinalOutput = new List<Event>[feedersSplit.Length];

            using (MySqlConnection mySqlConnection = new MySqlConnection(cs))
            {
                mySqlConnection.Open();

                foreach (var item in feedersSplit)
                {
                    ItemsSplit = item.Split("_");
                    query = "select EventNumber,EventTime,MilliTime,EventValue," +
                            "EventReason,EventType,CityName,PostName,FeederName from event.event ee inner join event.eventsetting es " +
                            "on es.SerialNum = ee.SerialNum Where CityName='" + ItemsSplit[0]
                            + "' and PostName = '" + ItemsSplit[1]
                            + "' and FeederName = '" + ItemsSplit[2]
                            + "' order by EventNumber Desc limit " + limit;

                    FinalOutput[len] = new List<Event>();

                    using (MySqlCommand Command = new MySqlCommand(query, mySqlConnection))
                    {
                        MySqlDataReader reader = Command.ExecuteReader();
                        while (reader.Read())
                        {
                            FinalOutput[len].Add(new Event()
                            {
                                EventNumber = Convert.ToUInt32(reader["EventNumber"]),
                                EventTime = Convert.ToDateTime(reader["EventTime"]).ToPersianDateTime().ToString(),
                                MilliTime = int.Parse(reader["MilliTime"].ToString()),
                                EventValue = Convert.ToString(reader["EventValue"]),
                                EventReason = Convert.ToString(reader["EventReason"]),
                                EventType = int.Parse(reader["EventType"].ToString()),
                                CityName = Convert.ToString(reader["CityName"]),
                                PostName = Convert.ToString(reader["PostName"]),
                                FeederName = Convert.ToString(reader["FeederName"])
                            });
                        }
                        reader.Close();
                        FinalOutput[len].OrderByDescending(x => x.EventTime);
                    }
                    len++;
                }
            }

            JsonResult json = new JsonResult(FinalOutput);

            return json;
        }
























        //public IActionResult OnGetDatabase()
        //{
        //    int tmp;
        //    using (MySqlConnection connection = new MySqlConnection(cs))
        //    {
        //        query = "insert  into `event`(`SerialNum`,`EventNumber`,`EventValue`,`EventTime`,`MilliTime`,`EventReason`,`EventType`,`Ack`,`UserCode`) " +
        //            "values (2774,0,'58821','2020-10-13 08:37:20',790,'Mrm',1,0,0)";

        //        connection.Open();
        //        using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
        //        {
        //            tmp = mySqlCommand.ExecuteNonQuery();
        //        }
        //    }

        //    JsonResult json = new JsonResult(tmp);

        //    return json;
        //}
    }
}