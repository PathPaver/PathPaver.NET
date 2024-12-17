using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathPaver.Domain.Common;

/**
 * Every entity should inherit this class 
 */
public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
    public bool IsVisible { get; set; } = true;
}