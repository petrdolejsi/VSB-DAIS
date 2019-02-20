using System;
using System.Collections.ObjectModel;
using FormuleSystem.ORM;
using FormuleSystem.ORM.DAO.Sqls;
using System.Collections.Generic;

namespace FormuleSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            db.Connect();

            try {
                Funkce1(db);
                Funkce2(db);
                Funkce3(db);
                Funkce4(db);
                Funkce5(db);
                Funkce6(db);
                Funkce7(db);
                Funkce8(db);
                Funkce9(db);
            }
            catch (Exception)
            {
                db.Rollback();
                Console.WriteLine("Vyskytla se chyba, program nepokračuje");
            }

            db.Close();
        }

        static void Funkce1(Database db)
        {
            

            Console.WriteLine("Funkce 1 --------------------------");
            Console.WriteLine("");
            Tymy Tym = new Tymy();
            Tym.Nazev = "Škoda Team";
            Tym.Dodavatel_pneumatik = "Barum";
            Tym.Vyrobce_motoru_ID = 1;
            Tym.Staty_ID = 1;
            EvidenceUzivatelu.VlozeniUzivatele("uzivatel1", "mojeHeslo", "Team", Tym, null, db); // funkce 1.1

            db.BeginTransaction();

            EvidenceUzivatelu.UpravaUzivatele(34, "asdasd", db);    // funkce 1.2
            EvidenceUzivatelu.UpravaHeslaUzivatele(36, "hesloheslo", db); // funkce nová - změna hesla uživatele

            Console.WriteLine("Vypis vsech uzivatelu (funkce 1.4):");
            Collection<Uzivatel> Uzivatele = EvidenceUzivatelu.VypisUzivatelu(db);  // funkce 1.4
            foreach (Uzivatel Polozka in Uzivatele)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Login);
            }

            EvidenceUzivatelu.SmazaniUzivatele(35, db); // funkce 1.3

            Console.WriteLine("");
            Console.WriteLine("Vypis detailu uzivatele (funkce 1.5):");
            Dictionary<string, string> uzivatel =  EvidenceUzivatelu.VypisUzivatele(34, db);    // funkce 1.5
            if (uzivatel["Type"] == "Team")
            {
                Console.WriteLine(uzivatel["UserID"] + "\t");
                Console.Write(uzivatel["Type"] + "\t");
                Console.Write(uzivatel["TymID"] + "\t");
                Console.Write(uzivatel["Nazev"] + "\t");
                Console.Write(uzivatel["Dodavatel_pneumatik"] + "\t");
                Console.Write(uzivatel["Staty"] + "\t");
                Console.Write(uzivatel["Vyrobce_motoru"]);
            } else if (uzivatel["Type"] == "GP")
            {
                Console.WriteLine(uzivatel["UserID"] + "\t" + uzivatel["Type"] + "\t" + uzivatel["GPID"] + "\t" + uzivatel["Nazev"] + "\t" + uzivatel["Datum"] + "\t" + uzivatel["Delka_okruhu"] + "\t" + uzivatel["Pocet_kol"] + "\t" + uzivatel["Staty"]);
            }

            db.EndTransaction();
        }

        static void Funkce2(Database db)
        {
            db.BeginTransaction();
            Console.WriteLine("");
            Console.WriteLine("Funkce 2 --------------------------");
            Console.WriteLine("");

            // funkce 2.1 je zakomponovaná ve funkci 1.1

            Console.WriteLine("Vypis vsech GP (funkce 2.4):");
            Collection<GP> GP = EvidenceGP.VypisGP(db); // funkce 2.4
            foreach (GP Polozka in GP)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Nazev + "\t" + Polozka.Datum);
            }

            // funkce 2.3 zakomponovaná ve funkci 1.3

            Console.WriteLine("");
            Console.WriteLine("Vypis detailu GP (funkce 2.5):");
            GP Vypis = EvidenceGP.VypisGPDetail(5, db); // funkce 2.5
            Console.WriteLine();
            Console.WriteLine(Vypis.ID + "\t" + Vypis.Nazev + "\t" + Vypis.Datum + "\t" + Vypis.Delka_okruhu + "\t" + Vypis.Pocet_kol + "\t" + Vypis.Staty_ID);

            EvidenceGP.UpravaGP(Vypis, db); // funkce 2.2

            db.EndTransaction();
        }

        static void Funkce3(Database db)
        {
            db.BeginTransaction();
            Console.WriteLine("");
            Console.WriteLine("Funkce 3 --------------------------");
            Console.WriteLine("");

            Vysledky Vysledek = new Vysledky();
            Vysledek.GP_ID = 21;
            Vysledek.Body_Poradi = 10;
            Vysledek.Jezdci_ID = 26;
            EvidenceVysledku.VlozeniVysledku(Vysledek, db); // funkce 3.1

            Vysledek.Body_Poradi = 0;
            EvidenceVysledku.UpravaVysledku(Vysledek, db);  // funkce 3.2

            Console.WriteLine();
            Console.WriteLine("Vypis poradi pro vybranout GP (funkce 3.3):");
            Collection<Vysledky> Vysledky = EvidenceVysledku.VypisVysledkuGP(1,db); // funkce 3.3
            foreach (Vysledky Polozka in Vysledky)
            {
                Console.WriteLine(Polozka.Body_Poradi + "\t" + Polozka.Jezdci_ID);
            }

            EvidenceVysledku.SmazaniVysledku(Vysledek, db); // funkce nová
            db.EndTransaction();

            Console.WriteLine();
            Console.WriteLine("Vypis tabulky jezdcu (funkce 3.4):");
            Collection<Jezdci> Jezdci = EvidenceVysledku.VypisPoradiJezdcu(db); // funkce 3.4
            foreach (Jezdci Polozka in Jezdci)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Jmeno + "\t" + Polozka.Prijmeni + "\t" + Polozka.Suma);
            }

            Console.WriteLine();
            Console.WriteLine("Vypis tabulky tymu (funkce 3.5):");
            Collection<Tymy> Tymy = EvidenceVysledku.VypisPoradiTymu(db); // funkce 3.5
            foreach (Tymy Polozka in Tymy)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Nazev + "\t" + Polozka.Suma);
            }

            Console.WriteLine();
            Console.WriteLine("Vypis poradi v odjetch GP zvoleneho jezdce (funkce 3.6):");
            Vysledky = EvidenceVysledku.VypisJezdceGP(1,db); // funkce 3.6
            foreach (Vysledky Polozka in Vysledky)
            {
                Console.WriteLine(Polozka.GP_ID + "\t" + Polozka.Body_Poradi);
            }

            Console.WriteLine();
            Console.WriteLine("Vypis poradi v odjetch GP zvoleneho jezdce, pouze, kde bodoval (funkce 3.7):");
            Vysledky = EvidenceVysledku.VypisJezdceBodovalGP(5,"id","body", db); // funkce 3.7
            foreach (Vysledky Polozka in Vysledky)
            {
                Console.WriteLine(Polozka.GP_ID + "\t" + Polozka.Body_Poradi);
            }

            Console.WriteLine();
            Console.WriteLine("Vypis jezdcu pro zadane umisteni v odjetych GP (funkce 3.8):");
            Vysledky = EvidenceVysledku.VypisJezdcuUmisteniGP(6,db); // funkce 3.8
            foreach (Vysledky Polozka in Vysledky)
            {
                Console.WriteLine(Polozka.GP_ID + "\t" + Polozka.Jezdci_ID);
            }
        }
        static void Funkce4(Database db)
        {
            db.BeginTransaction();
            Console.WriteLine("");
            Console.WriteLine("Funkce 4 --------------------------");
            Console.WriteLine("");

            Body Pozice = new Body();
            Pozice.Poradi = 123;
            Pozice.Pocet_bodu = 123;
            EvidenceBodu.VlozeniPoradi(Pozice, db); // funkce 4.1

            Pozice.Pocet_bodu = 456;
            EvidenceBodu.UpravaPoradi(Pozice, db); // funkce 4.2

            Console.WriteLine();
            Console.WriteLine("Vypis vsech poradi (funkce 4.4):");
            Collection<Body> Body = EvidenceBodu.VypisPoradi(db); // funkce 4.4
            foreach (Body Polozka in Body)
            {
                Console.WriteLine(Polozka.Poradi + "\t" + Polozka.Pocet_bodu);
            }

            EvidenceBodu.SmazaniPoradi(Pozice.Poradi, db); // funkce 4.3

            db.EndTransaction();
        }
        static void Funkce5(Database db)
        {
            db.BeginTransaction();
            Console.WriteLine("");
            Console.WriteLine("Funkce 5 --------------------------");
            Console.WriteLine("");

            Staty Stat = new Staty();
            Stat.Narodnost = "Česká";
            Stat.Nazev = "Česká Republika";
            EvidenceStatu.VlozeniStatu(Stat, db);   // funkce 5.1

            Stat.Nazev = "Česko";
            EvidenceStatu.UpravaStatu(Stat, db);    // funkce 5.2

            Console.WriteLine();
            Console.WriteLine("Vypis vsech statu (funkce 5.4):");
            Collection<Staty> Staty = EvidenceStatu.VypisStatu(db); // funkce 5.4
            foreach (Staty Polozka in Staty)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Narodnost + "\t" + Polozka.Nazev);
            }

            EvidenceStatu.SmazaniStatu(30, db);    // funkce 5.3

            Console.WriteLine();
            Console.WriteLine("Vypis tymu z GB (funkce 5.5):");
            Collection<Tymy> Tymy = EvidenceStatu.VypisTymu(2, db); // funkce 5.5
            foreach (Tymy Polozka in Tymy)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Nazev);
            }

            Console.WriteLine();
            Console.WriteLine("Vypis jezdcu z GB (funkce 5.6):");
            Collection<Jezdci> Jezdci = EvidenceStatu.VypisJezdcu(2, db); // funkce 5.6
            foreach (Jezdci Polozka in Jezdci)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Jmeno + "\t" + Polozka.Prijmeni);
            }

            db.EndTransaction();
        }
        static void Funkce6(Database db)
        {
            db.BeginTransaction();
            Console.WriteLine("");
            Console.WriteLine("Funkce 6 --------------------------");
            Console.WriteLine("");

            Vyrobce_motoru Vyrobce = new Vyrobce_motoru();
            Vyrobce.Nazev = "Porsche";
            EvidenceVyrobcuMotoru.VlozeniVyrobceMotoru(Vyrobce, db); // funkce 6.1

            Vyrobce.Nazev = "Škoda";
            EvidenceVyrobcuMotoru.UpravaVyrobceMotoru(Vyrobce, db); // funkce 6.2

            Console.WriteLine();
            Console.WriteLine("Vypis vsech vyrobcu motoru (funkce 6.4):");
            Collection<Vyrobce_motoru> Vyrobci = EvidenceVyrobcuMotoru.VypisVyrobcuMotoru(db); // funkce 6.4
            foreach (Vyrobce_motoru Polozka in Vyrobci)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Nazev);
            }

            EvidenceVyrobcuMotoru.SmazaniVyrobceMotoru(6, db); // funkce 6.3

            db.EndTransaction();
        }

        static void Funkce7(Database db)
        {
            db.BeginTransaction();
            Console.WriteLine("");
            Console.WriteLine("Funkce 7 --------------------------");
            Console.WriteLine("");

            // funkce 7.1 zakomponovaná ve funkci 1.1
            // funkce 7.3 zakomponovaná ve funkci 1.3

            Console.WriteLine();
            Console.WriteLine("Vypis sech tymu (funkce 7.4):");
            Collection<Tymy> Tymy = EvidenceTymu.VypisTymu(db); // funkce 7.4
            foreach (Tymy Polozka in Tymy)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Nazev);
            }

            Console.WriteLine();
            Console.WriteLine("Vypis detailu tymu (funkce 7.5):");
            Tymy Vypis = EvidenceTymu.VypisDetailuTymu(1, db); // funkce 7.5
            Console.WriteLine(Vypis.ID + "\t" + Vypis.Nazev + "\t" + Vypis.Dodavatel_pneumatik + "\t" + Vypis.Vyrobce_motoru_ID + "\t" + Vypis.Staty_ID + "\t" + Vypis.Uzivatel_ID);

            EvidenceTymu.UpravaTymu(Vypis, db);  // funkce 7.2

            Console.WriteLine();
            Console.WriteLine("Vypis vsech jezdcu zvoleneho tymu (funkce 7.6):");
            Collection<Jezdci> Jezdci = EvidenceTymu.VypisJezdcu(1, db); // funkce 7.6
            foreach (Jezdci Polozka in Jezdci)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Jmeno + "\t" + Polozka.Prijmeni);
            }

            db.EndTransaction();
        }
        static void Funkce8(Database db)
        {
            db.BeginTransaction();
            Console.WriteLine("");
            Console.WriteLine("Funkce 8 --------------------------");
            Console.WriteLine("");

            EvidenceMotoru.VlozeniMotoru(9999, db);  // funkce 8.1

            Console.WriteLine();
            Console.WriteLine("Vypis vsech motoru (funkce 8.8):");
            Collection<Motory> Motory = EvidenceMotoru.VypisMotoru(db); // funkce 8.3
            foreach (Motory Polozka in Motory)
            {
                Console.WriteLine(Polozka.Seriove_cislo + "\t" + Polozka.Pocet_pouziti);
            }
            
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Vypis vsech namontovanych motoru (funkce 8.4):");
            Motory = EvidenceMotoru.VypisNamontovanychMotoru(db); // funkce 8.4
            foreach (Motory Polozka in Motory)
            {
                Console.WriteLine(Polozka.Seriove_cislo + "\t" + Polozka.Pocet_pouziti);
            }

            EvidenceMotoru.SmazaniMotoru(9999,db);  // funkce 8.2

            db.EndTransaction();
        }
        static void Funkce9 (Database db)
        {
            db.BeginTransaction();
            Console.WriteLine("");
            Console.WriteLine("Funkce 9 --------------------------");
            Console.WriteLine("");

            Jezdci Jezdec = new Jezdci();
            Jezdec.Jmeno = "Lance";
            Jezdec.Prijmeni = "Stroll";
            Jezdec.Datum_narozeni = new DateTime(2000, 12, 25);
            Jezdec.Tymy_ID = 1;
            Jezdec.Staty_ID = 1;
            Jezdec.Motory_Seriove_cislo = 4848;

            EvidenceJezdcu.VlozeniJezdce(Jezdec, db);    // funkce 9.1

            Jezdec.Datum_narozeni = new DateTime(1990, 12, 25);
            EvidenceJezdcu.UpravaJezdce(Jezdec,db);     // funkce 9.2

            EvidenceJezdcu.ZmenitNamontovanyMotor(28, 2727,db);   // funkce 9.5

            Console.WriteLine();
            Console.WriteLine("Vypis vsech jezdcu (funkce 9.4):");
            Collection<Jezdci> Jezdci = EvidenceJezdcu.VypisJezdcu(db); // funkce 9.4
            foreach (Jezdci Polozka in Jezdci)
            {
                Console.WriteLine(Polozka.ID + "\t" + Polozka.Jmeno + "\t" + Polozka.Prijmeni);
            }

            Console.WriteLine();
            Console.WriteLine("Vypis detailu jezdce (funkce nová):");
            Jezdci Vypis = EvidenceJezdcu.VypisJezdce(1,db); // funkce 9.6 nová
            Console.WriteLine();
            Console.WriteLine(Vypis.ID + "\t" + Vypis.Jmeno + "\t" + Vypis.Prijmeni + "\t" + Vypis.Startovni_cislo + "\t" + Vypis.Datum_narozeni + "\t" + Vypis.Tymy_ID + "\t" + Vypis.Staty_ID);

            EvidenceJezdcu.SmazaniJezdce(27,db);    // funkce 9.3

            db.EndTransaction();
        }

    }
}
