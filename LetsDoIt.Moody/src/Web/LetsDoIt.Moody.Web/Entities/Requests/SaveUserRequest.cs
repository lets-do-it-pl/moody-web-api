namespace LetsDoIt.Moody.Web.Entities.Requests
{
    using Infrastructure.ValueTypes;

    public class SaveUserRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public Email Email { get; set; }
    }
}
