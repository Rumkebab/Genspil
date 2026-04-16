namespace Genspil.Klasser
{
    public static class ConsoleHelper
    {
        // Laver en pause så brugeren kan læse beskeden
        public static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Tryk på en tast for at fortsætte...");
            Console.ReadKey();
        }

        // Venter indtil brugeren trykker A
        public static void VentPåA(string promptTekst = "Tryk A for at vende tilbage: ")
        {
            while (true)
            {
                Console.Write(promptTekst);
                char input = char.ToUpper(Console.ReadKey().KeyChar);

                if (input == 'A')
                {
                    return;
                }
            }
        }
    }
}