using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(Creator))]
    public string CreatedById { get; set; } = default!;
    public User Creator { get; set; } = default!;

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(Updater))]
    public string? UpdatedById { get; set; } = default!;
    public User? Updater { get; set; } = default!;
}
