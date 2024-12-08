namespace PathPaver.Application.Common.Exceptions.Entities;

public class UserNotFound(long id, string message) 
    : Exception($"User with id {id} can't be found - {message}");
