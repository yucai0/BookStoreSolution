using BookStore.Exceptions;
using BookStore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BookStore.Tests
{
    [TestClass()]
    public class BookStoreTests
    {
        [DataRow("Non existing book", 0)]
        [DataRow("Ayn Rand - FountainHead", 10)]
        [DataRow("J.K Rowling - Goblet Of fire", 2)]
        [DataRow("Isaac Asimov - Foundation", 1)]
        [DataRow("Robin Hobb - Assassin Apprentice", 8)]
        [DataTestMethod]
        public void Test_Get_Book_Quantity_By_Name(string bookname, int expected) =>
            Assert.AreEqual(expected, bookStore.Quantity(bookname));

        [DataTestMethod]
        [DynamicData(nameof(BooksToBuy), DynamicDataSourceType.Property)]
        public void Test_Calculate_Book_Price(string[] books, double expected) =>
            Assert.AreEqual(expected, bookStore.Buy(books));

        [DataTestMethod]
        [DynamicData(nameof(BooksToBuy1), DynamicDataSourceType.Property)]
        public void Should_Throw_Exception_If_Stock_Insufficient(string[] books) =>
            Assert.ThrowsException<NotEnoughInventoryException>(() => bookStore.Buy(books));

        [TestMethod]
        public void Should_Throw_Exception_If_Json_Format_Invalid() =>
            Assert.ThrowsException<InvalidJsonFormatException>(() => BookStock.ValidateJsonFormat(InvalidCatalog, catalogSchema));

        /// <summary>
        /// Example books to buy of which the quantity exceeds the stock
        /// </summary>
        public static IEnumerable<object[]> BooksToBuy1
        {
            get
            {
                yield return new object[] { new string[] {
                    "Isaac Asimov - Foundation",
                    "Isaac Asimov - Foundation"} };
            }
        }

        /// <summary>
        /// Example books to buy
        /// </summary>
        public static IEnumerable<object[]> BooksToBuy
        {
            get
            {
                yield return new object[] { new string[] { "Isaac Asimov - Foundation" }, 16 };

                yield return new object[] { new string[] {
                    "Ayn Rand - FountainHead", // 12, Philosophy
                    "Isaac Asimov - Robot series", //5, Science fiction
                    "Robin Hobb - Assassin Apprentice", //12, Fantastique
                    "Robin Hobb - Assassin Apprentice",
                    "J.K Rowling - Goblet Of fire"}, //8, Fantastque
                    47 //total price : 12 + 5+ 12*0.9 + 12 + 8*0.9
                };

                yield return new object[] { new string[] {
                    "Ayn Rand - FountainHead", // 12, Philosophy
                    "Isaac Asimov - Robot series", //5, Science fiction
                    "Isaac Asimov - Foundation", //16, Science fiction
                    "Robin Hobb - Assassin Apprentice", //12, Fantastique
                    "J.K Rowling - Goblet Of fire", //8, Fantastque
                    "Robin Hobb - Assassin Apprentice",
                    "J.K Rowling - Goblet Of fire"},
                    69.95 // total price : 12 + 5 * 0.95 + 16 * 0.95 + 8 *0.9 + 8 + 12 * 0.9 + 12
                };
            }
        }

        static BookStore bookStore;

        const string catalogAsjson = "{ \"Category\":[ { \"Name\": \"Science Fiction\", \"Discount\": 0.05 }, " +
            "{ \"Name\": \"Fantastique\", \"Discount\": 0.1 }, { \"Name\": \"Philosophy\", \"Discount\": 0.15 }, ], " +
            "\"Catalog\": [ { \"Name\": \"J.K Rowling - Goblet Of fire\", \"Category\": \"Fantastique\", \"Price\": 8, \"Quantity\": 2 }, " +
            "{ \"Name\": \"Ayn Rand - FountainHead\", \"Category\": \"Philosophy\", \"Price\": 12, \"Quantity\": 10 }, " +
            "{ \"Name\": \"Isaac Asimov - Foundation\", \"Category\": \"Science Fiction\", \"Price\": 16, \"Quantity\": 1 }, " +
            "{ \"Name\": \"Isaac Asimov - Robot series\", \"Category\": \"Science Fiction\", \"Price\": 5, \"Quantity\": 1 }, " +
            "{ \"Name\": \"Robin Hobb - Assassin Apprentice\", \"Category\": \"Fantastique\", \"Price\": 12, \"Quantity\": 8 } ], }";

        const string InvalidCatalog = "{ \"Category\":[ { \"Name\": \"Science Fiction\", \"Discount\": 0.05 }, " +
            "{ \"Name\": \"Fantastique\", \"Discount\": 0.1 }, { \"Name\": \"Philosophy\", \"Discount\": 0.15 }]}";

        const string catalogSchema = "{ \"type\": \"object\", \"title\": \"The Root Schema\", \"required\": [ \"Category\", \"Catalog\" ]," +
            " \"properties\": { \"Category\": { \"type\": \"array\", \"title\": \"List of existing category with associated discount\", " +
            "\"items\": { \"type\": \"object\", \"title\": \"one category with its discount\"," +
            " \"required\": [ \"Name\", \"Discount\" ], \"properties\": " +
            "{ \"Name\": { \"type\": \"string\", \"title\": \"The unique name of the category, it is a functionnal key\", " +
            "\"default\": \"\", \"examples\": [ \"Fantastique\" ], \"pattern\": \"^(.+)$\" }, " +
            "\"Discount\": { \"type\": \"number\", \"title\": \"the discount applies when buying multiple book of this category\", \"default\": 0.0, " +
            "\"examples\": [ 0.05 ] } } } }, \"Catalog\": { \"type\": \"array\", \"title\": \"The Catalog of the store\", " +
            "\"items\": { \"type\": \"object\", \"title\": \"a book in the catalog\", \"required\": " +
            "[ \"Name\", \"Category\", \"Price\", \"Quantity\" ], \"properties\": {\r\n\"Name\": { \"type\": \"string\", " +
            "\"title\": \"The unique Name of the book, it is a functionnal key\", \"default\": \"\", " +
            "\"examples\": [ \"J.K Rowling - Goblet Of fire\" ], \"pattern\": \"^(.+)$\" }, \"Category\": { \"type\": \"string\", " +
            "\"title\": \"The name of one the category existing in the Category root properties.\", \"default\": \"\", " +
            "\"examples\": [ \"Fantastique\" ], \"pattern\": \"^(.+)$\" }, \"Price\": { \"type\": \"number\", " +
            "\"title\": \"the price of an copy of the book\", \"default\": 0, \"examples\": [ 8 ] }, " +
            "\"Quantity\": { \"type\": \"integer\", \"title\": \"The Quantity of copy of the book in the catalog.\", \"default\": 0, \"examples\": [ 2 ] } } } } } }";

        [ClassInitialize]
        public static void InitTest(TestContext context)
        {
            bookStore = new BookStore(catalogSchema);
            bookStore.Import(catalogAsjson);
        }
    }
}