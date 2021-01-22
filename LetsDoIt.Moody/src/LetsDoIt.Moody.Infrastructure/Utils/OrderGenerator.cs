using System;
namespace LetsDoIt.Moody.Infrastructure.Utils
{
    public static class OrderGenerator
    {
        public static decimal GenerateOrder(decimal leastOrder)
        {
            return leastOrder - 1;
        }
    }
}
