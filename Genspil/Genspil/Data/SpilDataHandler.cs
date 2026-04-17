using Genspil.Klasser; // Giver adgang til Spil-klassen


namespace Genspil.Data // Organiserer koden i en mappe/gruppe der hedder Data
{

    public static class SpilDataHandler // Klasse der står for at håndtere filer (læse og gemme spil)
    {
        // Metode til at læse spil fra en tekstfil
        public static List<Spil> LæsFraFil(string filnavn)
        {
            // Opretter en tom liste, hvor vi gemmer spillene
            List<Spil> spilListe = new List<Spil>();

            // Tjekker om filen overhovedet findes, så programmet ikke crasher
            if (File.Exists(filnavn))
            {
                // Åbner filen og sørger for at den bliver lukket igen automatisk bagefter
                using (StreamReader sr = new StreamReader(filnavn))
                {
                    string linje; // Variabel til at holde hver linje fra filen

                    // Læser linje for linje indtil der ikke er flere linjer (null betyder slut)
                    while ((linje = sr.ReadLine()) != null)
                    {
                        // Springer tomme linjer over (bare for at undgå fejl)
                        if (!string.IsNullOrWhiteSpace(linje))
                        {
                            // Laver linjen om til et Spil-objekt og tilføjer det til listen
                            spilListe.Add(Spil.FromString(linje));
                        }
                    }
                }
            }

            // Returnerer listen med alle spillene
            return spilListe;
        }

        // Metode til at gemme alle spil i en fil
        public static void GemTilFil(string filnavn, List<Spil> spilListe)
        {
            // Åbner filen til at skrive i (overskriver det gamle indhold)
            using (StreamWriter sw = new StreamWriter(filnavn))
            {
                // Går igennem hvert spil i listen
                foreach (Spil spil in spilListe)
                {
                    // Skriver spillet til filen som en tekstlinje
                    sw.WriteLine(spil.ToString());
                }
            }
        }
     
    }
}