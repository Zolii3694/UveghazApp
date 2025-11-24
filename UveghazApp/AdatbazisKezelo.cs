using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace UveghazApp
{
    internal class AdatbazisKezelo
    {
        private string connectionString = "Data Source=szenzorok.db;Version=3;";

        public AdatbazisKezelo()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTable =
                @"CREATE TABLE IF NOT EXISTS Meresek(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Time TEXT,
                    Homerseklet REAL,
                    Paratartalom REAL,
                    Talaj REAL
                );";

                using (var cmd = new SQLiteCommand(createTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Ment(MeresBlokk m)
        {
            using(var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert =
                           "INSERT INTO Meresek (Time, Homerseklet, Paratartalom, Talaj) " +
                           "VALUES (@time, @hom, @par, @talaj);";

                using (var cmd = new SQLiteCommand(insert, connection))
                {
                    cmd.Parameters.AddWithValue("@time", m.Time.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@hom", m.Homerseklet);
                    cmd.Parameters.AddWithValue("@par", m.Paratartalom);
                    cmd.Parameters.AddWithValue("@talaj", m.Talaj);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}