using System.Collections.Generic;

namespace ODataClient.Models
{
    public class ODataRequest<TModel>
        where TModel : class
    {
        public ICollection<TModel> data { get; set; }
    }
}
