namespace Genspil.Models
{
    public class Stand
    {
        public int Id { get; set; }
        public string Navn { get; set; }

        public override string ToString()
        {
            return $"{Id},{Navn}";
        }

        public static Stand FromString(string data)
        {
            string[] parts = data.Split(',');

            return new Stand
            {
                Id = int.Parse(parts[0]),
                Navn = parts[1]
            };
        }
    }
}