using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.ProcessManagement.Abstractions
{
    public interface IProcessManager
    {
        Task<IEnumerable<ProcessBase>> GetProcessList();
        Task Start();
        Task Stop();
    }
}