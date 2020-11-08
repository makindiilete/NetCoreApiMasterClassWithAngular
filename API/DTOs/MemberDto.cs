using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; } //u can name ds anything but it is important u leave it as 'Id' so entity framework can recognise it as our auto incremented primary key
        public string UserName { get; set; } //also its advisable to stick to d pascal case here in other to avoid having to refactor later when u setup identity bcos identity uses 'UserName' and not 'Username'
        public string PhotoUrl { get; set; } //d url to our main photo
        public int Age { get; set; }
        public string KnownAs { get; set; } // wat the user want to b known as, which can be different from their username
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; } // we will use this to display female members to a male user and vice versa
        public string Introduction { get; set; } //User can enter an introductory text 4 their profile
        public string LookingFor { get; set; } // d kind of people dey want
        public string Interest { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; } // Instead of using the Photo Entity dt contains also d AppUser which is given us cross ref error, we use a PhotoDto where we specify only the items we want to return
    }
}
