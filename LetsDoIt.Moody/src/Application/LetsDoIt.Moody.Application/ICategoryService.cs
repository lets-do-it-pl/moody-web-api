
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Application
{
    using Domain;
    public interface ICategoryService
    {
        void Update(int id, string name, int order, byte[] image);
        public CategoryEntity getCategory(string versionNumber);
    }
}
