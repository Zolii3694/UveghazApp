using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UveghazSzenzorok;

namespace UveghazApp
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            // 3 szenzor példányosítása
            var homero = new HomersekletSzenzor();
            var para = new ParatartalomSzenzor();
            var talaj = new TalajNedvesseg();

            // ESEMÉNYEKRE feliratkozás (objektumszinten)
            homero.ErtekValtozas += (s, ertek) =>
                Console.WriteLine($"Hőmérséklet: {ertek:F2} °C");

            para.ErtekValtozas += (s, ertek) =>
                Console.WriteLine($"Páratartalom: {ertek:F2} %");

            talaj.ErtekValtozas += (s, ertek) =>
                Console.WriteLine($"Talajnedvesség: {ertek:F2} %");

            // TESZT: 10 mérési ciklus objektumszinten
            for (int i = 0; i < 10; i++)
            {
                homero.ErtekOlvasas();   // generál + event
                para.ErtekOlvasas();     // generál + event
                talaj.ErtekOlvasas();    // generál + event

                System.Threading.Thread.Sleep(500);
            }

            Console.WriteLine("\nTeszt lefutott.");

            Console.ReadKey(true);
        }
    }
}
