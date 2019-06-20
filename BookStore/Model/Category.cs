using Newtonsoft.Json;

namespace BookStore.Model
{
    public class Category
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Discount")]
        public double Discount { get; set; }
    }
}
