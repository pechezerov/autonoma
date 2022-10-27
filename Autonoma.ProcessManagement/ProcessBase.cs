using System.ComponentModel.DataAnnotations;

namespace Autonoma.ProcessManagement
{
    public class ProcessBase
    {
        [Key]
        public int Id { get; set; }

        public string ProcessName { get; set; }
        public string FileName { get; set; }
        public string FileVersion { get; set; }
        public string ProductName { get; set; }
    }
}