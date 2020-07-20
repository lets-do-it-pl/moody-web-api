using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Application
{
    public  interface IUserService
    {
        void SaveUser(string userName, string password);
    }
}
