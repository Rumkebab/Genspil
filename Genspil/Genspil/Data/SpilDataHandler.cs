using Genspil.Klasser;

namespace Genspil.Data
{
    // Håndterer persistering af spil til og fra tekstfil
    public static class SpilDataHandler
    {
        // Læser alle spil fra tekstfil og returnerer dem som en liste
        public static List<Spil> LæsFraFil(string filnavn)
        {
            List<Spil> spilListe = new List<Spil>();

            // Tjekker om filen eksisterer før vi forsøger at læse
            if (File.Exists(filnavn))
            {
                // Using sikrer at filen lukkes korrekt, selv ved fejl
                using (StreamReader sr = new StreamReader(filnavn))
                {
                    string linje;

                    // Læser fil linje for linje indtil slutningen (null)
                    while ((linje = sr.ReadLine()) != null)
                    {
                        // Springer blanke linjer over
                        if (!string.IsNullOrWhiteSpace(linje))
                        {
                            // Konverterer tekstlinje til Spil-objekt
                            spilListe.Add(Spil.FromString(linje));
                        }
                    }
                }
            }

            return spilListe;
        }

        // Gemmer hele spillisten til fil (overskriver eksisterende indhold)
        public static void GemTilFil(string filnavn, List<Spil> spilListe)
        {
            using (StreamWriter sw = new StreamWriter(filnavn))
            {
                // Skriver hvert spil som en separat linje
                foreach (Spil spil in spilListe)
                {
                    sw.WriteLine(spil.ToString());
                }
            }
        }

    }
}