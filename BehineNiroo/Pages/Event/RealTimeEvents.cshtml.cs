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

        public List<Event> RealTimeEvents { get; set; } = new List<Event>();

        string cs = Globals.EventConnection;

        string query;

        public void OnGet()
        {
            query = "select EventNumber,EventTime,MilliTime,EventValue," +
            "EventReason,EventType,CityName,PostName,FeederName from event.event ee inner join event.eventsetting es " +
            "on es.SerialNum = ee.SerialNum order by EventNumber Desc limit 20";

            if (ModelState.IsValid)
            {
                using(MySqlConnection connection = new MySqlConnection(cs))
                {
                    using(MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        MySqlDataReader reader = mySqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            RealTimeEvents.Add(new Event()
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
                    }
                }
            }
        }


        public IActionResult OnGetNewEvents(int limit)
        {
            query = "select EventNumber,EventTime,MilliTime,EventValue," +
            "EventReason,EventType,CityName,PostName,FeederName from event.event ee inner join event.eventsetting es " +
            "on es.SerialNum = ee.SerialNum order by EventNumber Desc limit " + limit;
            RealTimeEvents.Clear();
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        RealTimeEvents.Add(new Event()
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
                    connection.Close();
                }
            }

            JsonResult json = new JsonResult(RealTimeEvents.ToArray());

            return json;
        }

        public IActionResult OnGetDatabase()
        {
            int tmp;
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                query = "insert  into `event`(`SerialNum`,`EventNumber`,`EventValue`,`EventTime`,`MilliTime`,`EventReason`,`EventType`,`Ack`,`UserCode`) " +
                    "values (2774,0,'58821','2020-10-13 08:37:20',790,'Mrm',1,0,0)";

                connection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                {
                    tmp = mySqlCommand.ExecuteNonQuery();
                }
            }

            JsonResult json = new JsonResult(tmp);

            return json;
        }
    }
}