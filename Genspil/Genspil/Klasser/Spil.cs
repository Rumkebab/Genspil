namespace Genspil.Klasser // Ligger i mappen Klasser
{
    public class Spil // Klasse der repræsenterer ét brætspil
    {
        // Properties (data om spillet)
        public string Titel { get; set; }   // Navnet på spillet
        public Genre Genre { get; set; }    // Hvilken type spil det er
        public Stand Stand { get; set; }    // Hvor god stand spillet er i
        public int Pris { get; set; }       // Pris i kroner

        // Constructor - bruges når vi opretter et nyt spil
        public Spil(string titel, Genre genre, Stand stand, int pris)
        {
            Titel = titel;   // Gemmer titel
            Genre = genre;   // Gemmer genre
            Stand = stand;   // Gemmer stand
            Pris = pris;     // Gemmer pris
        }

        // Bruges når vi skal gemme spillet som tekst i en fil
        public override string ToString()
        {
            // Laver objektet om til en tekstlinje med ; imellem
            return $"{Titel};{Genre};{Stand};{Pris}";
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

            // Returnerer et nyt Spil-objekt med værdierne
            return new Spil(titel, genre, stand, pris);
        }

        // Bruges til at vise spillet pænt i konsollen
        public string VisInfo()
        {
            return $"Titel: {Titel}, Genre: {Genre}, Stand: {Stand}, Pris: {Pris} kr.";
        }
    }
}