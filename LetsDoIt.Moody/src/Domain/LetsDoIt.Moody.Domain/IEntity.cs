using System;

namespace LetsDoIt.Moody.Domain
{
    public interface IEntity
    {
        DateTime CreateDate { get; set; }

        DateTime? ModifiedDate { get; set; }

        bool IsDeleted { get; set; }
    }
}