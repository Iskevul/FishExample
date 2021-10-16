using System;
using System.Collections.Generic;

namespace FishMonitoringConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Content-Type: text/html \n\n");
            // temperature data
            string name = "mentai";
            int interval = 10; //min
            string tempData = "-1, -2, -3, -6";
            //fish data
            int maxTemp = -4;
            int maxTempTime = 10; // min


            var quality = new TempQuality(interval, tempData);
            Fish mentai = new FrozenFish(name, quality, (double)maxTemp, new TimeSpan(0, maxTempTime, 0));
            Console.WriteLine(mentai.isValid());

            foreach (KeyValuePair<DateTime, double> q in quality)
            {
                Console.WriteLine(q.Key + " : " + q.Value);
            }
        }
    }




}