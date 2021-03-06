using System;
using System.Collections.Generic;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; } //u can name ds anything but it is important u leave it as 'Id' so entity framework can recognise it as our auto incremented primary key
        public string UserName { get; set; } //also its advisable to stick to d pascal case here in other to avoid having to refactor later when u setup identity bcos identity uses 'UserName' and not 'Username'
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; } // wat the user want to b known as, which can be different from their username
        public DateTime Created { get; set; } = DateTime.Now; // d time their acct was created. We giv it an initial value of the current date
        public DateTime LastActive { get; set; } = DateTime.Now; // d last time a user was active in our app
        public string Gender { get; set; } // we will use this to display female members to a male user and vice versa
        public string Introduction { get; set; } //User can enter an introductory text 4 their profile
        public string LookingFor { get; set; } // d kind of people dey want
        public string Interest { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        /*//An extension method to calculate Age base on date of birth
        public int GetAge()
        {
            // Now we can call our own defined extension of the DateTime class to return d age of the user
            return DateOfBirth.CalculateAge();
        }*/

    }
}
