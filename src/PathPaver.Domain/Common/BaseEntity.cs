using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace PathPaver.Domain.Common;

/**
 * Every entity should inherit this class 
 */
public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ObjectId Id { get; set; }
    public DateTime? DateCreated { get; set; } = DateTime.Now;
    public bool? IsVisible { get; set; } = true;
}