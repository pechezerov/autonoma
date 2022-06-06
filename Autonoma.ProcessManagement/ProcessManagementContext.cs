using Microsoft.EntityFrameworkCore;

namespace Autonoma.ProcessManagement
{
    public class ProcessManagementContext : DbContext
    {
        public ProcessManagementContext(DbContextOptions<ProcessManagementContext> options)
            : base(options)
        { }

        public DbSet<ProcessBase> Processes { get; set; }
    }
}
