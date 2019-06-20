using Newtonsoft.Json;

namespace BookStore.Model
{
    public class Book
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Category")]
        public string Category { get; set; }
        [JsonProperty("Price")]
        public double Price { get; set; }
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }
    }
}
