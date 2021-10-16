using System;
using System.IO;

namespace HtmlForm
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"/home/iskan/form.html";
            //string path = @"C:\Users\User\source\repos\FishExample\FishForm\form.html";
            string html;

            Console.WriteLine("Content-Type: text/html \n\n");
            try
            {
                using (StreamReader s = new StreamReader(path))
                {
                    html = s.ReadToEnd();
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
