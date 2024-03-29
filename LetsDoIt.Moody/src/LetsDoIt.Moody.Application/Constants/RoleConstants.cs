﻿namespace LetsDoIt.Moody.Application.Constants
{
    public static class RoleConstants
    {
        public const string AdminRole = UserTypeConstants.Admin;
        public const string StandardRole = UserTypeConstants.Admin + "," + UserTypeConstants.Standard;
        public const string ClientRole = UserTypeConstants.Admin + "," + UserTypeConstants.Standard + "," + UserTypeConstants.Client;
        public const string NotActivatedUserRole = UserTypeConstants.NotActivatedUser;
        public const string ResetPasswordRole = UserTypeConstants.ResetPassword;
    }
}
