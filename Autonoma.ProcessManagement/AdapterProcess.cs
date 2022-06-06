using Autonoma.Core;
using Autonoma.Domain.Entities;
using System;

namespace Autonoma.ProcessManagement
{
    public class AdapterProcess : ProcessBase
    {
        public AdapterConfiguration Configuration { get; set; }
        public DateTime StartedAt { get; set; }
        public HealthState Health { get; set; }
    }
}