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
                    Nev TEXT,
                    Ertek REAL,
                    Time TEXT
                );";

                using (var cmd = new SQLiteCommand(createTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Ment(Uveghaz.Meresek meres)
        {
            using(var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insert = "INSERT INTO Meresek (Nev, Ertek, Time) VALUES(@nev, @ertek, @time);";

                using (var cmd = new SQLiteCommand(insert, connection)) 
                {
                    cmd.Parameters.AddWithValue("@nev", meres.Nev);
                    cmd.Parameters.AddWithValue("@ertek", meres.Ertek);
                    cmd.Parameters.AddWithValue("@time",meres.Time.ToString("yyyy-MM-dd HH:mm:ss"));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}