namespace LetsDoIt.Moody.Application.User
{
    public class SystemUsersGetResult
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string UserType { get; set; }
    }
}