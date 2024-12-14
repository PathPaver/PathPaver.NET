namespace PathPaver.Application.Common.Exceptions.Entities;

public class UserNotFoundException(string email, string message = "Maybe he doesn't exist") 
    : Exception($"User associated to this email {email} can't be found - {message}");
