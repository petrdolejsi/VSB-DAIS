using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace AuctionDesktopApp.ORM.DAO.Sqls
{
    public class EvidenceTymu
    {
        public static String TABLE_NAME = "Tymy";

        public static String SQL_SELECT = "SELECT ID, Nazev FROM Tymy";
        public static String SQL_SELECT_DETAIL = "SELECT * FROM Tymy WHERE ID = @id";
        public static String SQL_SELECT_JEZDCI = "SELECT ID, Jmeno, Prijmeni FROM Jezdci WHERE Tymy_ID = @id";
        public static String SQL_UPDATE = "UPDATE Tymy SET Nazev=@nazev, Dodavatel_pneumatik=@dodavatel, Vyrobce_motoru_ID=@vyrobce, Staty_ID=@staty WHERE ID = @id";
        public static String SQL_UPDATE_UZIVATEL = "UPDATE Tymy SET Nazev=@nazev, Dodavatel_pneumatik=@dodavatel, Vyrobce_motoru_ID=@vyrobce, Staty_ID=@staty WHERE Uzivatel_ID = @id";

        // funkce 7.1 zakomponovaná ve funkci 1.1
        // funkce 7.2
        public static int UpravaTymu(Tymy Tym, Database pDb = null)
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
            PrepareCommand(command, Tym);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        public static int UpravaTymuUzivatel(Tymy Tym, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_UPDATE_UZIVATEL);
            command.Parameters.AddWithValue("@id", Tym.Uzivatel_ID);
            command.Parameters.AddWithValue("@nazev", Tym.Nazev);
            command.Parameters.AddWithValue("@dodavatel", Tym.Dodavatel_pneumatik);
            command.Parameters.AddWithValue("@vyrobce", Tym.Vyrobce_motoru_ID);
            command.Parameters.AddWithValue("@staty", Tym.Staty_ID);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 7.3 zakomponovaná ve funkci 1.3
        // funkce 7.4
        public static Collection<Tymy> VypisTymu(Database pDb = null)
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

            Collection<Tymy> Tymy = CteniTymu(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Tymy;
        }

        // funkce 7.5
        public static Tymy VypisDetailuTymu(int id, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_DETAIL);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = db.Select(command);

            Tymy Tym = CtenuDetailuTymu(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Tym;
        }

        // funkce 7.6
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

        private static Tymy CtenuDetailuTymu(SqlDataReader reader)
        {
            Tymy Tym = new Tymy();
            while (reader.Read())
            {
                int i = -1;
                Tym.ID = reader.GetInt32(++i);
                Tym.Nazev = reader.GetString(++i);
                Tym.Dodavatel_pneumatik = reader.GetString(++i);
                Tym.Vyrobce_motoru_ID = reader.GetInt32(++i);
                Tym.Staty_ID = reader.GetInt32(++i);
                Tym.Uzivatel_ID = reader.GetInt32(++i);
            }
            return Tym;
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

        private static void PrepareCommand(SqlCommand command, Tymy Tym)
        {
            command.Parameters.AddWithValue("@id", Tym.ID);
            command.Parameters.AddWithValue("@nazev", Tym.Nazev);
            command.Parameters.AddWithValue("@dodavatel", Tym.Dodavatel_pneumatik);
            command.Parameters.AddWithValue("@vyrobce", Tym.Vyrobce_motoru_ID);
            command.Parameters.AddWithValue("@staty", Tym.Staty_ID);
            command.Parameters.AddWithValue("@uzivatel", Tym.Uzivatel_ID);
        }
    }
}
