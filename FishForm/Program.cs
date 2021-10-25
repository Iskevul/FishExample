using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace HtmlForm
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"/home/iskan/form.html";
            string path = ConfigurationManager.AppSettings["formPath"].ToString();
            //string path = @"C:\Users\User\source\repos\FishExample\FishForm\form.html";
            string html;

            string connStr = "server=192.168.43.189;user=Iskan;database=Fish;port=3306;password=1234";
            MySqlConnection conn = new MySqlConnection(connStr);

            string select = "";

            List<string> options = new List<string>();

            try
            {
                conn.Open();
                string sql = "SELECT name FROM fish";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader res = cmd.ExecuteReader();

                while (res.Read())
                {
                    string tmp = Convert.ToString(res[0]);
                    options.Add($"<option>{tmp}</option>");
                }

                foreach (string i in options)
                {
                    select = select + i;
                }

                res.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            conn.Close();

            Console.WriteLine("Content-Type: text/html \n\n");
            try
            {
                using (StreamReader s = new StreamReader(path))
                {
                    html = s.ReadToEnd();
                    html = html.Replace("<!--Paste here-->", select);
                    Console.WriteLine(html);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
