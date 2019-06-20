using BookStore.Contract;
using BookStore.Exceptions;
using BookStore.Model;
using System.Collections.Generic;
using System.Linq;

namespace BookStore
{
    public class BookStore : IStore
    {
        /// <summary>
        /// String object containing the JSON Schema of book stock
        /// </summary>
        public string CatalogSchema { get;}

        /// <summary>
        /// All books in book store accessed by book name
        /// </summary>
        /// <example>Books["Ayn Rand - FountainHead"] returns 
        /// a Book object whose name is "Ayn Rand - FountainHead"</example>
        public Dictionary<string, Book> Books { get; private set; }

        /// <summary>
        /// Dictionary of categories and it's promotions
        /// </summary>
        /// <example>Category["Fantastique"] returns 0.1</example>
        public Dictionary<string, double> Categories { get; private set; }

        /// <summary>
        /// Object to which will deserialize the JSON data of book stock
        /// </summary>
        private BookStock bookStock;

        #region Ctor
        public BookStore() { }

        /// <summary>
        /// Initializes a BookStore object with it's JSON schema
        /// </summary>
        /// <param name="catalogSchema">JSON schema definition of book stock</param>
        public BookStore(string catalogSchema) { CatalogSchema = catalogSchema; }
        #endregion

        #region IStore memebers
        /// <summary>
        /// Load book stock from a json string
        /// </summary>
        /// <param name="catallogAsJson">Book stock in json format</param>
        public void Import(string catallogAsJson)
        {
            if (!string.IsNullOrWhiteSpace(CatalogSchema))
            {
                BookStock.ValidateJsonFormat(catallogAsJson, CatalogSchema);
            }

            bookStock = BookStock.FromJson(catallogAsJson);
            Books = bookStock.Books.ToDictionary(b => b.Name, b => b);
            Categories = bookStock.Categories.ToDictionary(c => c.Name, c => c.Discount);
        }

        /// <summary>
        /// Returns the quantity of a book in the stock
        /// </summary>
        /// <param name="name">Book name</param>
        /// <returns>Quantity of a book</returns>
        public int Quantity(string name)
        {
            if (!Books.ContainsKey(name))
            {
                return 0;
            }

            return Books[name].Quantity;
        }

        /// <summary>
        /// Calculates total price of books in the basket
        /// </summary>
        /// <param name="basketByNames">books</param>
        /// <returns>Total price of books, throws NotEnoughInventoryException if book stock insufficient
        /// </returns>
        public double Buy(params string[] basketByNames)
        {
            ValidateBookQuantity(basketByNames);
            var total = TotalPrice(basketByNames);
            return total;
        }
        #endregion

        /// <summary>
        /// Calculates total price of books to buy
        /// </summary>
        /// <param name="basketByNames">Names of books </param>
        /// <returns>Total price of books</returns>
        private double TotalPrice(params string[] basketByNames)
        {
            if (basketByNames.Length == 1)
            {
                return Books[basketByNames[0]].Price;
            }

            var books = basketByNames.GroupBy(b => b).Select(g => new Book { Name = g.Key, Quantity = g.Count(), Price = Books[g.Key].Price, Category = Books[g.Key].Category });

            var total = books.GroupBy(b => b.Category).Sum(category => category.Sum(book =>
            {
                var price = book.Price * book.Quantity;
                if (category.Count() > 1)
                {
                    price -= book.Price * Categories[category.Key];
                }
                return price;
            }));

            return total;
        }

        /// <summary>
        /// Validate if quantity of book to buy exceeds the stock
        /// </summary>
        /// <param name="names">names of books</param>
        /// <exception cref="NotEnoughInventoryException">Thrown if stock is insufficient</exception>
        private void ValidateBookQuantity(params string[] names)
        {
            var bookNamesAndQuantities = names.GroupBy(n => n).Select(g => new NameQuantity { Name = g.Key, Quantity = g.Count() });

            var missing = bookNamesAndQuantities.Where(x => x.Quantity > Books[x.Name].Quantity);

            if (missing.Any())
            {
                throw new NotEnoughInventoryException(missing);
            }
        }
    }
}
