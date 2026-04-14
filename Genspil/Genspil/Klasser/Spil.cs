namespace Genspil.Klasser // Ligger i mappen Klasser
{
    public class Spil // Klasse der repræsenterer ét brætspil
    {
        // Static field til at holde styr på det næste ID der skal tildeles
        private static int lastId = 0;

        // Properties (data om spillet)
        public string Titel { get; set; }   // Navnet på spillet
        public Genre Genre { get; set; }    // Hvilken type spil det er
        public Stand Stand { get; set; }    // Hvor god stand spillet er i
        public int Pris { get; set; }       // Pris i kroner
        public bool ErReserveret { get; set; }       //Reserveret eller ej
        public bool ErRequest { get; set; }       // Ønsket eller ej
        public int Id { get; private set; }       // Unikt ID for hvert spil

        // Metode til at opdatere lastId når vi læser fra fil
        public static void SetLastId(int id)
        {
            if (id > lastId)
            {
                lastId = id;
            }
        }

        // Constructor - bruges når vi opretter et nyt spil
        public Spil(string titel, Genre genre, Stand stand, int pris, int id = 0, bool erReserveret = false, bool erRequest = false)
        {
            Titel = titel;   // Gemmer titel
            Genre = genre;   // Gemmer genre
            Stand = stand;   // Gemmer stand
            Pris = pris;     // Gemmer pris
            ErReserveret = erReserveret; // Standardværdi
            ErRequest = erRequest;    // Standardværdi
            if (id > 0)
            {
                Id = id;
                SetLastId(id); // Opdaterer lastId hvis vi loader fra fil
            }
            else
            {
                Id = GenerateId(); // Genererer et unikt ID fortløbende fra højeste ID i filen
            }
        }

        private int GenerateId()
        {
            lastId++;
            // Genererer et unikt ID baseret på tidspunktet og en tilfældig del
            return lastId;
        }

        // Bruges når vi skal gemme spillet som tekst i en fil
        public override string ToString()
        {
            // Laver objektet om til en tekstlinje med ; imellem
            return $"{Titel};{Genre};{Stand};{Pris};{Id}";
        }

        // Bruges når vi læser fra fil og skal lave tekst om til et objekt
        public static Spil FromString(string linje)
        {
            // Deler linjen op ved ;
            string[] data = linje.Split(';');

            // Henter værdierne fra arrayet
            string titel = data[0];
            Genre genre = Enum.Parse<Genre>(data[1]);
            Stand stand = Enum.Parse<Stand>(data[2]);
            int pris = int.Parse(data[3]);
            int id = int.Parse(data[4]);

            // Returnerer et nyt Spil-objekt med værdierne
            return new Spil(titel, genre, stand, pris, id);
        }

        // Bruges til at vise spillet pænt i konsollen
        public string VisInfo()
        {
            return $"{Id}: Titel: {Titel}, Genre: {Genre}, Stand: {Stand}, Pris: {Pris} kr.";
        }
    }
}