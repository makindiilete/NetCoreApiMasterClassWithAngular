namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; } //u can name ds anything but it is important u leave it as 'Id' so entity framework can recognise it as our auto incremented primary key
        public string UserName { get; set; } //also its advisable to stick to d pascal case here in other to avoid having to refactor later when u setup identity bcos identity uses 'UserName' and not 'Username'
    }
}
