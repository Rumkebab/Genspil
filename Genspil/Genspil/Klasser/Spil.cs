namespace Genspil.Klasser // Ligger i mappen Klasser
{
    public class Spil // Klasse der repræsenterer ét brætspil
    {
        // Holder styr på sidste brugte ID
        private static int lastId = 0;

        // Oplysninger om spillet
        public string Titel { get; set; }
        public Genre Genre { get; set; }
        public Stand Stand { get; set; }
        public int Pris { get; set; }
        public int Id { get; private set; }
        public bool ErReserveret { get; set; }
        public bool ErRequest { get; set; }

        // Opdaterer sidste ID når vi læser fra fil
        public static void SetLastId(int id)
        {
            if (id > lastId)
            {
                lastId = id;
            }
        }

        // Constructor
        public Spil(string titel, Genre genre, Stand stand, int pris, int id = 0, bool erReserveret = false, bool erRequest = false)
        {
            Titel = titel;
            Genre = genre;
            Stand = stand;
            Pris = pris;
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

        // Laver nyt ID
        private int GenerateId()
        {
            lastId++;
            return lastId;
        }

        // Gør objektet til tekst til filen
        public override string ToString()
        {
<<<<<<< HEAD
            // Laver objektet om til en tekstlinje med ; imellem
=======
>>>>>>> Karl-på-Fiozi
            return $"{Titel};{Genre};{Stand};{Pris};{Id};{ErReserveret};{ErRequest}";
        }

        // Laver tekst fra filen om til et objekt
        public static Spil FromString(string linje)
        {
            string[] data = linje.Split(';');

            string titel = data[0];
            Genre genre = Enum.Parse<Genre>(data[1]);
            Stand stand = Enum.Parse<Stand>(data[2]);
            int pris = int.Parse(data[3]);
            int id = int.Parse(data[4]);
<<<<<<< HEAD
            bool erReserveret = data.Length > 5 ? bool.Parse(data[5]) : false;
            bool erRequest = data.Length > 6 ? bool.Parse(data[6]) : false;

            // Returnerer et nyt Spil-objekt med værdierne
=======
            bool erReserveret = bool.Parse(data[5]);
            bool erRequest = bool.Parse(data[6]);

>>>>>>> Karl-på-Fiozi
            return new Spil(titel, genre, stand, pris, id, erReserveret, erRequest);
        }

        // Viser spillet pænt i konsollen
        public string VisInfo()
        {
<<<<<<< HEAD
            // Truncater titlen hvis den er for lang, så det ikke ødelægger layoutet, tilføjer "..." for at indikere det er forkortet
            string titelTruncated = Titel.Length > 50 ? Titel.Substring(0, 47) + "..." : Titel;

            if (ErReserveret)
            {
                string info = $"{Id,-5} {titelTruncated,-50} {Genre,-12} {Stand,-8} {Pris,8} kr {"(RESERVERET)",20}";
                return info;
            }
            else if (ErRequest)
            {
                string info = $"{Id,-5} {titelTruncated,-50} {Genre,-12} {Stand,-8} {Pris,8} kr {"(ØNSKET)",20}";
                return info;
            }
            else
            {
                return $"{Id,-5} {titelTruncated,-50} {Genre,-12} {Stand,-8} {Pris,8} kr ";
            }
=======
            return $"{Id}: Titel: {Titel}, Genre: {Genre}, Stand: {Stand}, Pris: {Pris} kr., Reserveret: {ErReserveret}, Request: {ErRequest}";
>>>>>>> Karl-på-Fiozi
        }
    }
}