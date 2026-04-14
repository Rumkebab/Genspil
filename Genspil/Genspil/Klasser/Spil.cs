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
            bool erReserveret = bool.Parse(data[5]);
            bool erRequest = bool.Parse(data[6]);

            return new Spil(titel, genre, stand, pris, id, erReserveret, erRequest);
        }

        // Viser spillet pænt i konsollen
        public string VisInfo()
        {
            return $"{Id}: Titel: {Titel}, Genre: {Genre}, Stand: {Stand}, Pris: {Pris} kr., Reserveret: {ErReserveret}, Request: {ErRequest}";
        }
    }
}