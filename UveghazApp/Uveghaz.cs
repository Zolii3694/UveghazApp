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
   
        private List<MeresBlokk> meresek = new List<MeresBlokk>();
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
            if (ertek > 70)
            {
                Console.WriteLine("Magas talajnedvesseg -> Ontozes kikapcsolva!");
            }
            else if (ertek < 35)
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
        public void HomersekletOlvas() => homerseklet.ErtekOlvasas();
        public void ParatartalomOlvas() => paratartalom.ErtekOlvasas();
        public void TalajOlvas() => talaj.ErtekOlvasas();
        public MeresBlokk MeresiCiklus()
        {
            var h = homerseklet.ErtekOlvasasCsendben();
            var p = paratartalom.ErtekOlvasasCsendben();
            var t = talaj.ErtekOlvasasCsendben();

            var blokk = new MeresBlokk
            {
                Time = DateTime.Now,
                Homerseklet = h,
                Paratartalom = p,
                Talaj = t
            };

            meresek.Add(blokk);
            db.Ment(blokk);

            return blokk;
        }
        public List<MeresBlokk> GetMeresek()
        {
            return meresek;
        }

    }

}
