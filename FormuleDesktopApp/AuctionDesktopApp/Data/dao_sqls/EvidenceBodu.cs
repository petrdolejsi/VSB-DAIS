using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace AuctionDesktopApp.ORM.DAO.Sqls
{
    public class EvidenceBodu
    {
        public static String TABLE_NAME = "Body";

        public static String SQL_SELECT = "SELECT * FROM Body ORDER BY Poradi";
        public static String SQL_UPDATE = "UPDATE Body SET Pocet_bodu=@pocet WHERE Poradi=@poradi";
        public static String SQL_INSERT = "INSERT INTO Body VALUES (@poradi, @pocet)";
        public static String SQL_DELETE = "DELETE FROM Body WHERE Poradi=@poradi";

        // function 4.1
        public static int VlozeniPoradi(Body Body, Database pDb = null)
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
            command.Parameters.AddWithValue("@poradi", Body.Poradi);
            command.Parameters.AddWithValue("@pocet", Body.Pocet_bodu);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 4.2
        public static int UpravaPoradi(Body Body, Database pDb = null)
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
            command.Parameters.AddWithValue("@poradi", Body.Poradi);
            command.Parameters.AddWithValue("@pocet", Body.Pocet_bodu);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 4.3
        public static int SmazaniPoradi(int id, Database pDb = null)
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
            command.Parameters.AddWithValue("@poradi", id);
            int ret = db.ExecuteNonQuery(command);

            if (pDb == null)
            {
                db.Close();
            }

            return ret;
        }

        // funkce 4.4
        public static Collection<Body> VypisPoradi(Database pDb = null)
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

            Collection<Body> Body = CteniPoradi(reader);
            reader.Close();

            if (pDb == null)
            {
                db.Close();
            }

            return Body;
        }

        private static Collection<Body> CteniPoradi(SqlDataReader reader)
        {
            Collection<Body> Body = new Collection<Body>();

            while (reader.Read())
            {
                int i = -1;
                Body body = new Body();
                body.Poradi = reader.GetInt32(++i);
                body.Pocet_bodu = reader.GetInt32(++i);

                Body.Add(body);
            }
            return Body;
        }
    }
}
