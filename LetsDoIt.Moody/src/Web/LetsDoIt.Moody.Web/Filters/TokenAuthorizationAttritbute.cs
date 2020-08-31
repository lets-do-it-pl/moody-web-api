using System;
using System.Security;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TokenAuthorizationAttritbute : Attribute, IFilterMetadata
    {

    }
}
