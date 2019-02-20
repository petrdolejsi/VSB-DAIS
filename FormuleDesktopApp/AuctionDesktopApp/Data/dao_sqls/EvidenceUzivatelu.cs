using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace AuctionDesktopApp.ORM.DAO.Sqls
{
    public class EvidenceUzivatelu
    {
        public static String TABLE_NAME = "Uzivatel";

        public static String SQL_SELECT_MAX_ID = "SELECT MAX(ID) AS Maximum FROM Uzivatel";
        public static String SQL_SELECT_MAX_ID_TYMY = "SELECT MAX(ID) AS Maximum FROM Tymy";
        public static String SQL_SELECT_MAX_ID_GP = "SELECT MAX(ID) AS Maximum FROM GP";
        public static String SQL_INSERT = "EXECUTE VlozitUzivatele @login, @heslo, @typ, @info1_string, @info2_string, @info1_int, @info2_int, @info1_double, @info1_datum";
        public static String SQL_UPDATE_USER = "UPDATE Uzivatel SET Login=@login WHERE ID=@id";
        public static String SQL_UPDATE_PASSWORD = "UPDATE Uzivatel SET Heslo=@heslo WHERE ID=@id";
        public static String SQL_DELETE = "DELETE FROM Uzivatel WHERE ID = @id";
        public static String SQL_SELECT = "SELECT ID, Login, Type FROM Uzivatel";
        public static String SQL_SELECT_DETAIL_TYMY = "SELECT Uzivatel.ID AS UserID, Login, Type, Tymy.ID AS TymID, Tymy.Nazev, Dodavatel_pneumatik, Staty.Nazev AS Stat, Vyrobce_motoru.Nazev FROM Uzivatel JOIN Tymy ON Uzivatel.ID = Tymy.Uzivatel_ID JOIN Staty ON Tymy.Staty_ID = Staty.ID JOIN Vyrobce_motoru ON Tymy.Vyrobce_motoru_ID = Vyrobce_motoru.ID WHERE Uzivatel.ID = @id";
        public static String SQL_SELECT_DETAIL_GP = "SELECT Uzivatel.ID AS UserID, Login, Type, GP.ID AS GPID, GP.Nazev, Datum, Delka_okruhu, Pocet_kol, Staty.Nazev AS Stat FROM Uzivatel JOIN GP ON Uzivatel.ID = GP.Uzivatel_ID JOIN Staty ON GP.Staty_ID = Staty.ID WHERE Uzivatel.ID = @id";
        public static String SQL_SELECT_DETAIL = "SELECT ID, Login, Type FROM Uzivatel WHERE ID = @id";

        // funkce 1.1
        public static int VlozeniUzivatele(String login, String heslo, String type, Tymy tym = null, GP gp = null, Database pDb = null)
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

            int ret=0;
            if ((tym != null && type == "Team")||(gp != null && type == "GP")||(type == "FIA"))
            {
                if (type == "Team")
                {
                    SqlCommand command_insert = db.CreateCommand(SQL_INSERT);
                    command_insert.Parameters.AddWithValue("@login", login);
                    command_insert.Parameters.AddWithValue("@heslo", heslo);
                    command_insert.Parameters.AddWithValue("@typ", type);
                    command_insert.Parameters.AddWithValue("@info1_string", tym.Nazev);
                    command_insert.Parameters.AddWithValue("@info2_string", tym.Dodavatel_pneumatik);
                    command_insert.Parameters.AddWithValue("@info1_int", tym.Vyrobce_motoru_ID);
                    command_insert.Parameters.AddWithValue("@info2_int", tym.Staty_ID);
                    command_insert.Parameters.AddWithValue("@info1_double", (double)0);
                    command_insert.Parameters.AddWithValue("@info1_datum", new DateTime(2008, 6, 1, 7, 47, 0));
                    ret = db.ExecuteNonQuery(command_insert);
                } else if (type == "GP")
                {
                    SqlCommand command_insert = db.CreateCommand(SQL_INSERT);
                    command_insert.Parameters.AddWithValue("@login", login);
                    command_insert.Parameters.AddWithValue("@heslo", heslo);
                    command_insert.Parameters.AddWithValue("@typ", type);
                    command_insert.Parameters.AddWithValue("@info1_string", gp.Nazev);
                    command_insert.Parameters.AddWithValue("@info1_datum", gp.Datum);
                    command_insert.Parameters.AddWithValue("@info1_double", gp.Delka_okruhu);
                    command_insert.Parameters.AddWithValue("@info1_int", gp.Pocet_kol);
                    command_insert.Parameters.AddWithValue("@info2_int", gp.Staty_ID);
                    command_insert.Parameters.AddWithValue("@info2_string", "");
                    ret = db.ExecuteNonQuery(command_insert);
                }
            }

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 1.2 
        public static int UpravaUzivatele(int id, String login, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_UPDATE_USER);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@login", login);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce nová - změna hesla uživatele
        public static int UpravaHeslaUzivatele(int id, String heslo, Database pDb = null)
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

            SqlCommand command = db.CreateCommand(SQL_UPDATE_PASSWORD);
            command.Parameters.AddWithValue("@heslo", heslo);
            command.Parameters.AddWithValue("@id", id);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 1.3
        public static int SmazaniUzivatele(int id, Database pDb = null)
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

        // funkce 1.4
        public static Collection<Uzivatel> VypisUzivatelu(Database pDb = null)
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

            Collection<Uzivatel> Uzivatele = CteniUzivatelu(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Uzivatele;
        }

        private static Collection<Uzivatel> CteniUzivatelu(SqlDataReader reader)
        {
            Collection<Uzivatel> Uzivatele = new Collection<Uzivatel>();

            while (reader.Read())
            {
                int i = -1;
                Uzivatel Uzivatel = new Uzivatel();
                Uzivatel.ID = reader.GetInt32(++i);
                Uzivatel.Login = reader.GetString(++i);
                Uzivatel.Type = reader.GetString(++i);

                Uzivatele.Add(Uzivatel);
            }
            return Uzivatele;
        }

        // funkce 1.5
        public static Dictionary<string, string> VypisUzivatele(int id, Database pDb = null)
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

            SqlCommand command_type = db.CreateCommand(SQL_SELECT_DETAIL);
            command_type.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = db.Select(command_type);

            String typ = "";

            while (reader.Read())
            {
                int i = -1;
                reader.GetInt32(++i);
                ++i;
                typ = reader.GetString(++i);
            }
            reader.Close();


            Dictionary<string, string> uzivatel = new Dictionary<string, string>();
            SqlCommand command;
            if (typ=="Team")
            {
                command = db.CreateCommand(SQL_SELECT_DETAIL_TYMY);
                command.Parameters.AddWithValue("@id", id);
                reader = db.Select(command);

                uzivatel = CteniUzivateleTym(reader);
                reader.Close();
            } else if (typ == "GP")
            {
                command = db.CreateCommand(SQL_SELECT_DETAIL_GP);
                command.Parameters.AddWithValue("@id", id);
                reader = db.Select(command);

                uzivatel = CteniUzivateleGP(reader);
                reader.Close();
            } else
            {
                command = db.CreateCommand(SQL_SELECT_DETAIL);
                command.Parameters.AddWithValue("@id", id);
                reader = db.Select(command);

                uzivatel = CteniUzivatele(reader);
                reader.Close();
            }

            if (pDb == null)
            {
                db.Close();
            }

            return uzivatel;
        }

        private static Dictionary<string, string> CteniUzivateleTym(SqlDataReader reader)
        {
            Dictionary<string, string> uzivatel = new Dictionary<string, string>();
            if (reader.Read())
            {
                int i = -1;
                uzivatel.Add("UserID", reader.GetInt32(++i).ToString());
                uzivatel.Add("Login", reader.GetString(++i));
                uzivatel.Add("Type", reader.GetString(++i));
                uzivatel.Add("TymID", reader.GetInt32(++i).ToString());
                uzivatel.Add("Nazev", reader.GetString(++i));
                uzivatel.Add("Dodavatel_pneumatik", reader.GetString(++i));
                uzivatel.Add("Staty", reader.GetString(++i));
                uzivatel.Add("Vyrobce_motoru", reader.GetString(++i));
            }
            return uzivatel;
        }

        private static Dictionary<string, string> CteniUzivateleGP(SqlDataReader reader)
        {
            Dictionary<string, string> uzivatel = new Dictionary<string, string>();
            if (reader.Read())
            {
                int i = -1;
                uzivatel.Add("UserID", reader.GetInt32(++i).ToString());
                uzivatel.Add("Login", reader.GetString(++i));
                uzivatel.Add("Type", reader.GetString(++i));
                uzivatel.Add("GPID", reader.GetInt32(++i).ToString());
                uzivatel.Add("Nazev", reader.GetString(++i));
                if (!reader.IsDBNull(++i))
                {
                    uzivatel.Add("Datum", reader.GetDateTime(i).ToString());
                } else
                {
                    uzivatel.Add("Datum", "");
                }
                uzivatel.Add("Delka_okruhu", reader.GetDouble(++i).ToString());
                if (!reader.IsDBNull(++i))
                {
                    uzivatel.Add("Pocet_kol", reader.GetInt32(i).ToString());
                } else
                {
                    uzivatel.Add("Pocet_kol", "");
                }
                uzivatel.Add("Staty", reader.GetString(++i));
            }
            return uzivatel;
        }

        private static Dictionary<string, string> CteniUzivatele(SqlDataReader reader)
        {
            Dictionary<string, string> uzivatel = new Dictionary<string, string>();
            if (reader.Read())
            {
                int i = -1;
                uzivatel.Add("UserID", reader.GetInt32(++i).ToString());
                uzivatel.Add("Login", reader.GetString(++i));
                uzivatel.Add("Type", reader.GetString(++i));
            }
            return uzivatel;
        }
    }
}
