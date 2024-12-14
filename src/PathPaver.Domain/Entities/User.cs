using System.ComponentModel.DataAnnotations;
using PathPaver.Domain.Common;

namespace PathPaver.Domain.Entities;

public sealed class User(string? username, string email, string password, string biography = "") : BaseEntity
{
    #region Properties
    
    public string Email { get; } = email;
    public string Username { get; set; } = username ?? $"Anonymous";
    public string Biography { get; set; } = biography;
    public string Password { get; set; } = password;
    
    #endregion
    
    public string ShowInfo()
    {
        return $"{Username} - " +
               $"{Biography} - " +
               $"{Email} - ";
    }
}