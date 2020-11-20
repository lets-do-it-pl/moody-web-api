namespace LetsDoIt.Moody.Application.Constants
{
    public static class RoleConstants
    {
        public const string AdminRole = UserTypeConstants.Admin;
        public const string StandardRole = UserTypeConstants.Admin + "," + UserTypeConstants.Standard;
        public const string ClientRole = UserTypeConstants.Admin + "," + UserTypeConstants.Standard + "," + UserTypeConstants.Client;
    }
}
