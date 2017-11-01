using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiODataService.Models
{
    public class Trip
    {
        [Key]
        public String ID { get; set; }
        [Required]
        public String Name { get; set; }
    }
}