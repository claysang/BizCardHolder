using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizCard.Core.Models
{
    public abstract class Entity
    {
        public static readonly DateTime EntityInitialDate = DateTime.Now;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedAtUtc { get; set; } = EntityInitialDate;

        public DateTime ModifiedAtUtc { get; set; } = EntityInitialDate;

        public DateTime VerifiedTime => ModifiedAtUtc > CreatedAtUtc ? ModifiedAtUtc : CreatedAtUtc;
    }
}