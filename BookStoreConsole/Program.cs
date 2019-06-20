using BookStore.Contract;
using System;
using System.Collections.Generic;
using System.IO;

namespace BookStoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Modify the file path to load json files
            var pathCatalog = @"C:\Code\BookStoreSolution\BookStore\Json\Catalog.json";
            var pathSchema = @"C:\Code\BookStoreSolution\BookStore\Json\CatalogSchema.json";
            var catalogAsJson = File.ReadAllText(pathCatalog);
            var schema = File.ReadAllText(pathSchema);

            //Load json in memory
            IStore bookStore = new BookStore.BookStore(schema);
            bookStore.Import(catalogAsJson);


            //Test quanity
            Console.WriteLine("\nQuantity of following books: ");
            var books = new List<string>
            {
                "Ayn Rand - FountainHead",
                "J.K Rowling - Goblet Of fire",
                "Isaac Asimov - Foundation",
                "Robin Hobb - Assassin Apprentice"
            };
            Console.WriteLine("--------------------------------");
            books.ForEach(book => Console.WriteLine($"{book} : {bookStore.Quantity(book)}"));

            Console.WriteLine("\nBooks to buy :");
            Console.WriteLine("--------------------------------");
            var booksToBuy = new List<string>
            {
                "Ayn Rand - FountainHead", // 12, Philosophy
                "Isaac Asimov - Robot series", //5, Science fiction
                "Isaac Asimov - Foundation", //16, Science fiction
                "Robin Hobb - Assassin Apprentice", //12, Fantastique
                "J.K Rowling - Goblet Of fire", //8, Fantastque
                "Robin Hobb - Assassin Apprentice",
                "J.K Rowling - Goblet Of fire"
            };
            booksToBuy.ForEach(b=>Console.WriteLine(b));
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Total Price : {bookStore.Buy(booksToBuy.ToArray())}");

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
