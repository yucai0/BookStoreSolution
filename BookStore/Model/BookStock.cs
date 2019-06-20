using BookStore.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;

namespace BookStore.Model
{
    /// <summary>
    /// Class used to serialize and deserialize the JSON data containing the book stock
    /// </summary>
    public class BookStock
    {
        [JsonProperty("Catalog")]
        public Book[] Books { get; set; }

        [JsonProperty("Category")]
        public Category[] Categories { get; set; }

        /// <summary>
        /// Deserializes the input json to a BookStock object
        /// </summary>
        /// <param name="catalogAsJson">String object containing the JSON of book stock</param>
        /// <returns>An BookStock object</returns>
        public static BookStock FromJson(string catalogAsJson) =>
            JsonConvert.DeserializeObject<BookStock>(catalogAsJson);

        /// <summary>
        /// Valdates if json string conforms to the given schema definition
        /// </summary>
        /// <param name="jsonToValidate">json to validate</param>
        /// <param name="jsonSchema">json schema</param>
        /// <exception cref="InvalidJsonFormatException">Thrown if input json doesn't conform to the shcema</exception>
        public static void ValidateJsonFormat(string jsonToValidate, string jsonSchema)
        {
            JSchema schema = JSchema.Parse(jsonSchema);
            JObject json = JObject.Parse(jsonToValidate);

            if (!json.IsValid(schema, out IList<ValidationError> errors))
            {
                throw new InvalidJsonFormatException(errors);
            }
        }
    }
}
