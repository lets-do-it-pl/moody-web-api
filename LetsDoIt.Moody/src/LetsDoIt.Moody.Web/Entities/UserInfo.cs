using System.Security.AccessControl;
using LetsDoIt.Moody.Application.Constants;

namespace LetsDoIt.Moody.Web.Entities
{
    public class UserInfo
    {
        public UserInfo(
            string userId, 
            string fullName,
            string userType = null)
        {
            FullName = fullName;
            UserId = int.Parse(userId);
            UserType = userType;
        }

        public int UserId { get; }

        public string FullName { get; }

        public string UserType { get; set; }
    }
}
