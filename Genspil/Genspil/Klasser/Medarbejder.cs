using Genspil.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Timers;

namespace Genspil.Klasser
{
    public class Medarbejder
    {
        private static string filsti = "Datafiler/medarbejdere.txt";  // Sti til filen hvor medarbejderne gemmes
        // Static field til at holde styr på det sidste ID der blev tildelt
        private static int lastId = 0;
        public string Navn { get; set; }   // Navnet på medarbejderen
        public int Id { get; private set; }

        // Metode til at opdatere lastId når vi læser fra fil
        public static void SetLastId(int id)
        {
            if (id > 0)
            {
                lastId = id;
            }
        }
        public Medarbejder(int id, string navn)
        {
            Id = id;
            Navn = navn;
            SetLastId(id); // Opdaterer lastId hvis vi loader fra fil
        }

        public override string ToString()
        {
            // Laver objektet om til en tekstlinje med ; imellem
            return $"{Id};{Navn}";
        }

        // Bruges når vi læser fra fil og skal lave tekst om til et objekt
        public static Medarbejder FromString(string linje)
        {
            // Deler linjen op ved ;
            string[] data = linje.Split(';');

            // Henter værdierne fra arrayet
            int id = int.Parse(data[0]);
            string navn = data[1];

            // Returnerer et nyt Medarbejder-objekt med værdierne
            return new Medarbejder(id, navn);
        }

        public static void VisAlleMedarbejdere()
        {
            var medarbejderListe = SpilDataHandler.LæsMedarbejdereFraFil(filsti);

            Console.WriteLine($"Antal medarbejdere indlæst: {medarbejderListe.Count}");

            if (medarbejderListe != null && medarbejderListe.Count > 0)
            {
                Console.WriteLine("Medarbejdere:");
                foreach (var m in medarbejderListe)
                {
                    Console.WriteLine($"ID: {m.Id}, Navn: {m.Navn}");
                }
            }
            else
            {
                Console.WriteLine("Ingen medarbejdere fundet.");
            }
        }
    }
}
