using System.Collections.Generic;

namespace SandPersistence
{
    public class Account
    {
        public int AccountID { get; set; }
        public string AccountPassword { get; set; }
        public List<PlayerCharacter> Characters { get; set; }
        public string AccountSettingsJson { get; set; }
    }
}