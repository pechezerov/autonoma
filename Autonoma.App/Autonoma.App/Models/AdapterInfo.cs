using Autonoma.Domain;
using System;

namespace Autonoma.App.Models
{
    public class AdapterInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string IpAddress { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public AdapterType Type { get; set; }

        public AdapterStateInfo State { get; set; }
    }

    public class AdapterStateInfo
    {
        public int Id { get; set; }
        public WorkState State { get; set; }
    }
}