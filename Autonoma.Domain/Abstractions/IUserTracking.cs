using Autonoma.Domain.Entities;
using System;

namespace Autonoma.Domain.Abstractions
{
    public interface IUserTracking
    {
        User CreatedBy { get; set; }
        int? CreatedById { get; set; }
        User ModifiedBy { get; set; }
        int? ModifiedById { get; set; }
    }
}
