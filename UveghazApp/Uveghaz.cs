using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UveghazSzenzorok;
using System.IO;
using Newtonsoft.Json;

namespace UveghazApp
{
    internal class Uveghaz
    {
        public class Meresek
        {
            public string Nev { get; set; }
            public double Ertek { get; set; }
            public DateTime Time { get; set; }

        }
        private List<Meresek> meresek = new List<Meresek>();
        private HomersekletSzenzor homerseklet;
        private ParatartalomSzenzor paratartalom;
        private TalajNedvesseg talaj;
        private AdatbazisKezelo db = new AdatbazisKezelo();

        public Uveghaz()
        {
            homerseklet = new HomersekletSzenzor();
            paratartalom = new ParatartalomSzenzor();
            talaj= new TalajNedvesseg();
            homerseklet.ErtekValtozas += KezeldHomersekletet;
            paratartalom.ErtekValtozas += KezeldParatartalmat;
            talaj.ErtekValtozas += KezeldTalajnedvesseget;
            
        }

        private void KezeldHomersekletet(object sender, double ertek)
        {
            var ujMeres = new Meresek
            {
                Nev = "Hőmérséklet",
                Ertek = ertek,
                Time = DateTime.Now
            };
            meresek.Add(ujMeres);
            db.Ment(ujMeres);

            if (ertek < 18)
            {
                Console.WriteLine("Hideg van -> Futes bekapcsolva!");
            }
            else if (ertek > 30)
            {
                Console.WriteLine("Meleg van -> Klima bekapcsolva!");
            }

        }
        private void KezeldParatartalmat(object sender, double ertek)
        {
            var ujMeres = new Meresek
            {
                Nev = "Páratartalom",
                Ertek = ertek,
                Time = DateTime.Now
            };

            meresek.Add(ujMeres);
            db.Ment(ujMeres);

            if (ertek > 75)
            {
                Console.WriteLine("Magas paratartalom -> Szelloztetes bekapcsolva!");
            }
            else if (ertek < 45)
            {
                Console.WriteLine("Alacsony paratartalom -> Parasito bekapcsolva!");
            }
        }
        private void KezeldTalajnedvesseget(object sender, double ertek)
        {
            var ujMeres = new Meresek
            {
                Nev = "Talaj nedvesség",
                Ertek = ertek,
                Time = DateTime.Now
            };

            meresek.Add(ujMeres);
            db.Ment(ujMeres);

            if (ertek > 100)
            {
                Console.WriteLine("Magas talajnedvesseg -> Ontozes kikapcsolva!");
            }
            else if (ertek < 15)
            {
                Console.WriteLine("Alacsony talajnedvesseg -> Ontozes bekapcsolva!");
            }
        }
        public void MentsJson()
        {
            string json = JsonConvert.SerializeObject(meresek, Formatting.Indented);
            File.WriteAllText("meresek.json", json);
            Console.WriteLine("JSON file-ba mentes megtortent: meresek.json");
        }
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
        }
    }
}
