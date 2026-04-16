using System.Collections;
using System.Timers;

namespace Genspil.Klasser // Ligger i mappen Klasser
{
    public enum Genre // bruger enum her for at vælge valg
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
            string titel = Titel.Length > 50 ? Titel.Substring(0, 46) + "... " : Titel; // Afkorter titel hvis den er for lang
            string status = "";
            if(ErReserveret)
            {   status += "(RESERVERET) "; }
            if(ErRequest)
            {   status += "(ØNSKET) "; }
            return $"{Id,-5}" + // Viser ID i en kolonne på 5 tegn
             $"{titel,-50}" + // Viser titel afkortet til max 50 tegn
             $"{Genre,-25}" + // Viser genre i kolonne på 15 tegn
             $"{Stand,-15}" + // Viser stand i kolonne på 15 tegn
             $"{Pris + " kr",10}" + // Viser pris højrejusteret i kolonne på 10 tegn
             $"{status,15}";// Viser status i kolonne på 20 tegn
        }
    }
}