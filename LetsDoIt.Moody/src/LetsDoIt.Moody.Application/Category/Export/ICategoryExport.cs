using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Category.Export
{
    public interface ICategoryExport
    {
        Task ExportAsync();
    }
}