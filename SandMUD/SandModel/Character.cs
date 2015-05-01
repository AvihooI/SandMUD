using System.Collections.Generic;

namespace SandModel
{
    public abstract class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Item> Inventory { get; set; }
    }
}