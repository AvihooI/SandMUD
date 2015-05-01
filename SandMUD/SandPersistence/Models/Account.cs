using System.Collections.Generic;

namespace SandPersistence.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountPassword { get; set; }
        public List<PlayerCharacter> Characters { get; set; }
        public string AccountSettingsJson { get; set; }
    }
}