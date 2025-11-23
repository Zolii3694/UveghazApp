using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UveghazSzenzorok;

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
            meresek.Add(new Meresek
            {
                Nev = "Hőmérséklet",
                Ertek = ertek,
                Time = DateTime.Now
            });

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
            meresek.Add(new Meresek
            {
                Nev = "Páratartalom",
                Ertek = ertek,
                Time = DateTime.Now
            });

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
            meresek.Add(new Meresek
            {
                Nev = "Talaj nedvesség",
                Ertek = ertek,
                Time = DateTime.Now
            });

            if (ertek > 100)
            {
                Console.WriteLine("Magas talajnedvesseg -> Ontozes kikapcsolva!");
            }
            else if (ertek < 15)
            {
                Console.WriteLine("Alacsony talajnedvesseg -> Ontozes bekapcsolva!");
            }
        }
    }
}
