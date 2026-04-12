namespace Genspil.Klasser
{
    public class Spil
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public double Pris { get; set; }
        public string GenreIds { get; set; }
        public int StandId { get; set; }
        public string AntalSpillere { get; set; }
        public bool ErAvailable { get; set; }
        public int AntalAvailable { get; set; }

        public override string ToString()
        {
            return $"{Id};{Navn};{Pris};{GenreIds};{StandId};{AntalSpillere};{ErAvailable};{AntalAvailable}";
        }

        public static Spil FromString(string data)
        {
            string[] parts = data.Split(';');

            return new Spil
            {
                Id = int.Parse(parts[0]),
                Navn = parts[1],
                Pris = double.Parse(parts[2]),
                GenreIds = parts[3],
                StandId = int.Parse(parts[4]),
                AntalSpillere = parts[5],
                ErAvailable = bool.Parse(parts[6]),
                AntalAvailable = int.Parse(parts[7])
            };
        }
    }
}