namespace Genspil.Klasser
{
    public enum Genre
    {
        Strategi,
        Familie,
        Kortspil,
        Quiz,
        Samarbejde
    }

    public enum Stand
    {
        Ny,
        Uåbnet,
        God,
        Slidt,
        Reparation
    }

    public class Spil
    {
        // Holder styr på sidste brugte ID
        private static int lastId = 0;

        public string Titel { get; set; }
        public Genre Genre { get; set; }
        public Stand Stand { get; set; }
        public string AntalSpillere { get; set; }
        public int Pris { get; set; }
        public int Id { get; private set; }
        public bool ErReserveret { get; set; }
        public bool ErRequest { get; set; }

        public static void SetLastId(int id)
        {
            if (id > lastId)
            {
                lastId = id;
            }
        }

        // Opretter et spil og giver automatisk ID hvis der ikke er et i forvejen
        public Spil(string titel, Genre genre, Stand stand, int pris, string antalSpillere, int id = 0, bool erReserveret = false, bool erRequest = false)
        {
            Titel = titel;
            Genre = genre;
            Stand = stand;
            Pris = pris;
            AntalSpillere = antalSpillere;
            ErReserveret = erReserveret;
            ErRequest = erRequest;

            if (id > 0)
            {
                Id = id;
                SetLastId(id);
            }
            else
            {
                Id = GenerateId();
            }
        }

        private int GenerateId()
        {
            lastId++;
            return lastId;
        }

        // Gemmer spillet i samme rækkefølge som FromString læser det
        public override string ToString()
        {
            return $"{Titel};{Genre};{Stand};{Pris};{AntalSpillere};{Id};{ErReserveret};{ErRequest}";
        }

        public static Spil FromString(string linje)
        {
            string[] data = linje.Split(';');

            string titel = data[0];
            Genre genre = Enum.Parse<Genre>(data[1]);
            Stand stand = Enum.Parse<Stand>(data[2]);
            int pris = int.Parse(data[3]);
            string antalSpillere = data[4];
            int id = int.Parse(data[5]);
            bool erReserveret = bool.Parse(data[6]);
            bool erRequest = bool.Parse(data[7]);

            return new Spil(titel, genre, stand, pris, antalSpillere, id, erReserveret, erRequest);
        }

        // Bruges til at vise spillet pænt i tabellen
        public string VisInfo()
        {
            string titel = Titel.Length > 50 ? Titel.Substring(0, 46) + "..." : Titel;
            string status = "";

            if (ErReserveret)
                status += "(RESERVERET) ";

            if (ErRequest)
                status += "(ØNSKET)";

            return $"{Id,-5}" +
                   $"{titel,-50}" +
                   $"{Genre,-15}" +
                   $"{AntalSpillere,-12}" +
                   $"{Stand,-15}" +
                   $"{Pris + " kr",8}" +
                   $"{status,15}";
        }
    }
}