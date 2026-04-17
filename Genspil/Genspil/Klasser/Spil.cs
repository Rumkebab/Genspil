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
        // Holder styr på sidste brugte ID på tværs af alle spil-instanser
        private static int lastId = 0;

        // Spillets grundlæggende information
        public string Titel { get; private set; }
        public Genre Genre { get; private set; }
        public Stand Stand { get; private set; }
        public string AntalSpillere { get; private set; }
        public int Pris { get; private set; }
        public int Id { get; private set; }

        // Status for reservation og ønskeliste
        public bool ErReserveret { get; private set; }
        public bool ErRequest { get; private set; }
        public string Kontaktperson { get; private set; }
        public string ReserveretAf { get; private set; }

        // Metode til at opdatere reservation
        public void SætReservation(string navn)
        {
            ErReserveret = true;
            ReserveretAf = navn;
        }

        // Metode til at fjerne reservation
        public void FjernReservation()
        {
            ErReserveret = false;
            ReserveretAf = "";
        }

        // Metoder til at redigere spiloplysninger
        public void OpdaterTitel(string nyTitel)
        {
            Titel = nyTitel;
        }

        public void OpdaterGenre(Genre nyGenre)
        {
            Genre = nyGenre;
        }

        public void OpdaterStand(Stand nyStand)
        {
            Stand = nyStand;
        }

        public void OpdaterAntalSpillere(string nytAntal)
        {
            AntalSpillere = nytAntal;
        }

        public void OpdaterPris(int nyPris)
        {
            Pris = nyPris;
        }

        public void SætRequest(bool erRequest, string kontaktperson = "")
        {
            ErRequest = erRequest;
            Kontaktperson = kontaktperson;
        }

        public void OpdaterKontaktperson(string nyKontaktperson)
        {
            Kontaktperson = nyKontaktperson;
        }
         
        private static void SetLastId(int id)
        {
            if (id > lastId)
            {
                lastId = id;
            }
        }

        // Opretter et spil og giver automatisk ID hvis der ikke er et i forvejen
        public Spil(string titel, Genre genre, Stand stand, int pris, string antalSpillere, int id = 0, bool erReserveret = false, bool erRequest = false, string kontaktperson = "", string reserveretAf = "")
        {
            Titel = titel;
            Genre = genre;
            Stand = stand;
            Pris = pris;
            AntalSpillere = antalSpillere;
            ErReserveret = erReserveret;
            ErRequest = erRequest;
            Kontaktperson = kontaktperson;
            ReserveretAf = reserveretAf;

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
            return $"{Titel};{Genre};{Stand};{Pris};{AntalSpillere};{Id};{ErReserveret};{ErRequest};{Kontaktperson};{ReserveretAf}";
        }

        public static Spil FromString(string linje)
        {
            string[] data = linje.Split(';'); //Vi vælger semikolon da det er mindre sandsynligt at det vil være en del af felterne, og det gør parsing mere robust.

            string titel = data[0];
            Genre genre = Enum.Parse<Genre>(data[1]);
            Stand stand = Enum.Parse<Stand>(data[2]);
            int pris = int.Parse(data[3]);
            string antalSpillere = data[4];
            int id = int.Parse(data[5]);
            bool erReserveret = bool.Parse(data[6]);
            bool erRequest = bool.Parse(data[7]);
            string kontaktperson = data.Length > 8 ? data[8] : "";
            string reserveretAf = data.Length > 9 ? data[9] : "";

            return new Spil(titel, genre, stand, pris, antalSpillere, id, erReserveret, erRequest, kontaktperson, reserveretAf);
        }

        // Formaterer spillet til visning i konsol-tabel med faste kolonnebredder
        public string VisInfo()
        {
            string status = "";

            // Bygger status-tekst med reservation hvis relevant
            if (ErReserveret)
            {
                string navn = ReserveretAf.Length > 20 ? ReserveretAf.Substring(0, 17) + "..." : ReserveretAf;
                status += string.IsNullOrWhiteSpace(ReserveretAf)
                    ? "(RESERVERET)"
                    : $"(RES: {navn})";
            }

            // Tilføjer ønsket-status hvis relevant
            if (ErRequest)
            {
                string kontakt = Kontaktperson.Length > 20 ? Kontaktperson.Substring(0, 17) + "..." : Kontaktperson;
                status += string.IsNullOrWhiteSpace(Kontaktperson)
                    ? "(ØNSKET)"
                    : $"(ØNS: {kontakt})";
            }

            // Afkorter titel hvis den er for lang
            string titel = Titel.Length > 32 ? Titel.Substring(0, 29) + "..." : Titel;

            // Returnerer formateret string med faste kolonnebredder
            return $"{Id,-5}" +
                   $"{titel,-33}" +
                   $"{Genre,-15}" +
                   $"{AntalSpillere,-12}" +
                   $"{Stand,-15}" +
                   $"{Pris + " kr",8}" +
                   $"{status,32}";
        }

        // Indlæser alle spil fra tekstfil med fejlhåndtering
        public static List<Spil> LæsFraFil(string filnavn) {
            List<Spil> spilListe = new List<Spil>();

            try {
                if (File.Exists(filnavn)) {
                    using (StreamReader sr = new StreamReader(filnavn)) {
                        string? linje;
                        while ((linje = sr.ReadLine()) != null) {
                            if (!string.IsNullOrWhiteSpace(linje)) {
                                try {
                                    spilListe.Add(Spil.FromString(linje));
                                }
                                catch (Exception ex) {
                                    // Springer fejlagtige linjer over i stedet for at crashe
                                    Console.WriteLine($"Advarsel: Kunne ikke indlæse linje: {linje}");
                                    Console.WriteLine($"Fejl: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (IOException ex) {
                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                Console.WriteLine("Starter med tom liste.");
            }
            catch (UnauthorizedAccessException ex) {
                Console.WriteLine($"Ingen adgang til fil: {ex.Message}");
            }

            return spilListe;
        }
    }
}