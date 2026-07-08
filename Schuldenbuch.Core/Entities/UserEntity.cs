namespace Schuldenbuch.Core.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        

        public ICollection<PersonEntity> Persons { get; set; } = new List<PersonEntity>();
    }
}