namespace LetsDoIt.Moody.Web.Entities
{
    public class UserInfo
    {
        public UserInfo(
            string userId, 
            string fullName)
        {
            FullName = fullName;
            UserId = int.Parse(userId);
        }

        public int UserId { get; }

        public string FullName { get; }
    }
}
