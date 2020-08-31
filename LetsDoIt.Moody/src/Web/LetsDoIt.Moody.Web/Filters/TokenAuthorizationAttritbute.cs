using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LetsDoIt.Moody.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TokenAuthorizationAttritbute : Attribute, IFilterMetadata
    {

    }
}
