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
using OfficeOpenXml;

namespace BehineNiroo
{
    public class ShowEnergyTableModel : PageModel
    {

        public ShowEnergyTableModel(BehineNirooContext _context)
        {
            context = _context;
        }

        public static string query = "";

        public static List<Energy> ExcelEnergies = new List<Energy>();

        public List<Energy> energies { get; set; } = new List<Energy>();

        [BindProperty]
        public string[] Energysplit { get; set; }

        string cs = Globals.EnergyConnection;
        private readonly BehineNirooContext context;

        public void OnGet(string fdate, string tdate, string feeder)
        {
            //string query = "";
            Energysplit = feeder.Split(",");
            string[] MoreSplit;

            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                connection.Open();
                foreach (var item in Energysplit)
                {
                    MoreSplit = item.Split("_");
                    query = "select DateAndTime,ActEImp,T1Imp,T2Imp,T3Imp,T4Imp,DemandImp,ActEExp,T1Exp,T2Exp,T3Exp" +
                    ", T4Exp, DemandExp, ReactEImp, pd.CityName,pd.PostName,pd.FeederName " +
                    "from energy.dailyenergy ed inner join pocketswitch.deviceinfo pd on ed.SerialNum = pd.SerialNumber " +
                    "where DateAndTime > '" + fdate + "' and DateAndTime < '" + tdate + "' and " +
                    "CityName='" + MoreSplit[0] + "' and PostName='" + MoreSplit[1] + "' and  FeederName='" + MoreSplit[2] + "'" +
                    " order by DateAndTime desc";

                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                    {
                        MySqlDataReader reader = mySqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            energies.Add(new Energy()
                            {
                                EnergyTime = Convert.ToDateTime(reader["DateAndTime"]).ToPersianDateTime().ToString(),
                                //EnergyIntensity = int.Parse(reader["EnergyIntensity"].ToString()),
                                ActElmpWh = int.Parse(reader["ActEImp"].ToString()),
                                T1ImpWh = int.Parse(reader["T1Imp"].ToString()),
                                T2ImpWh = int.Parse(reader["T2Imp"].ToString()),
                                T3ImpWh = int.Parse(reader["T3Imp"].ToString()),
                                T4ImpWh = int.Parse(reader["T4Imp"].ToString()),
                                DemandImpW = int.Parse(reader["DemandImp"].ToString()),
                                ActEExpWh = int.Parse(reader["ActEExp"].ToString()),
                                T1ExpWh = int.Parse(reader["T1Exp"].ToString()),
                                T2ExpWh = int.Parse(reader["T2Exp"].ToString()),
                                T3ExpWh = int.Parse(reader["T3Exp"].ToString()),
                                T4ExpWh = int.Parse(reader["T4Exp"].ToString()),
                                DemandExpw = int.Parse(reader["DemandExp"].ToString()),
                                ReactEImpVArh = int.Parse(reader["ReactEImp"].ToString()),
                                CityName = Convert.ToString(reader["CityName"]),
                                PostName = Convert.ToString(reader["PostName"]),
                                FeederName = Convert.ToString(reader["FeederName"])
                            });
                        }
                        reader.Close();
                    }
                }
            }
            ExcelEnergies = energies;

            return;
        }

        public IActionResult OnPost()
        {
            byte[] fliecontents;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            int EnergyCount = ExcelEnergies.Count;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("sheet1");

                worksheet.Cells[1, 1].Value = "EnergyTime";
                worksheet.Cells[1, 1].Style.Font.Size = 12;
                worksheet.Cells[1, 1].Style.Font.Bold = true;

                worksheet.Cells[1, 2].Value = "EnergyIntensity";
                worksheet.Cells[1, 2].Style.Font.Size = 12;
                worksheet.Cells[1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 3].Value = "ActElmpWh";
                worksheet.Cells[1, 3].Style.Font.Size = 12;
                worksheet.Cells[1, 3].Style.Font.Bold = true;

                worksheet.Cells[1, 4].Value = "T1ImpWh";
                worksheet.Cells[1, 4].Style.Font.Size = 12;
                worksheet.Cells[1, 4].Style.Font.Bold = true;

                worksheet.Cells[1, 5].Value = "T2ImpWh";
                worksheet.Cells[1, 5].Style.Font.Size = 12;
                worksheet.Cells[1, 5].Style.Font.Bold = true;

                worksheet.Cells[1, 6].Value = "T3ImpWh";
                worksheet.Cells[1, 6].Style.Font.Size = 12;
                worksheet.Cells[1, 6].Style.Font.Bold = true;

                worksheet.Cells[1, 7].Value = "T4ImpWh";
                worksheet.Cells[1, 7].Style.Font.Size = 12;
                worksheet.Cells[1, 7].Style.Font.Bold = true;

                worksheet.Cells[1, 8].Value = "DemandImpW";
                worksheet.Cells[1, 8].Style.Font.Size = 12;
                worksheet.Cells[1, 8].Style.Font.Bold = true;

                worksheet.Cells[1, 9].Value = "ActEExpWh";
                worksheet.Cells[1, 9].Style.Font.Size = 12;
                worksheet.Cells[1, 9].Style.Font.Bold = true;

                worksheet.Cells[1, 10].Value = "T1ExpWh";
                worksheet.Cells[1, 10].Style.Font.Size = 12;
                worksheet.Cells[1, 10].Style.Font.Bold = true;

                worksheet.Cells[1, 11].Value = "T2ExpWh";
                worksheet.Cells[1, 11].Style.Font.Size = 12;
                worksheet.Cells[1, 11].Style.Font.Bold = true;

                worksheet.Cells[1, 12].Value = "T3ExpWh";
                worksheet.Cells[1, 12].Style.Font.Size = 12;
                worksheet.Cells[1, 12].Style.Font.Bold = true;

                worksheet.Cells[1, 13].Value = "T4ExpWh";
                worksheet.Cells[1, 13].Style.Font.Size = 12;
                worksheet.Cells[1, 13].Style.Font.Bold = true;

                worksheet.Cells[1, 14].Value = "DemandExpw";
                worksheet.Cells[1, 14].Style.Font.Size = 12;
                worksheet.Cells[1, 14].Style.Font.Bold = true;

                worksheet.Cells[1, 15].Value = "ReactEImpVArh";
                worksheet.Cells[1, 15].Style.Font.Size = 12;
                worksheet.Cells[1, 15].Style.Font.Bold = true;

                worksheet.Cells[1, 16].Value = "CityName";
                worksheet.Cells[1, 16].Style.Font.Size = 12;
                worksheet.Cells[1, 16].Style.Font.Bold = true;

                worksheet.Cells[1, 17].Value = "PostName";
                worksheet.Cells[1, 17].Style.Font.Size = 12;
                worksheet.Cells[1, 17].Style.Font.Bold = true;

                worksheet.Cells[1, 18].Value = "FeederName";
                worksheet.Cells[1, 18].Style.Font.Size = 12;
                worksheet.Cells[1, 18].Style.Font.Bold = true;

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = ExcelEnergies[i].EnergyTime.ToString();
                    worksheet.Cells[i + 2, 1].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    //worksheet.Cells[i + 2, 2].Value = ExcelEnergies[i].EnergyIntensity;
                    worksheet.Cells[i + 2, 2].Value = "-";
                    worksheet.Cells[i + 2, 2].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 3].Value = Convert.ToInt32(ExcelEnergies[i].ActElmpWh);
                    worksheet.Cells[i + 2, 3].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 4].Value = Convert.ToInt32(ExcelEnergies[i].T1ImpWh);
                    worksheet.Cells[i + 2, 4].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 5].Value = ExcelEnergies[i].T2ImpWh;
                    worksheet.Cells[i + 2, 5].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 6].Value = ExcelEnergies[i].T3ImpWh;
                    worksheet.Cells[i + 2, 6].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 7].Value = ExcelEnergies[i].T4ImpWh;
                    worksheet.Cells[i + 2, 7].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 8].Value = ExcelEnergies[i].DemandImpW;
                    worksheet.Cells[i + 2, 8].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 9].Value = ExcelEnergies[i].ActEExpWh;
                    worksheet.Cells[i + 2, 9].Style.Font.Size = 12;
                }
                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 10].Value = ExcelEnergies[i].T1ExpWh;
                    worksheet.Cells[i + 2, 10].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 11].Value = ExcelEnergies[i].T2ExpWh;
                    worksheet.Cells[i + 2, 11].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 12].Value = ExcelEnergies[i].T3ExpWh;
                    worksheet.Cells[i + 2, 12].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 13].Value = ExcelEnergies[i].T4ExpWh;
                    worksheet.Cells[i + 2, 13].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 14].Value = ExcelEnergies[i].DemandExpw;
                    worksheet.Cells[i + 2, 14].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 15].Value = ExcelEnergies[i].ReactEImpVArh;
                    worksheet.Cells[i + 2, 15].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 16].Value = ExcelEnergies[i].CityName;
                    worksheet.Cells[i + 2, 16].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 17].Value = ExcelEnergies[i].PostName;
                    worksheet.Cells[i + 2, 17].Style.Font.Size = 12;
                }

                for (int i = 0; i < EnergyCount; i++)
                {
                    worksheet.Cells[i + 2, 18].Value = Convert.ToInt32(ExcelEnergies[i].FeederName);
                    worksheet.Cells[i + 2, 18].Style.Font.Size = 12;
                }

                fliecontents = package.GetAsByteArray();
            }

            string excelName = $"Energy-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            if (fliecontents == null || fliecontents.Length == 0)
            {
                return NotFound();
            }
            return File(
                fileContents: fliecontents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: excelName
                );

        }
    }
}