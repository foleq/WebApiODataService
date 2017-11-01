using System.ComponentModel.DataAnnotations;

namespace WebApiODataService.Models
{
    public class Trip
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}