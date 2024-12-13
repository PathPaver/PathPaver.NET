namespace PathPaver.Application.Common.Exceptions.Entities;

public class UserNotFoundException(long id, string message = "Maybe he doesn't exist") 
    : Exception($"User with id {id} can't be found - {message}");
