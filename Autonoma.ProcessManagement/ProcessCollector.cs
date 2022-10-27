using Autonoma.ProcessManagement.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Autonoma.ProcessManagement
{
    public class ProcessCollector : IProcessCollector
    {
        public List<AdapterProcess> GetProcesses()
        {
            return Process.GetProcesses()
                .Select(p => new AdapterProcess 
                { 
                    Id = p.Id,
                    ProcessName = p.ProcessName
                })
                .ToList();
        }
    }
}
