using Autonoma.Domain.Entities;
using System.Diagnostics;

namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterItem
    {
        public AdapterConfiguration Configuration { get; set; }

        public FileVersionInfo ProcessModuleFileInfo { get; set; }

        public string ProcessModuleFilePath { get; set; }
    }
}
