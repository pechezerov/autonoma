using Autonoma.Domain.Abstractions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autonoma.Domain.Entities
{
    public abstract class Entity : IDateTracking, IUserTracking, IExternalData
    {
        protected Entity()
        {
            Id = 0;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ExternalId { get; set; }
        public User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
        public User? ModifiedBy { get; set; }
        public int? ModifiedById { get; set; }
    }
}