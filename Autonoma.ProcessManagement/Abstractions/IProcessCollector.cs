using System.Collections.Generic;

namespace Autonoma.ProcessManagement.Abstractions
{
    public interface IProcessCollector
    {
        List<AdapterProcess> GetProcesses();
    }
}