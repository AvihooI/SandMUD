using System;
using System.Collections.Generic;

namespace SandPersistence.Models
{
    public interface ICharacter
    {
        int CharacterId { get; set; }
        string CharacterName { get; set; }
        List<Item> Inventory { get; set; }
        List<Attribute> Attributes { get; set; }
    }


    public class Npc : ICharacter
    {
        private string NpcScriptName { get; set; }
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        public List<Item> Inventory { get; set; }
        public List<Attribute> Attributes { get; set; }
    }

    public class PlayerCharacter : ICharacter
    {
        public int AccountId { get; set; }
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        public List<Item> Inventory { get; set; }
        public List<Attribute> Attributes { get; set; }
    }
}