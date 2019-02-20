using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace AuctionDesktopApp.ORM.DAO.Sqls
{
    public class EvidenceVysledku
    {
        public static String TABLE_NAME = "Vysledky";

        public static String SQL_INSERT = "INSERT INTO Vysledky VALUES (@gp, @poradi, @jezdec)";
        public static String SQL_UPDATE = "UPDATE Vysledky SET Body_Poradi=@poradi WHERE GP_ID=@gp AND Jezdci_ID=@jezdec";
        public static String SQL_DELETE = "DELETE FROM Vysledky WHERE GP_ID=@gp AND Body_poradi=@poradi";
        public static String SQL_SELECT_GP = "SELECT Body_Poradi, Jezdci_ID FROM Vysledky WHERE GP_ID=@gp ORDER BY CASE Body_Poradi WHEN 0 THEN 2 ELSE 1 END, Body_Poradi";
        public static String SQL_SELECT_PORADI_JEZDCU = "SELECT Jezdci.ID, Jmeno, Prijmeni, SUM(Pocet_bodu) AS Suma FROM Vysledky LEFT JOIN Body ON Vysledky.Body_Poradi = Body.Poradi LEFT JOIN Jezdci ON Jezdci.ID = Vysledky.Jezdci_ID GROUP BY Jezdci.ID, Jmeno, Prijmeni ORDER BY SUM(Pocet_bodu) DESC";
        public static String SQL_SELECT_PORADI_TYMU = "SELECT Tymy.ID, Nazev, SUM (Pocet_bodu) AS Suma FROM Vysledky LEFT JOIN Body ON Vysledky.Body_Poradi = Body.Poradi LEFT JOIN Jezdci ON Jezdci.ID= Vysledky.Jezdci_ID LEFT JOIN tymy ON tymy.ID= Jezdci.Tymy_ID GROUP BY Tymy.ID, Nazev ORDER BY SUM(Pocet_bodu) DESC";
        public static String SQL_SELECT_JEZDCE = "SELECT GP_ID, Body_Poradi FROM Vysledky WHERE Jezdci_ID=@id";
        public static String SQL_SELECT_COUNT = "SELECT MAX(Poradi) FROM Body WHERE NOT Pocet_bodu = 0";
        public static String SQL_SELECT_JEZDCE_BODOVAL_1 = "SELECT GP_ID, Body.Pocet_bodu FROM Vysledky JOIN Body ON Vysledky.Body_Poradi = Body.Poradi JOIN Jezdci ON Vysledky.Jezdci_ID = Jezdci.ID WHERE Jezdci.Startovni_cislo = @cislo AND Vysledky.Body_poradi <= @maximum AND Vysledky.Body_poradi > 0";
        public static String SQL_SELECT_JEZDCE_BODOVAL_2 = "SELECT GP_ID, Body.Pocet_bodu FROM Vysledky JOIN Body ON Vysledky.Body_Poradi = Body.Poradi WHERE Vysledky.Jezdci_ID = @cislo AND Vysledky.Body_poradi <= @maximum AND Vysledky.Body_poradi > 0";
        public static String SQL_SELECT_JEZDCE_BODOVAL_3 = "SELECT GP_ID, Vysledky.Body_poradi FROM Vysledky JOIN Jezdci ON Vysledky.Jezdci_ID = Jezdci.ID WHERE Jezdci.Startovni_cislo = @cislo AND Vysledky.Body_poradi <= @maximum AND Vysledky.Body_poradi > 0";
        public static String SQL_SELECT_JEZDCE_BODOVAL_4 = "SELECT GP_ID, Vysledky.Body_poradi FROM Vysledky WHERE Vysledky.Jezdci_ID = @cislo AND Vysledky.Body_poradi <= @maximum AND Vysledky.Body_poradi > 0";
        public static String SQL_SELECT_PORADI = "SELECT GP_ID, Jezdci_ID FROM Vysledky WHERE Body_Poradi=@poradi";

        // function 3.1
        public static int VlozeniVysledku(Vysledky Vysledek, Database pDb = null)
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
            command.Parameters.AddWithValue("@gp", Vysledek.GP_ID);
            command.Parameters.AddWithValue("@poradi", Vysledek.Body_Poradi);
            command.Parameters.AddWithValue("@jezdec", Vysledek.Jezdci_ID);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 3.2
        public static int UpravaVysledku(Vysledky Vysledek, Database pDb = null)
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
            command.Parameters.AddWithValue("@gp", Vysledek.GP_ID);
            command.Parameters.AddWithValue("@poradi", Vysledek.Body_Poradi);
            command.Parameters.AddWithValue("@jezdec", Vysledek.Jezdci_ID);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 3.3
        public static int SmazaniVysledku(Vysledky Vysledek, Database pDb = null)
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
            command.Parameters.AddWithValue("@gp", Vysledek.GP_ID);
            command.Parameters.AddWithValue("@poradi", Vysledek.Body_Poradi);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 3.4
        public static Collection<Vysledky> VypisVysledkuGP(int gp, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_GP);
            command.Parameters.AddWithValue("@gp", gp);
            SqlDataReader reader = db.Select(command);

            Collection<Vysledky> Vysledky = CteniPoradi(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Vysledky;
        }

        private static Collection<Vysledky> CteniPoradi(SqlDataReader reader)
        {
            Collection<Vysledky> Vysledky = new Collection<Vysledky>();

            while (reader.Read())
            {
                int i = -1;
                Vysledky Vysledek = new Vysledky();
                Vysledek.Body_Poradi = reader.GetInt32(++i);
                Vysledek.Jezdci_ID = reader.GetInt32(++i);

                Vysledky.Add(Vysledek);
            }
            return Vysledky;
        }

        // funkce 3.5
        public static Collection<Jezdci> VypisPoradiJezdcu(Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_PORADI_JEZDCU);
            SqlDataReader reader = db.Select(command);

            Collection<Jezdci> Jezdci = CteniPoradiJezdcu(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Jezdci;
        }

        private static Collection<Jezdci> CteniPoradiJezdcu(SqlDataReader reader)
        {
            Collection<Jezdci> Jezdci = new Collection<Jezdci>();

            while (reader.Read())
            {
                int i = -1;
                Jezdci Jezdec = new Jezdci();
                Jezdec.ID = reader.GetInt32(++i);
                Jezdec.Jmeno = reader.GetString(++i);
                Jezdec.Prijmeni = reader.GetString(++i);
                Jezdec.Suma = reader.GetInt32(++i);

                Jezdci.Add(Jezdec);
            }
            return Jezdci;
        }

        // funkce 3.6
        public static Collection<Tymy> VypisPoradiTymu(Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_PORADI_TYMU);
            SqlDataReader reader = db.Select(command);

            Collection<Tymy> Tymy = CteniPoradiTymu(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Tymy;
        }

        private static Collection<Tymy> CteniPoradiTymu(SqlDataReader reader)
        {
            Collection<Tymy> Tymy = new Collection<Tymy>();

            while (reader.Read())
            {
                int i = -1;
                Tymy Tym = new Tymy();
                Tym.ID = reader.GetInt32(++i);
                Tym.Nazev = reader.GetString(++i);
                Tym.Suma = reader.GetInt32(++i);

                Tymy.Add(Tym);
            }
            return Tymy;
        }

        // funkce 3.7
        public static Collection<Vysledky> VypisJezdceGP(int id, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_JEZDCE);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = db.Select(command);

            Collection<Vysledky> Vysledky = CteniJezdceGP(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Vysledky;
        }
        private static Collection<Vysledky> CteniJezdceGP(SqlDataReader reader)
        {
            Collection<Vysledky> Vysledky = new Collection<Vysledky>();

            while (reader.Read())
            {
                int i = -1;
                Vysledky Vysledek = new Vysledky();
                Vysledek.GP_ID = reader.GetInt32(++i);
                Vysledek.Body_Poradi = reader.GetInt32(++i);

                Vysledky.Add(Vysledek);
            }
            return Vysledky;
        }

        // funkce 3.8
        public static Collection<Vysledky> VypisJezdceBodovalGP(int id, String typ, String vypis, Database pDb = null)
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

            db.BeginTransaction();

            // poslední pozice, která je bodovaná
            SqlCommand command_count = db.CreateCommand(SQL_SELECT_COUNT);
            SqlDataReader reader = db.Select(command_count);

            int pozice = 0;

            while (reader.Read())
            {
                int i = -1;
                pozice = reader.GetInt32(++i);
            }
            reader.Close();

            SqlCommand command;
            if (typ == "startovni" && vypis == "body")
            {
                command = db.CreateCommand(SQL_SELECT_JEZDCE_BODOVAL_1);
            } else if (typ == "id" && vypis == "body")
            {
                command = db.CreateCommand(SQL_SELECT_JEZDCE_BODOVAL_2);
            } else if (typ == "startovni" && vypis == "poradi")
            {
                command = db.CreateCommand(SQL_SELECT_JEZDCE_BODOVAL_3);
            } else if (typ == "id" && vypis == "poradi")
            {
                command = db.CreateCommand(SQL_SELECT_JEZDCE_BODOVAL_4);
            } else
            {
                db.EndTransaction();
                if (pDb == null)
                {
                    db.Close();
                }
                return null;
            }

            command.Parameters.AddWithValue("@cislo", id);
            command.Parameters.AddWithValue("@maximum", pozice);
            reader = db.Select(command);

            Collection<Vysledky> Vysledky = CteniJezdceBodovalGP(reader);

            reader.Close();

            db.EndTransaction();

            if (pDb == null)
            {
                db.Close();
            }

            return Vysledky;
        }
        private static Collection<Vysledky> CteniJezdceBodovalGP(SqlDataReader reader)
        {
            Collection<Vysledky> Vysledky = new Collection<Vysledky>();

            while (reader.Read())
            {
                int i = -1;
                Vysledky Vysledek = new Vysledky();
                Vysledek.GP_ID = reader.GetInt32(++i);
                Vysledek.Body_Poradi = reader.GetInt32(++i);

                Vysledky.Add(Vysledek);
            }
            return Vysledky;
        }

        // funkce 3.9
        public static Collection<Vysledky> VypisJezdcuUmisteniGP(int pozice,Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_SELECT_PORADI);
            command.Parameters.AddWithValue("@poradi", pozice);
            SqlDataReader reader = db.Select(command);

            Collection<Vysledky> Vysledky = CteniJezdcuUmisteniGP(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Vysledky;
        }

        private static Collection<Vysledky> CteniJezdcuUmisteniGP(SqlDataReader reader)
        {
            Collection<Vysledky> Vysledky = new Collection<Vysledky>();

            while (reader.Read())
            {
                int i = -1;
                Vysledky Vysledek = new Vysledky();
                Vysledek.GP_ID = reader.GetInt32(++i);
                Vysledek.Jezdci_ID = reader.GetInt32(++i);

                Vysledky.Add(Vysledek);
            }
            return Vysledky;
        }
    }
}
