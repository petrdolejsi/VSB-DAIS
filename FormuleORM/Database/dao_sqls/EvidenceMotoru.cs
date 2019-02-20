using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace FormuleSystem.ORM.DAO.Sqls
{
    public class EvidenceMotoru
    {
        public static String TABLE_NAME = "Motory";

        public static String SQL_SELECT = "SELECT * FROM Motory";
        public static String SQL_SELECT_NAMONTOVANO = "SELECT Seriove_cislo, Pocet_pouziti FROM Motory JOIN Jezdci ON Motory.Seriove_cislo=Jezdci.Motory_Seriove_cislo";
        public static String SQL_INSERT = "INSERT INTO Motory VALUES (@cislo, 0)";
        public static String SQL_UPDATE = "UPDATE Motory SET Seriove_cislo=@cislo, Pocet_pouziti=@pocet WHERE Seriove_cislo = @cislo";
        public static String SQL_DELETE_CISLO = "DELETE FROM Motory WHERE Seriove_cislo=@cislo";

        // funkce 8.1
        public static int VlozeniMotoru(int cislo, Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)pDb;
            }

            SqlCommand command = db.CreateCommand(SQL_INSERT);
            command.Parameters.AddWithValue("@cislo", cislo);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }
            return ret;
        }

        // funkce 8.2
        public static int SmazaniMotoru(int cislo, Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)pDb;
            }

            SqlCommand command = db.CreateCommand(SQL_DELETE_CISLO);
            command.Parameters.AddWithValue("@cislo", cislo);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 8.3
        public static Collection<Motory> VypisMotoru(Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)pDb;
            }

            SqlCommand command = db.CreateCommand(SQL_SELECT);
            SqlDataReader reader = db.Select(command);

            Collection<Motory> Motory = Cteni(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Motory;
        }

        // funkce 8.4
        public static Collection<Motory> VypisNamontovanychMotoru(Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
            }
            else
            {
                db = (Database)pDb;
            }

            SqlCommand command = db.CreateCommand(SQL_SELECT_NAMONTOVANO);
            SqlDataReader reader = db.Select(command);

            Collection<Motory> Motory = Cteni(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Motory;
        }

        private static Collection<Motory> Cteni(SqlDataReader reader)
        {
            Collection<Motory> Motory = new Collection<Motory>();

            while (reader.Read())
            {
                int i = -1;
                Motory Motor = new Motory();
                Motor.Seriove_cislo = reader.GetInt32(++i);
                Motor.Pocet_pouziti = reader.GetInt32(++i);

                Motory.Add(Motor);
            }
            return Motory;
        }
    }
}
