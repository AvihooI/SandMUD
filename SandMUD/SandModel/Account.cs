using System.Collections.Generic;

namespace SandModel
{
    public class Account
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public List<PlayerCharacter> PlayerCharacters { get; set; }
    }
}