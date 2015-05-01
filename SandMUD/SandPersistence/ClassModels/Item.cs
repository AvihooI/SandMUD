using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandPersistence
{
    public class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public List<Attribute> Attributes { get; set; }
    }
}
