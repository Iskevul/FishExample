using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
//using Core;

namespace FishHandle
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> fishes = new Dictionary<string, string>();

            Console.WriteLine("Content-Type: text/html \n\n");
            var queryStr = Environment.GetEnvironmentVariable("QUERY_STRING");

            //queryStr = "name=mentai&interval=10&tempData=-1 -2 -3 -6&maxTemp=-4&maxTime=10";
            queryStr = WebUtility.UrlDecode(queryStr);
            string[] queryArr = queryStr.Split('&');

            foreach(var box in queryArr)
            {
                var element = box.Split('=');
                fishes[$"{element[0]}"] = Convert.ToString(element[1]);
            }

            foreach(var fish in fishes.Keys)
            {
                Console.WriteLine($"<p>{fish} = {fishes[fish]}</p>");
            }
            Console.WriteLine("<hr>");


            ////
            ///

            string name = fishes["name"];
            int interval = Convert.ToInt32(fishes["interval"]);
            string tempData = fishes["tempData"];
            //fish data
            int maxTemp = Convert.ToInt32(fishes["maxTemp"]);
            int maxTempTime = Convert.ToInt32(fishes["maxTime"]); // min


            var quality = new TempQuality(interval, tempData);
            Fish mentai = new FrozenFish(name, quality, (double)maxTemp, new TimeSpan(0, maxTempTime, 0));
            Console.WriteLine($"<p>Fish is valid - {mentai.isValid()}</p>");

            Console.WriteLine("<h2>Report:</h2>");
            foreach (KeyValuePair<DateTime, double> q in quality)
            {
                Console.WriteLine($"<p>{q.Key} : {q.Value}</p>");
            }
            Console.WriteLine("<hr>");
            Console.WriteLine("<hr>");
        }
    }
}
