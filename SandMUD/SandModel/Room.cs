using System.Collections.Generic;

namespace SandModel
{
    //DONE
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExitsJson { get; set; }
        public string OtherJson { get; set; }
        public ICollection<InventoryItem> Inventory { get; set; }
    }
}