using System.Collections.Generic;

namespace SandModel
{
    //DONE
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PreferencesJSon { get; set; }
        public virtual ICollection<PlayerCharacter> PlayerCharacters { get; set; }
    }
}