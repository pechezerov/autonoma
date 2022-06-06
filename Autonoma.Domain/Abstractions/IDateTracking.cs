using System;

namespace Autonoma.Domain.Abstractions
{
    public interface IDateTracking
    {
        DateTime? CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
