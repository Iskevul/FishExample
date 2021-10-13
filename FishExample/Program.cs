using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace FishMonitoringConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string name;
            int interval;
            string tempData;
            int maxTemp;
            int maxTime;

            string connStr = "server=192.168.43.189;user=Iskan;database=Fish;port=3306;password=1234";

            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM fish";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader res = cmd.ExecuteReader();

                Console.WriteLine("Content-Type: text/html \n\n");

                while (res.Read())
                {
                    name = Convert.ToString(res[1]);
                    interval = Convert.ToInt32(res[2]); //min
                    tempData = Convert.ToString(res[3]);
                    //fish data
                    maxTemp = Convert.ToInt32(res[4]);
                    maxTime = Convert.ToInt32(res[5]); // min

                    var quality = new TempQuality(interval, tempData);
                    Fish mentai = new FrozenFish(name, quality, (double)maxTemp, new TimeSpan(0, maxTime, 0));
                    Console.WriteLine(mentai.isValid());

                    foreach (KeyValuePair<DateTime, double> q in quality)
                    {
                        Console.WriteLine(q.Key + " : " + q.Value);
                    }
                }

                res.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");


            // temperature data
            //string name = "mentai";
            //int interval = 10; //min
            //string tempData = "-1,-2,-3,-6";
            ////fish data
            //int maxTemp = -4;
            //int maxTime = 10; // min

            
        }
    }




}