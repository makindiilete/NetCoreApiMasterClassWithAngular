using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    //ds table will be named 'Photos' in the database.. we did not do ds for AppUser bcos we created a dbcontext where we setup ds name inside its DbSet but bcos we are not setting up a DbSet for Photos... we can use annotations to achieve d same thing..
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; } // we will need ds for the photo storage solution we are going to use
        public AppUser Type { get; set; } // owner of the particular photo : - ds will not be created as a column in the Photos table.. its only here to fully defined the relationship
        public int AppUserId { get; set; } // id (from AppUser table) of the owner of the photo : - ds will be created in the Photos table as the ForeignKey
    }
}
