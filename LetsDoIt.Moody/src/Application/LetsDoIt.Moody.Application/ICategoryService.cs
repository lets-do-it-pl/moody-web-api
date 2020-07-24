
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Application
{
    public interface ICategoryService
    {
        void Update(int id, string name, int order, byte[] image);
        void getCategory(string versionNumber);
    }
}
