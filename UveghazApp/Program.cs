using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UveghazSzenzorok;
using static UveghazSzenzorok.TalajNedvesseg;

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
            Uveghaz uveghaz = new Uveghaz();
            // TESZT: 10 mérési ciklus objektumszinten
            Console.WriteLine("Teszteles:\n");
            for (int i = 0; i < 10; i++)
            {
                homero.ErtekOlvasas();   // generál + event
                uveghaz.HomersekletOlvas();
                para.ErtekOlvasas();     // generál + event
                uveghaz.ParatartalomOlvas();
                talaj.ErtekOlvasas();    // generál + event
                uveghaz.TalajOlvas();
                Console.WriteLine();

                System.Threading.Thread.Sleep(100);
            }
            // JSON mentés
            uveghaz.MentsJson();

            Console.WriteLine("\nTeszt lefutott.\n");
            var blokk = uveghaz.MeresiCiklus();
            Console.WriteLine("\nDB mentes OK.");

            Console.WriteLine("\nLINQ LEKÉRDEZÉSEK:");
            var lista = uveghaz.GetMeresek();
            Linq_Lekerdezes.Lefuttat(lista);


            Console.ReadKey(true);
        }
    }
}
