using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Data;
using BehineNiroo.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace BehineNiroo
{
    public class ShowLogsModel : PageModel
    {

        private readonly BehineNirooContext context;

        public ShowLogsModel(BehineNirooContext _context)
        {
            context = _context;
        }


        [BindProperty]
        public int Len { get; set; }

        [BindProperty]
        public int dataLen { get; set; }

        public string query = "";

        [BindProperty]
        public List<string> myDate { get; set; } = new List<string>();

        [BindProperty]
        public string[] myDateArray { get; set; }




        [BindProperty]
        public string[] chartData1Array { get; set; } 

        [BindProperty]
        public string[] chartData2Array { get; set; } = null;

        [BindProperty]
        public string[] chartData3Array { get; set; } = null;
        [BindProperty]
        public string[] chartData4Array { get; set; } = null;

        [BindProperty]
        public string[] chartData5Array { get; set; } = null;
        


        [BindProperty]
        public static List<string>[] myData { get; set; }

        public List<string[]> myDataArray { get; set; } = new List<string[]>();
        //[BindProperty]
        //public string[] myDataArray { get; set; }

        [BindProperty]
        public string[] categorysplit { get; set; }

        //public static string[] temp;

        //public int SelectedIndex;

        //[BindProperty]
        //public string[] SelectedCategory { get; set; }

        //[BindProperty]
        //public string[] SSelectedCategory { get; set; }

        //[BindProperty]
        //public string[] SecondSelectedCategory { get; set; }

        //public string[] UnSelectedCategories { get; set; }



        string cs = Globals.PocketSwitchConnection;

        public void OnGet(string fdate, string tdate, string categories)
        {

            categorysplit = categories.Split(",");
            //temp = categorysplit;
            Len = categorysplit.Length;

            myData = new List<string>[Len];

            string[] MoreSplit ;

            int i = 0;

            foreach (var item in categorysplit)
            {
                MoreSplit = item.Split("_");
                using (MySqlConnection connection = new MySqlConnection(cs))
                {
                    query = "select " + MoreSplit[0] +
                        ",LogTime from logger." +
                        MoreSplit[3] + " where LogTime > '" +
                        fdate + "' and LogTime < '" + tdate + "' order by LogTime asc";
                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
                    {
                        DataTable dataTable = new DataTable();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(mySqlCommand))
                        {
                            da.Fill(dataTable);

                            dataLen = dataTable.Rows.Count;

                            myData[i] = new List<string>();

                            for (int j = 0; j < dataTable.Rows.Count; j++)
                            {
                                myData[i].Add(Convert.ToString(dataTable.Rows[j][0]));
                            }

                            //myDataArray.Add(myData[i].ToArray());

                            if (i == Len - 1)
                            {
                                for (int k = 0; k < dataTable.Rows.Count; k++)
                                {
                                    myDate.Add(Convert.ToString(dataTable.Rows[k][1]));
                                }
                            }
                        }
                    }
                }
                i++;
            }

            switch (Len)
            {
                case 1:
                    chartData1Array = myData[0].ToArray();
                    chartData2Array = new string[1] { ""};
                    chartData3Array = new string[1] { "" };
                    chartData4Array = new string[1] { "" };
                    chartData5Array = new string[1] { "" };
                    break;

                case 2:
                    chartData1Array = myData[0].ToArray();
                    chartData2Array = myData[1].ToArray();
                    chartData3Array = new string[1] { "" };
                    chartData4Array = new string[1] { "" };
                    chartData5Array = new string[1] { "" };
                    break;

                case 3:
                    chartData1Array = myData[0].ToArray();
                    chartData2Array = myData[1].ToArray();
                    chartData3Array = myData[2].ToArray();
                    chartData4Array = new string[1] { "" };
                    chartData5Array = new string[1] { "" };
                    break;

                case 4:
                    chartData1Array = myData[0].ToArray();
                    chartData2Array = myData[1].ToArray();
                    chartData3Array = myData[2].ToArray();
                    chartData4Array = myData[3].ToArray();
                    chartData5Array = new string[1] { "" };
                    break;

                case 5:
                    chartData1Array = myData[0].ToArray();
                    chartData2Array = myData[1].ToArray();
                    chartData3Array = myData[2].ToArray();
                    chartData4Array = myData[3].ToArray();
                    chartData5Array = myData[4].ToArray();
                    break;

                default:
                    break;
            }

            //using (MySqlConnection connection = new MySqlConnection(cs))
            //{
            //    using (MySqlCommand mySqlCommand = new MySqlCommand(query, connection))
            //    {
            //        DataTable dataTable = new DataTable();

            //        using (MySqlDataAdapter da = new MySqlDataAdapter(mySqlCommand))
            //        {
            //            da.Fill(dataTable);

            //            myData = new List<string>[Len];

            //            for (int i = 0; i < Len; i++)
            //            {
            //                myData[i] = new List<string>();
            //                for (int j = 0; j < dataTable.Rows.Count; j++)
            //                {
            //                    myData[i].Add(Convert.ToString(dataTable.Rows[j][i]));
            //                }
            //            }

            //            for (int i = 0; i < dataTable.Rows.Count; i++)
            //            {
            //                myDate.Add(Convert.ToString(dataTable.Rows[i][Len]));
            //            }
            //            SelectedCategory = myData[0].ToArray();
            //            SSelectedCategory = myData[1].ToArray();
            //            myDateArray = myDate.ToArray();
            //        }
            //    }
            //}

        }




        //public IActionResult OnGetListToArray(string fdate, string tdate, string categories)
        //{

        //    JsonResult json = new JsonResult(myData.ToArray());

        //    return json;
        //}



        //public IActionResult OnGetSelectedCategory(string cat)
        //{
        //    Global.SelectedIndex = Array.IndexOf(temp, cat);

        //    SelectedCategory = myData[Global.SelectedIndex].ToArray();

        //    JsonResult json = new JsonResult(SelectedCategory);

        //    return json;
        //}

        //public IActionResult OnGetUnSelectedCategory(string cat)
        //{
        //    int index = Array.IndexOf(temp, cat);

        //    UnSelectedCategories = temp.Where(w => w != temp[index]).ToArray();

        //    JsonResult json = new JsonResult(UnSelectedCategories);

        //    return json;
        //}

        //public IActionResult OnGetSecondSelectedCategory(string second_cat)
        //{
        //    int index = Array.IndexOf(temp, second_cat);

        //    SecondSelectedCategory = myData[index].ToArray();

        //    SSelectedCategory = myData[Global.SelectedIndex].ToArray();

        //    ArrayList tmp = new ArrayList();

        //    tmp.Add(SSelectedCategory);
        //    tmp.Add(SecondSelectedCategory);

        //    JsonResult json = new JsonResult(tmp.ToArray());

        //    return json;
        //}

    }
}