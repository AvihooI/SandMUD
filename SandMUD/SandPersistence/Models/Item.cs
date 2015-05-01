using System;
using System.Collections.Generic;

namespace SandPersistence.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public List<Attribute> Attributes { get; set; }
    }
}