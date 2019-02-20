using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace FormuleSystem.ORM.DAO.Sqls
{
    public class EvidenceVyrobcuMotoru
    {
        public static String TABLE_NAME = "Vyrobce_motoru";

        public static String SQL_SELECT = "SELECT * FROM Vyrobce_motoru";
        public static String SQL_SELECT_MAX_ID = "SELECT MAX(ID) AS Maximum FROM Vyrobce_motoru";
        public static String SQL_INSERT = "INSERT INTO Vyrobce_motoru VALUES (@id, @nazev)";
        public static String SQL_UPDATE = "UPDATE Vyrobce_motoru SET Nazev = @nazev WHERE ID = @id";
        public static String SQL_DELETE = "DELETE FROM Vyrobce_motoru WHERE ID = @id";


        // function 6.1
        public static int VlozeniVyrobceMotoru(Vyrobce_motoru Vyrobce_motoru, Database pDb = null)
        {
            Database db;
            if (pDb == null)
            {
                db = new Database();
                db.Connect();
                db.BeginTransaction();
            }
            else
            {
                db = (Database)pDb;
            }

            SqlCommand command_count = db.CreateCommand(SQL_SELECT_MAX_ID);
            SqlDataReader reader = db.Select(command_count);

            int id_next = 0;

            while (reader.Read())
            {
                int i = -1;
                id_next = reader.GetInt32(++i);
            }
            id_next++;
            reader.Close();

            SqlCommand command = db.CreateCommand(SQL_INSERT);
            command.Parameters.AddWithValue("@id", id_next);
            command.Parameters.AddWithValue("@nazev", Vyrobce_motoru.Nazev);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.EndTransaction();
                db.Close();
            }

            return ret;
        }
        
        // funkce 6.2
        public static int UpravaVyrobceMotoru(Vyrobce_motoru Vyrobce_motoru, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_UPDATE);
            command.Parameters.AddWithValue("@id", Vyrobce_motoru.ID);
            command.Parameters.AddWithValue("@nazev", Vyrobce_motoru.Nazev);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 6.3
        public static int SmazaniVyrobceMotoru(int id, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_DELETE);
            command.Parameters.AddWithValue("@id", id);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 6.4
        public static Collection<Vyrobce_motoru> VypisVyrobcuMotoru(Database pDb = null)
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

            Collection<Vyrobce_motoru> Vyrobce_motoru = CteniVyrobcuMotoru(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Vyrobce_motoru;
        }

        private static Collection<Vyrobce_motoru> CteniVyrobcuMotoru(SqlDataReader reader)
        {
            Collection<Vyrobce_motoru> Vyrobce_motoru = new Collection<Vyrobce_motoru>();

            while (reader.Read())
            {
                int i = -1;
                Vyrobce_motoru Vyrobce = new Vyrobce_motoru();
                Vyrobce.ID = reader.GetInt32(++i);
                Vyrobce.Nazev = reader.GetString(++i);

                Vyrobce_motoru.Add(Vyrobce);
            }
            return Vyrobce_motoru;
        }
    }
}
