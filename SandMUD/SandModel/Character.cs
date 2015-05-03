using System.Collections.Generic;

namespace SandModel
{
    //DONE
    public abstract class Character
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StatsJson { get; set; }
        public virtual ICollection<InventoryItem> Inventory { get; set; }
    }
}