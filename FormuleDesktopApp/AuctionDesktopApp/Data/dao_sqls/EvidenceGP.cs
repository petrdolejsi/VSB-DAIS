using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace AuctionDesktopApp.ORM.DAO.Sqls
{
    public class EvidenceGP
    {
        public static String TABLE_NAME = "GP";

        public static String SQL_UPDATE = "UPDATE GP SET Nazev = @nazev, Datum = @datum, Delka_okruhu = @delka, Pocet_kol = @pocet, Staty_ID = @stat WHERE ID = @id";
        public static String SQL_UPDATE_UZIVATEL = "UPDATE GP SET Nazev = @nazev, Datum = @datum, Delka_okruhu = @delka, Pocet_kol = @pocet, Staty_ID = @stat WHERE Uzivatel_ID = @id";
        public static String SQL_SELECT = "SELECT ID, Nazev, Datum FROM GP";
        public static String SQL_SELECT_DETAIL = "SELECT * FROM GP WHERE ID = @id";

        // funkce 2.1 zakomponovaná ve funkci 1.1
        // funkce 2.2
        public static int UpravaGP(GP GP, Database pDb = null)
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
            command.Parameters.AddWithValue("@nazev", GP.Nazev);
            command.Parameters.AddWithValue("@datum", GP.Datum);
            command.Parameters.AddWithValue("@delka", GP.Delka_okruhu);
            command.Parameters.AddWithValue("@pocet", GP.Pocet_kol);
            command.Parameters.AddWithValue("@stat", GP.Staty_ID);
            command.Parameters.AddWithValue("@id", GP.ID);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        public static int UpravaGPUzivatel(GP GP, Database pDb = null)
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
            command.Parameters.AddWithValue("@nazev", GP.Nazev);
            command.Parameters.AddWithValue("@datum", GP.Datum);
            command.Parameters.AddWithValue("@delka", GP.Delka_okruhu);
            command.Parameters.AddWithValue("@pocet", GP.Pocet_kol);
            command.Parameters.AddWithValue("@stat", GP.Staty_ID);
            command.Parameters.AddWithValue("@id", GP.Uzivatel_ID);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 2.3 zakomponovaná ve funkci 1.3
        // funkce 2.4
        public static Collection<GP> VypisGP(Database pDb = null)
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

            Collection<GP> GP = CteniGP(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return GP;
        }

        private static Collection<GP> CteniGP(SqlDataReader reader)
        {
            Collection<GP> GPs = new Collection<GP>();

            while (reader.Read())
            {
                int i = -1;
                GP GP = new GP();
                GP.ID = reader.GetInt32(++i);
                GP.Nazev = reader.GetString(++i);
                if (!reader.IsDBNull(++i))
                {
                    GP.Datum = reader.GetDateTime(i);
                };

                GPs.Add(GP);
            }
            return GPs;
        }

        // funkce 2.4
        public static GP VypisGPDetail(int id,Database pDb = null)
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

            GP GP = CteniGPDetail(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return GP;
        }
        private static GP CteniGPDetail(SqlDataReader reader)
        {
            GP GP = new GP();

            if (reader.Read())
            {
                int i = -1;
                GP.ID = reader.GetInt32(++i);
                GP.Nazev = reader.GetString(++i);
                if (!reader.IsDBNull(++i))
                {
                    GP.Datum = reader.GetDateTime(i);
                }
                GP.Delka_okruhu = reader.GetDouble(++i);
                if (!reader.IsDBNull(++i))
                {
                    GP.Pocet_kol = reader.GetInt32(i);
                }
                GP.Staty_ID = reader.GetInt32(++i);
                GP.Uzivatel_ID = reader.GetInt32(++i);

            }
            return GP;
        }
    }
}
