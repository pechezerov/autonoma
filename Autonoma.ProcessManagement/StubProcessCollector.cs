using Autonoma.ProcessManagement.Abstractions;
using System;
using System.Collections.Generic;

namespace Autonoma.ProcessManagement
{
    public class StubProcessCollector : IProcessCollector
    {
        public List<AdapterProcess> GetProcesses()
        {
            var result = new List<AdapterProcess>();
            result.Add(new AdapterProcess
            {
                Id = 113,
                ProcessName = "IECServer.dll",
                ProductName = "IEC 60870-5-104 Server",
                FileVersion = "2.1",
                StartedAt = DateTime.Now,
            });
            result.Add(new AdapterProcess
            {
                Id = 56988,
                ProcessName = "ModbusClient.dll",
                ProductName = "ModbusTCP Client",
                FileVersion = "3.1",
                StartedAt = DateTime.Now,
            });

            return result;
        }
    }
}
