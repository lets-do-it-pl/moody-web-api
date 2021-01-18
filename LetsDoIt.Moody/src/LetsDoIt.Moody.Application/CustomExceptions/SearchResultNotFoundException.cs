using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Application.CustomExceptions
{
    class SearchResultNotFoundException : Exception
    {
        public SearchResultNotFoundException(string searchKey)
            : base($"Not a single data contains \"{searchKey}\"")
        {
        }
    }
}
