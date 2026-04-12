using Genspil.Klasser;
using Genspil.Data;

List<Spil> spilListe = new List<Spil>();

SpilDataHandler handler = new SpilDataHandler("Datafiler/spil.txt");

spilListe = handler.LoadFromFile();

foreach (var spil in spilListe)
{
    Console.WriteLine($"{spil.Navn} - {spil.Pris} kr");
}