namespace ODataClient.Models
{
    public class ODataSearchParameters
    {
        public ODataFilter Filter { get; set; }
        public int? Top { get; set; }
    }

    public class ODataFilter
    {
        public string Query { get; set; }
    }
}
