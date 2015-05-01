using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandPersistence
{
    public interface ICharacter
    {
        int CharacterID { get; set; }

        string CharacterName { get; set; }

        List<Item> Inventory { get; set; }

        List<Attribute> Attributes { get; set; }
    }


    public class Npc : ICharacter
    {
        public int CharacterID { get; set; }

        public string CharacterName { get; set; }

        public List<Item> Inventory { get; set; }

        public List<Attribute> Attributes { get; set; }
        string NpcScriptName { get; set; }
    }

    public class PlayerCharacter : ICharacter
    {
        public int CharacterID { get; set; }

        public string CharacterName { get; set; }

        public List<Item> Inventory { get; set; }

        public List<Attribute> Attributes { get; set; }
        public int AccountID { get; set; }
    }

}
