using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using MySql.Data;
using MySql.Data.MySqlClient;

//using Core;

namespace FishHandle
{
    class Program
    {
        public static string name;
        public static int interval;
        public static string tempData;
        public static int maxTemp;
        public static int maxTime;
        static void Main(string[] args)
        {
            string connStr = "server=192.168.43.189;user=Iskan;database=Fish;port=3306;password=1234";
            MySqlConnection conn = new MySqlConnection(connStr);

            var form = new HtmlFormData();
            form.Handle();
            //var queryStr = Environment.GetEnvironmentVariable("QUERY_STRING");

            Console.WriteLine("Content-Type: text/html \n\n");            

            //queryStr = "name=mentai&interval=10&tempData=-1 -2 -3 -6&maxTemp=-4&maxTime=10";
            //queryStr = "select=mentai";
            //queryStr = WebUtility.UrlDecode(queryStr);

            //Dictionary<string, string> fishes = getDict(queryStr);

            string fishName = form["select"];

            

            try
            {
                conn.Open();
                string sql = $"SELECT * FROM fish WHERE name = \"{fishName}\"";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader res = cmd.ExecuteReader();

                while (res.Read())
                {
                    name     = Convert.ToString(res[1]);
                    interval = Convert.ToInt32(res[2]);
                    tempData = Convert.ToString(res[3]);
                    maxTemp  = Convert.ToInt32(res[4]);
                    maxTime  = Convert.ToInt32(res[5]);
                }
                res.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            conn.Close();


            ////
            ///

            //string name = fishes["name"];
            //int interval = Convert.ToInt32(fishes["interval"]);
            //string tempData = fishes["tempData"];
            ////fish data
            //int maxTemp = Convert.ToInt32(fishes["maxTemp"]);
            //int maxTime = Convert.ToInt32(fishes["maxTime"]); // min



            PrintReport();
            
        }

        public static Dictionary<string, string> getDict(string queryStr)
        {
            Dictionary<string, string> book = new Dictionary<string, string>();
            string[] queryArr = queryStr.Split('&');
            foreach (var box in queryArr)
            {
                var element = box.Split('=');
                book[$"{element[0]}"] = Convert.ToString(element[1]);
            }
            return book;
        }

        public static void PrintReport()
        {
            Console.WriteLine("<p><pre>name:     " + name + "</pre></p>");
            Console.WriteLine("<p><pre>interval: " + interval + "</pre></p>");
            Console.WriteLine("<p><pre>tempData: " + tempData + "</pre></p>");
            Console.WriteLine("<p><pre>maxTemp:  " + maxTemp + "</pre></p>");
            Console.WriteLine("<p><pre>maxTime:  " + maxTime + "</pre></p>");

            Console.WriteLine("<hr>");

            var quality = new TempQuality(interval, tempData);
            Fish fish = new FrozenFish(name, quality, (double)maxTemp, new TimeSpan(0, maxTime, 0));
            Console.WriteLine($"<p>Fish is valid - {fish.isValid()}</p>");

            Console.WriteLine("<h2>Report:</h2>");
            foreach (KeyValuePair<DateTime, double> q in quality)
            {
                Console.WriteLine($"<p>{q.Key} : {q.Value}</p>");
            }
            Console.WriteLine("<hr>");
            Console.WriteLine("<hr>");
        }
    }


    public class HtmlFormData : IEnumerable
    {
        private string rawFormData;
        private string requestMethod;
        private Dictionary<string, string> formData;

        public int Count
        {
            get => formData.Count;
        }

        public string this[string key]
        {
            get => formData[key];
        }

        public HtmlFormData()
        {
            formData = new Dictionary<string, string>();
        }

        public void HandlePost()
        {
            GetMethodPostData();
            ParseRawFormData();
        }

        public void HandleGet()
        {
            GetMethodGetData();
            ParseRawFormData();
        }

        public void Handle(string data)
        {
            rawFormData = data;
            ParseRawFormData();
        }

        public void Handle()
        {
            requestMethod = Environment.GetEnvironmentVariable("REQUEST_METHOD");

            switch (requestMethod)
            {
                case "GET":
                    HandleGet();
                    break;

                case "POST":
                    HandlePost();
                    break;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return formData.GetEnumerator();
        }

        private void GetMethodPostData()
        {
            // get post data length
            var dataLen = int.Parse(Environment.GetEnvironmentVariable("CONTENT_LENGTH"));

            //get post data
            var data = new char[dataLen + 1];
            for (int i = 0; i < dataLen; ++i)
            {
                data[i] = (char)Console.Read();
            }

            rawFormData = new String(data);
        }

        private void GetMethodGetData()
        {
            rawFormData = Environment.GetEnvironmentVariable("QUERY_STRING");
        }

        private void ParseRawFormData()
        {
            var fields = new String(rawFormData).Split("&");
            foreach (var field in fields)
            {
                var fieldData = field.Split("=");
                formData.Add(fieldData[0], fieldData[1]);
            }
        }
    }
}
