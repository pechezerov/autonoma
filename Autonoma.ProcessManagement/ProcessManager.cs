using Autonoma.Configuration;
using Autonoma.ProcessManagement.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace Autonoma.ProcessManagement
{
    public class ProcessManager : IProcessManager
    {
        private readonly IProcessCollector _collector;
        private readonly DbContextOptions<ProcessManagementContext> _procContextOptions;
        private readonly DbContextOptions<ConfigurationContext> _cfgContextOptions;
        private readonly Timer _timer;

        public ProcessManager(IProcessCollector collector, IConfiguration configuration)
        {
            _collector = collector;
            _procContextOptions = new DbContextOptionsBuilder<ProcessManagementContext>()
               .UseInMemoryDatabase(databaseName: $"{nameof(Autonoma)}{nameof(Autonoma.ProcessManagement)}")
               .Options;

            string cs = configuration.GetConnectionString("ConfigurationDatabase");
            _cfgContextOptions = new DbContextOptionsBuilder<ConfigurationContext>()
               .UseSqlite(cs)
               .Options;

            _timer = new Timer(5000);
            _timer.Elapsed += Scan;
            _timer.Start();
        }

        public async Task<IEnumerable<ProcessBase>> GetProcessList()
        {
            using (var context = new ProcessManagementContext(_procContextOptions))
            {
                return await context.Processes
                    .ToListAsync();
            }
        }

        public async Task Start()
        {
            try
            {
                using (var context = new ProcessManagementContext(_procContextOptions))
                {
                   var adapters = await context.Processes
                        .ToListAsync();

                    foreach (var adapter in adapters)
                    {
                        if (adapter is AdapterProcess)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Stop()
        {
            try
            {
                using (var context = new ProcessManagementContext(_procContextOptions))
                {
                    var adapters = await context.Processes
                        .ToListAsync();

                    foreach (var adapter in adapters)
                    {

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Scan(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;

            try
            {
                using (var context = new ProcessManagementContext(_procContextOptions))
                {
                    var relevantProcesses = _collector.GetProcesses();

                    context.Processes.RemoveRange(context.Processes);
                    context.Processes.AddRange(relevantProcesses);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _timer.Enabled = true;
            }
        }
    }
}
