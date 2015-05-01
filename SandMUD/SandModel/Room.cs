using System;
using System.Collections.Generic;

namespace SandModel
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Description { get; set; }
        public ICollection<Item> Storage { get; set; }
    }
}