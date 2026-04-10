namespace Genspil.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Navn { get; set; }

        public override string ToString()
        {
            return $"{Id},{Navn}";
        }

        public static Genre FromString(string data)
        {
            string[] parts = data.Split(',');

            return new Genre
            {
                Id = int.Parse(parts[0]),
                Navn = parts[1]
            };
        }
    }
}