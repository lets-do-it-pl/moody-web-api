namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class SaveClientRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
