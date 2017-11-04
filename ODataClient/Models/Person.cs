﻿using System.Collections.Generic;

namespace ODataClient.Models
{
    public class Person
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Trip> Trips { get; set; }
    }
}
