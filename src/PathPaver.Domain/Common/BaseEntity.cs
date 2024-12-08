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
    protected static long Id { get; set; }
    protected DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
    protected bool IsVisible { get; set; } = false;
}