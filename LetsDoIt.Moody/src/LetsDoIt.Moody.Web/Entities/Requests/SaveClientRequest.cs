namespace LetsDoIt.Moody.Web.Entities.Requests
{
<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Entities/Requests/SaveUserRequest.cs
    using Infrastructure.ValueTypes;

    public class SaveUserRequest
========
    public class SaveClientRequest
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Entities/Requests/SaveClientRequest.cs
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }
    }
}
