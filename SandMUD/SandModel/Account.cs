using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SandModel
{
    //DONE
    public class Account : IValidatableObject
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PreferencesJSon { get; set; }
        public virtual ICollection<PlayerCharacter> PlayerCharacters { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Char.IsLetter(Username.First()))
                yield return new ValidationResult("Username needs to begin with a letter");

            if (!Username.All(Char.IsLetterOrDigit))
                yield return new ValidationResult("Username needs to be alphanumeric");

            if ((Username.Length < 3) || (Username.Length > 15))
                yield return new ValidationResult("Username needs to be between 3 to 15 characters long");
            
        }
    }
}