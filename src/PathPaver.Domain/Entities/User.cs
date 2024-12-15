using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PathPaver.Domain.Common;

namespace PathPaver.Domain.Entities;

public sealed class User(
    string email, 
    string username, 
    string password, 
    string[] roles) : BaseEntity
{
    #region Properties
    
    public string Email { get; set; } = email;
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
    public string[] Roles { get; set; } = roles; 
    
    #endregion

    #region Methods

    // Class Methods should go here

    #endregion
}