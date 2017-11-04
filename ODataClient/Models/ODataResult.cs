using Newtonsoft.Json;
using System.Collections.Generic;

namespace ODataClient.Models
{
    public class ODataResult<TModel> where TModel : class
    {
        [JsonProperty("value")]
        public IList<TModel> Documents { get; set; }
    }
}
