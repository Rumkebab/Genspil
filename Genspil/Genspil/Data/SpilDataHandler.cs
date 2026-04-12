using System.Collections.Generic;
using System.IO;
using Genspil.Klasser;

namespace Genspil.Data
{
    public class SpilDataHandler
    {
        public string FilePath { get; set; }

        public SpilDataHandler(string filePath)
        {
            FilePath = filePath;
        }

        public void SaveToFile(List<Spil> spilListe)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (var spil in spilListe)
                {
                    sw.WriteLine(spil.ToString());
                }
            }
        }

        public List<Spil> LoadFromFile()
        {
            List<Spil> spilListe = new List<Spil>();

            if (!File.Exists(FilePath))
                return spilListe;

            using (StreamReader sr = new StreamReader(FilePath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        spilListe.Add(Spil.FromString(line));
                    }
                }
            }

            return spilListe;
        }
    }
}