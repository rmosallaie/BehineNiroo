using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using BehineNiroo.Models;
using BehineNiroo.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using OfficeOpenXml;

namespace BehineNiroo
{

    public class ShowEventTableModel : PageModel
    {
        private readonly BehineNirooContext context;

        public ShowEventTableModel(BehineNirooContext _context)
        {
            context = _context;
        }

        public static string query = "";

        public static List<Event> ExcelEvent = new List<Event>();

        [BindProperty]
        public string[] Eventsplit { get; set; }

        [BindProperty]
        public List<Event> events { get; set; } = new List<Event>();

        string cs = Globals.EventConnection;

        public void OnGet(string fdate, string tdate,/* string city, string post,*/ string feeder, int sie)
        {
            string query = "";
            Eventsplit = feeder.Split(",");
            string[] MoreSplit;
            string EventType = "";

            switch (sie)
            {
                case 0:
                    EventType = "('0')";
                    break;
                case 1:
                    EventType = "('1')";
                    break;
                case 2:
                    EventType = "('1','0')";
                    break;
                default:
                    break;
            }
            

            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                connection.Open();
                foreach (var item in Eventsplit)
                {
                    MoreSplit = item.Split("_");

                    query = "select EventNumber,EventTime,MilliTime,EventValue," +
                    "EventReason,EventType, pd.CityName, pd.PostName, pd.FeederName from event.event ee inner join pocketswitch.deviceinfo pd " +
                    "on pd.SerialNumber = ee.SerialNum where EventTime > '" + fdate + "' and EventTime < '" + tdate + "' and" +
                    " CityName='" + MoreSplit[0] + "' and PostName='" + MoreSplit[1] + "' and  FeederName='" + 
                    MoreSplit[2] + "' and EventType in " + EventType + " order by EventTime Desc";

                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                    {
                        MySqlDataReader reader = mySqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            events.Add(new Event()
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
            ExcelEvent = events;

            return;
        }

        //public IActionResult OnPost()
        //{

        //    byte[] fliecontents;

        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("sheet1");

        //        worksheet.Cells[1, 1].Value = "EventNumber";
        //        worksheet.Cells[1, 1].Style.Font.Size = 12;
        //        worksheet.Cells[1, 1].Style.Font.Bold = true;

        //        worksheet.Cells[1, 2].Value = "EventTime";
        //        worksheet.Cells[1, 2].Style.Font.Size = 12;
        //        worksheet.Cells[1, 2].Style.Font.Bold = true;

        //        worksheet.Cells[1, 3].Value = "MilliTime";
        //        worksheet.Cells[1, 3].Style.Font.Size = 12;
        //        worksheet.Cells[1, 3].Style.Font.Bold = true;

        //        worksheet.Cells[1, 4].Value = "EventValue";
        //        worksheet.Cells[1, 4].Style.Font.Size = 12;
        //        worksheet.Cells[1, 4].Style.Font.Bold = true;

        //        worksheet.Cells[1, 5].Value = "EventReason";
        //        worksheet.Cells[1, 5].Style.Font.Size = 12;
        //        worksheet.Cells[1, 5].Style.Font.Bold = true;

        //        worksheet.Cells[1, 6].Value = "EventType";
        //        worksheet.Cells[1, 6].Style.Font.Size = 12;
        //        worksheet.Cells[1, 6].Style.Font.Bold = true;

        //        worksheet.Cells[1, 7].Value = "CityName";
        //        worksheet.Cells[1, 7].Style.Font.Size = 12;
        //        worksheet.Cells[1, 7].Style.Font.Bold = true;

        //        worksheet.Cells[1, 8].Value = "PostName";
        //        worksheet.Cells[1, 8].Style.Font.Size = 12;
        //        worksheet.Cells[1, 8].Style.Font.Bold = true;

        //        worksheet.Cells[1, 9].Value = "FeederName";
        //        worksheet.Cells[1, 9].Style.Font.Size = 12;
        //        worksheet.Cells[1, 9].Style.Font.Bold = true;

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 1].Value = ExcelEvent[i].EventNumber;
        //            worksheet.Cells[i + 2, 1].Style.Font.Size = 12;
        //        }

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 2].Value = ExcelEvent[i].EventTime/*.ToString("yyyy-MM-dd HH:mm:ss")*/;
        //            worksheet.Cells[i + 2, 2].Style.Font.Size = 12;
        //        }

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 3].Value = Convert.ToInt32(ExcelEvent[i].MilliTime);
        //            worksheet.Cells[i + 2, 3].Style.Font.Size = 12;
        //        }

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 4].Value = Convert.ToInt32(ExcelEvent[i].EventValue);
        //            worksheet.Cells[i + 2, 4].Style.Font.Size = 12;
        //        }

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 5].Value = ExcelEvent[i].EventReason;
        //            worksheet.Cells[i + 2, 5].Style.Font.Size = 12;
        //        }

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 6].Value = ExcelEvent[i].EventType;
        //            worksheet.Cells[i + 2, 6].Style.Font.Size = 12;
        //        }

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 7].Value = ExcelEvent[i].CityName;
        //            worksheet.Cells[i + 2, 7].Style.Font.Size = 12;
        //        }

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 8].Value = ExcelEvent[i].PostName;
        //            worksheet.Cells[i + 2, 8].Style.Font.Size = 12;
        //        }

        //        for (int i = 0; i < ExcelEvent.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 9].Value = Convert.ToInt32(ExcelEvent[i].FeederName);
        //            worksheet.Cells[i + 2, 9].Style.Font.Size = 12;
        //        }

        //        fliecontents = package.GetAsByteArray();
        //    }

        //    string excelName = $"Event-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

        //    if (fliecontents == null || fliecontents.Length == 0)
        //    {
        //        return NotFound();
        //    }
        //    return File(
        //        fileContents: fliecontents,
        //        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //        fileDownloadName: excelName
        //        );

        //}

    }
}