using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace FormuleSystem.ORM.DAO.Sqls
{
    public class EvidenceJezdcu
    {
        public static String TABLE_NAME = "Jezdci";

        public static String SQL_SELECT = "SELECT ID, Jmeno, Prijmeni FROM Jezdci";
        public static String SQL_SELECT_MAX_ID = "SELECT MAX(ID) AS Maximum FROM Jezdci";
        public static String SQL_SELECT_DETAIL = "SELECT ID, Jmeno, Prijmeni, Startovni_cislo, Datum_narozeni, Tymy_ID, Staty_ID, Motory_Seriove_cislo FROM Jezdci WHERE ID = @id";
        public static String SQL_INSERT = "INSERT INTO Jezdci VALUES (@id, @jmeno, @prijmeni, @cislo, @datum, @tymyID, @statyID, @motoryID)";
        public static String SQL_UPDATE = "UPDATE Jezdci SET ID=@id, Jmeno=@jmeno, Prijmeni=@prijmeni, Startovni_cislo=@cislo, Datum_narozeni=@datum, Tymy_ID=@tymyID, Staty_ID=@statyID, Motory_Seriove_cislo=@motoryID WHERE ID = @id";
        public static String SQL_DELETE_ID = "DELETE FROM Jezdci WHERE ID=@id";
        public static String SQL_UPDATE_ENGINE = "EXECUTE VymenitMotor @id, @motor";

        // funkce 9.1
        public static int VlozeniJezdce(Jezdci Jezdec, Database pDb = null)
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
            command.Parameters.AddWithValue("@jmeno", Jezdec.Jmeno);
            command.Parameters.AddWithValue("@prijmeni", Jezdec.Prijmeni);
            command.Parameters.AddWithValue("@cislo", Jezdec.Startovni_cislo == null ? DBNull.Value : (object)Jezdec.Startovni_cislo);
            command.Parameters.AddWithValue("@datum", Jezdec.Datum_narozeni);
            command.Parameters.AddWithValue("@tymyID", Jezdec.Tymy_ID);
            command.Parameters.AddWithValue("@statyID", Jezdec.Staty_ID);
            command.Parameters.AddWithValue("@motoryID", Jezdec.Motory_Seriove_cislo);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.EndTransaction();
                db.Close();
            }

            return ret;
        }

        // funkce 9.2
        public static int UpravaJezdce(Jezdci Jezdec, Database pDb = null)
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
            command.Parameters.AddWithValue("@id", Jezdec.ID);
            command.Parameters.AddWithValue("@jmeno", Jezdec.Jmeno);
            command.Parameters.AddWithValue("@prijmeni", Jezdec.Prijmeni);
            command.Parameters.AddWithValue("@cislo", Jezdec.Startovni_cislo == null ? DBNull.Value : (object)Jezdec.Startovni_cislo);
            command.Parameters.AddWithValue("@datum", Jezdec.Datum_narozeni);
            command.Parameters.AddWithValue("@tymyID", Jezdec.Tymy_ID);
            command.Parameters.AddWithValue("@statyID", Jezdec.Staty_ID);
            command.Parameters.AddWithValue("@motoryID", Jezdec.Motory_Seriove_cislo);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 9.3
        public static int SmazaniJezdce(int id, Database pDb = null) 
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

            SqlCommand command = db.CreateCommand(SQL_DELETE_ID);
            command.Parameters.AddWithValue("@id", id);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 9.4
        public static Collection<Jezdci> VypisJezdcu(Database pDb = null)
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

            Collection<Jezdci> Jezdci = CteniJezdcu(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Jezdci;
        }

        // funkce 9.5
        public static int ZmenitNamontovanyMotor(int id, int motor, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_UPDATE_ENGINE);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@motor", motor);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 9.6 - tato funkce je přidaná, oproti analýze
        /*
         Tato funkce vypisuje informace o zvoleném jezdci 
         */
        public static Jezdci VypisJezdce(int id, Database pDb = null)
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

            Jezdci Jezdec = CteniJezdce(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Jezdec;
        }

        private static Jezdci CteniJezdce(SqlDataReader reader)
        {
            Jezdci Jezdec = new Jezdci();
            while (reader.Read()) { 
                int i = -1;
                Jezdec.ID = reader.GetInt32(++i);
                Jezdec.Jmeno = reader.GetString(++i);
                Jezdec.Prijmeni = reader.GetString(++i);
                if (!reader.IsDBNull(++i))
                {
                    Jezdec.Startovni_cislo = reader.GetInt32(i);
                }
                Jezdec.Datum_narozeni = reader.GetDateTime(++i);
                Jezdec.Tymy_ID = reader.GetInt32(++i);
                Jezdec.Staty_ID = reader.GetInt32(++i);
                Jezdec.Motory_Seriove_cislo = reader.GetInt32(++i);
            }
            return Jezdec;
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
    }
}
