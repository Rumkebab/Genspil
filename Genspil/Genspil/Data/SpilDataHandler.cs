using Genspil.Klasser;

namespace Genspil.Data
{
    public class SpilDataHandler
    {
        // Læser alle spil fra tekstfilen
        public static List<Spil> LæsFraFil(string filnavn)
        {
            List<Spil> spilListe = new List<Spil>();

            // Tjekker om filen findes, før vi prøver at læse den
            if (File.Exists(filnavn))
            {
                using (StreamReader sr = new StreamReader(filnavn))
                {
                    string? linje; // ? betyder at variablen godt må være null

                    while ((linje = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(linje))
                        {
                            spilListe.Add(Spil.FromString(linje));
                        }
                    }
                }
            }

            return spilListe;
        }

        // Gemmer alle spil i filen
        public static void GemTilFil(string filnavn, List<Spil> spilListe)
        {
            using (StreamWriter sw = new StreamWriter(filnavn))
            {
                foreach (Spil spil in spilListe)
                {
                    sw.WriteLine(spil.ToString());
                }
            }
        }
    }
}