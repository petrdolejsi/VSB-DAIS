using AuctionDesktopApp.ORM;
using AuctionDesktopApp.ORM.DAO.Sqls;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace AuctionDesktopApp.ORM.DAO.Sqls
{
    public class EvidenceStatu
    {
        public static String TABLE_NAME = "Staty";

        public static String SQL_SELECT = "SELECT * FROM Staty";
        public static String SQL_SELECT_MAX_ID = "SELECT MAX(ID) AS Maximum FROM Staty";
        public static String SQL_SELECT_JEZDCI = "SELECT ID, Jmeno, Prijmeni FROM Jezdci WHERE Staty_ID = @id";
        public static String SQL_SELECT_TYMY = "SELECT ID, Nazev FROM Tymy WHERE Staty_ID = @id";
        public static String SQL_UPDATE = "UPDATE Staty SET Nazev = @nazev, Narodnost = @narodnost WHERE ID = @id";
        public static String SQL_INSERT = "INSERT INTO Staty VALUES (@id, @nazev, @narodnost)";
        public static String SQL_DELETE = "DELETE FROM Staty WHERE ID = @id";

        // funkce 5.1
        public static int VlozeniStatu(Staty Stat, Database pDb = null)
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
            command.Parameters.AddWithValue("@nazev", Stat.Nazev);
            command.Parameters.AddWithValue("@narodnost", Stat.Narodnost);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.EndTransaction();
                db.Close();
            }

            return ret;
        }

        // funkce 5.2
        public static int UpravaStatu(Staty Stat, Database pDb = null)
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
            command.Parameters.AddWithValue("@id", Stat.ID);
            command.Parameters.AddWithValue("@nazev", Stat.Nazev);
            command.Parameters.AddWithValue("@narodnost", Stat.Narodnost);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 5.3
        public static int SmazaniStatu(int id, Database pDb = null)
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

        // funkce 5.4
        public static Collection<Staty> VypisStatu(Database pDb = null)
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

            Collection<Staty> Staty = CteniStatu(reader);
            reader.Close();
            
            if (pDb == null)
            {
                db.Close();
            }

            return Staty;
        }

        // funkce 5.5
        public static Collection<Tymy> VypisTymu(int id, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_TYMY);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = db.Select(command);

            Collection<Tymy> Tymy = CteniTymu(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Tymy;
        }

        // funkce 5.6
        public static Collection<Jezdci> VypisJezdcu(int id, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_JEZDCI);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = db.Select(command);

            Collection<Jezdci> Jezdci = CteniJezdcu(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Jezdci;
        }

        private static Collection<Tymy> CteniTymu(SqlDataReader reader)
        {
            Collection<Tymy> Tymy = new Collection<Tymy>();

            while (reader.Read())
            {
                int i = -1;
                Tymy Tym = new Tymy();
                Tym.ID = reader.GetInt32(++i);
                Tym.Nazev = reader.GetString(++i);

                Tymy.Add(Tym);
            }
            return Tymy;
        }
        private static Collection<Jezdci> CteniJezdcu(SqlDataReader reader)
        {
            Collection<Jezdci> Jezdci = new Collection<Jezdci>();

            while (reader.Read())
            {
                int i = -1;
                Jezdci Jezdec = new Jezdci();
                Jezdec.ID = reader.GetInt32(++i);
                Jezdec.Jmeno = reader.GetString(++i);
                Jezdec.Prijmeni = reader.GetString(++i);

                Jezdci.Add(Jezdec);
            }
            return Jezdci;
        }

        private static Collection<Staty> CteniStatu(SqlDataReader reader)
        {
            Collection<Staty> Staty = new Collection<Staty>();

            while (reader.Read())
            {
                int i = -1;
                Staty Stat = new Staty();
                Stat.ID = reader.GetInt32(++i);
                Stat.Nazev = reader.GetString(++i);
                Stat.Narodnost = reader.GetString(++i);

                Staty.Add(Stat);
            }
            return Staty;
        }
    }
}
